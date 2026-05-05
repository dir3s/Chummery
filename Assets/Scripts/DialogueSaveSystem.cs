using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class DialogueSaveSystem : MonoBehaviour
{
    public static DialogueSaveSystem Instance;

    private HashSet<string> visitedNodes = new HashSet<string>();
    private string savePath;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Application.persistentDataPath + "/nodes.json";

        Load();
    }


    public void MarkVisited(string nodeID)
    {
        if (!visitedNodes.Contains(nodeID))
        {
            visitedNodes.Add(nodeID);
            Save();
        }
    }

    public bool IsVisited(string nodeID)
    {
        return visitedNodes.Contains(nodeID);
    }

    void Save()
    {
        string json = JsonUtility.ToJson(new SaveData(visitedNodes));
        File.WriteAllText(savePath, json);
    }

    void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            visitedNodes = new HashSet<string>(data.nodes);
        }
    }

    [System.Serializable]
    class SaveData
    {
        public List<string> nodes;

        public SaveData(HashSet<string> set)
        {
            nodes = new List<string>(set);
        }
    }
}