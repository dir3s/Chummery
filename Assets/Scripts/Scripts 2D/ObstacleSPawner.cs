using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab;
    public float spawnRate = 2f;
    public float spawnY = -2f;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), 1f, spawnRate);
    }

    void Spawn()
    {
        Instantiate(obstaclePrefab,
            new Vector3(10f, spawnY, 0),
            Quaternion.identity);
    }
}