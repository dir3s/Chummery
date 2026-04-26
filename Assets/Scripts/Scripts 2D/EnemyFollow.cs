using UnityEngine;

public class MitaFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(player.position.x - 1.5f, player.position.y, 0),
            speed * Time.deltaTime);
    }
}
