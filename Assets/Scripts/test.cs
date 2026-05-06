using UnityEngine;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO; // Додано для роботи з файлами

public class test : MonoBehaviour
{
    public DialogueNode startNode;
    public string fileName = "DialogueGraph.mmd"; // Назва файлу

    [ContextMenu("Generate Mermaid Diagram")]
    public void ExportToMermaid()
    {
        if (startNode == null)
        {
            Debug.LogError("Start Node не призначено!");
            return;
        }

        StringBuilder mermaid = new StringBuilder();
        mermaid.AppendLine("graph TD");

        HashSet<DialogueNode> visited = new HashSet<DialogueNode>();
        Queue<DialogueNode> queue = new Queue<DialogueNode>();

        // Списки для розділення описів та зв'язків (економить місце у файлі)
        List<string> nodeDefinitions = new List<string>();
        List<string> connections = new List<string>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            DialogueNode current = queue.Dequeue();
            string currentId = GetSafeId(current);

            // Створюємо опис вузла (один раз для кожного)
            string previewText = GetFirstWords(current.dialogueText, 5);
            nodeDefinitions.Add($"    {currentId}[\"{current.name}<br/><i>[{previewText}...]</i>\"]");

            // Обробляємо вибори
            if (current.choices != null && current.choices.Count > 0)
            {
                foreach (var choice in current.choices)
                {
                    if (choice.nextNode != null)
                    {
                        string nextId = GetSafeId(choice.nextNode);
                        connections.Add($"    {currentId} -- \"{choice.answerText}\" --> {nextId}");

                        if (visited.Add(choice.nextNode))
                        {
                            queue.Enqueue(choice.nextNode);
                        }
                    }
                }
            }
            // Обробляємо лінійний перехід
            else if (current.nextLinearNode != null)
            {
                string linearId = GetSafeId(current.nextLinearNode);
                connections.Add($"    {currentId} -- \"Далі\" --> {linearId}");

                if (visited.Add(current.nextLinearNode))
                {
                    queue.Enqueue(current.nextLinearNode);
                }
            }
        }

        // Збираємо фінальний текст: спочатку всі вузли, потім всі стрілки
        foreach (var def in nodeDefinitions) mermaid.AppendLine(def);
        foreach (var conn in connections) mermaid.AppendLine(conn);

        // Запис у файл
        string path = Path.Combine(Application.dataPath, fileName);
        try
        {
            File.WriteAllText(path, mermaid.ToString());
            Debug.Log($"<b>[Mermaid Export]</b> Граф успішно збережено: <color=lime>{path}</color>");

            // Додатково виводимо в консоль короткий звіт
            Debug.Log($"Згенеровано вузлів: {visited.Count}. Тепер просто відкрий файл у папці Assets.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Помилка при записі файлу: {e.Message}");
        }
    }

    private string GetFirstWords(string text, int wordCount)
    {
        if (string.IsNullOrEmpty(text)) return "...";
        string cleanText = text.Replace("\"", "'").Replace("\n", " ").Replace("\r", " ");
        string[] words = cleanText.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        return string.Join(" ", words.Take(wordCount));
    }

    private string GetSafeId(DialogueNode node)
    {
        // Додаємо HashCode до ID, щоб уникнути конфліктів, якщо назви нод однакові
        return $"{node.name.Replace(" ", "_").Replace("(", "").Replace(")", "").Replace("-", "_")}_{node.GetHashCode()}";
    }
}