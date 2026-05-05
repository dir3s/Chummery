using UnityEngine;
using System.Collections;

public class NodeLine3D : MonoBehaviour
{
    public string nodeID;

    [Header("Renderer")]
    public MeshRenderer meshRenderer;

    [Header("Materials")]
    public Material visitedMaterial;
    public Material unvisitedMaterial;

    void OnEnable()
    {
        StartCoroutine(InitWhenReady());
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
        bool visited = DialogueSaveSystem.Instance.IsVisited(nodeID);

        Debug.Log("LINE3D: " + nodeID + " visited = " + visited);

        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer " + gameObject.name);
            return;
        }

        meshRenderer.material = visited ? visitedMaterial : unvisitedMaterial;
    }
}