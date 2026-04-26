using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndDoor : MonoBehaviour
{
    public string nextSceneName = "Running 3D";

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TOUCHED: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Level Complete!");
            SceneManager.LoadScene(nextSceneName);
        }
    }
}