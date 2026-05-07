using System.IO;
using UnityEngine;

public static class SaveLastNode
{
    private static string path => Application.persistentDataPath + "/dialogueLast.json";

    public static void Save(string nodeID)
    {
        DialogueSaveData data = new DialogueSaveData();
        data.lastNodeID = nodeID;

        File.WriteAllText(path, JsonUtility.ToJson(data));
    }

    public static string Load()
    {
        if (!File.Exists(path)) return null;

        return JsonUtility.FromJson<DialogueSaveData>(
            File.ReadAllText(path)
        ).lastNodeID;
    }

    public static void DeleteSave()
    {
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("Save deleted");
        }
    }
}