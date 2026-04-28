using UnityEngine;

public class CorridorSegmentTwist : MonoBehaviour
{
    public Transform player;

    public float twistStrength = 3f; 
    public float distanceMultiplier = 1f;

    private Vector3 startRotation;

    void Start()
    {
        startRotation = transform.localEulerAngles;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        float distance = Vector3.Dot(
            transform.position - player.position,
            player.forward
        );

        float angle = distance * twistStrength * 0.1f;

        transform.rotation = Quaternion.AngleAxis(
        angle,
        player.forward 
    ) * Quaternion.Euler(startRotation);
    }
}