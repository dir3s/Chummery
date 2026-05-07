using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndDoor : MonoBehaviour
{
    public string nextSceneName = "StartScene";

    [Header("Dialogue")]
    public DialogueNode nodeToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Debug.Log("Level Complete!");

        if (nodeToLoad != null)
        {
            SaveLastNode.Save(nodeToLoad.nodeID);
        }

        SceneManager.LoadScene(nextSceneName);
    }
}