using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    public string sceneName;

    [Header("Dialogue")]
    public DialogueNode nodeToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (nodeToLoad != null)
        {
            SaveLastNode.Save(nodeToLoad.nodeID);
        }

        SceneManager.LoadScene(sceneName);
    }
}