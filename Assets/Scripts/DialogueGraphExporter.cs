using UnityEngine;
using System.Text;
using System.Collections.Generic;
using System.Linq; // Потрібно для роботи з масивами слів

public class DialogueGraphExporter : MonoBehaviour
{
    public DialogueNode startNode;

    [ContextMenu("Generate Mermaid Diagram")]
    public void ExportToMermaid()
    {
        if (startNode == null) return;

        StringBuilder mermaid = new StringBuilder();
        mermaid.AppendLine("graph TD");

        HashSet<DialogueNode> visited = new HashSet<DialogueNode>();
        Queue<DialogueNode> queue = new Queue<DialogueNode>();

        queue.Enqueue(startNode);
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            DialogueNode current = queue.Dequeue();
            string currentId = GetSafeId(current);

            // Отримуємо перші 6 слів для відображення в блоці
            string previewText = GetFirstWords(current.dialogueText, 6);
            string currentLabel = $"{current.name}<br/><i>[{previewText}...]</i>";

            if (current.choices != null && current.choices.Count > 0)
            {
                foreach (var choice in current.choices)
                {
                    if (choice.nextNode != null)
                    {
                        string nextId = GetSafeId(choice.nextNode);
                        string nextPreview = GetFirstWords(choice.nextNode.dialogueText, 6);

                        mermaid.AppendLine($"    {currentId}[\"{currentLabel}\"] -- \"{choice.answerText}\" --> {nextId}[\"{choice.nextNode.name}<br/><i>[{nextPreview}...]</i>\"]");

                        if (!visited.Contains(choice.nextNode))
                        {
                            visited.Add(choice.nextNode);
                            queue.Enqueue(choice.nextNode);
                        }
                    }
                }
            }
            else if (current.nextLinearNode != null)
            {
                string linearId = GetSafeId(current.nextLinearNode);
                string nextPreview = GetFirstWords(current.nextLinearNode.dialogueText, 6);

                mermaid.AppendLine($"    {currentId}[\"{currentLabel}\"] -- \"Далі\" --> {linearId}[\"{current.nextLinearNode.name}<br/><i>[{nextPreview}...]</i>\"]");

                if (!visited.Contains(current.nextLinearNode))
                {
                    visited.Add(current.nextLinearNode);
                    queue.Enqueue(current.nextLinearNode);
                }
            }
        }

        Debug.Log("Код Mermaid згенеровано:\n" + mermaid.ToString());
    }

    // Метод для виділення перших N слів
    private string GetFirstWords(string text, int wordCount)
    {
        if (string.IsNullOrEmpty(text)) return "Порожньо";

        // Очищаємо текст від зайвих символів, які можуть зламати Mermaid
        string cleanText = text.Replace("\"", "'").Replace("\n", " ").Replace("\r", " ");
        string[] words = cleanText.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

        return string.Join(" ", words.Take(wordCount));
    }

    private string GetSafeId(DialogueNode node)
    {
        return node.name.Replace(" ", "_").Replace("(", "").Replace(")", "").Replace("-", "_");
    }
}