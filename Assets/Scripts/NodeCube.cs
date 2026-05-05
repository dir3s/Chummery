using UnityEngine;
using System.Collections;
public class NodeSprite : MonoBehaviour
{
    public string nodeID;

    [Header("Visual")]
    public SpriteRenderer spriteRenderer;

    [Range(0f, 1f)]
    public float visitedAlpha = 1f;

    [Range(0f, 1f)]
    public float unvisitedAlpha = 0.3f;

    void Awake()
    {
        Debug.Log("AWAKE WORKS: " + gameObject.name);
    }

    void OnEnable()
    {
        UpdateVisual();
    }
    void Start()
    {
        StartCoroutine(InitWhenReady());
    }
    IEnumerator InitWhenReady()
    {
        yield return new WaitUntil(() => DialogueSaveSystem.Instance != null);

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        Debug.Log("ENTER UpdateVisual: " + nodeID);

        if (DialogueSaveSystem.Instance == null)
        {
            Debug.LogError("DialogueSaveSystem == NULL");
            return;
        }

        bool visited = DialogueSaveSystem.Instance.IsVisited(nodeID);

        Debug.Log("visited = " + visited);

        Color c = spriteRenderer.color;
        c.a = visited ? visitedAlpha : unvisitedAlpha;
        spriteRenderer.color = c;
    }
}