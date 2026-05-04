using UnityEngine;

public class RunningWallDoor : MonoBehaviour
{
    public Transform player;

    [Header("Trigger")]
    public float triggerDistance = 6f;

    [Header("Movement")]
    public float moveDistance = 5f;
    public float moveSpeed = 2f;
    public int maxEscapes = 5;

    [Header("Door Pivot")]
    public Transform doorPivot;
    public float openAngle = 90f;
    public float openSpeed = 2f;

    private bool isOpening = false;
    private int escapeCount = 0;

    private Quaternion closedRot;
    private Quaternion openRot;

    private Vector3 startPos;
    private Vector3 targetPos;

    private float t = 0f;
    private bool isMoving = false;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        closedRot = doorPivot.localRotation;

        // 💀 ВОТ ГЛАВНАЯ ИСПРАВЛЕННАЯ СТРОКА
        openRot = closedRot * Quaternion.Euler(0f, -openAngle, 0f);
    }

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (escapeCount >= maxEscapes)
        {
            isOpening = true;
        }

        if (isOpening)
        {
            OpenDoor();
            return;
        }

        if (!isMoving && distance < triggerDistance)
        {
            StartMove();
        }

        if (isMoving)
        {
            MoveSmooth();
        }
    }

    void StartMove()
    {
        isMoving = true;

        startPos = transform.position;

        Vector3 dir = transform.right;

        targetPos = startPos + dir * moveDistance;

        t = 0f;

        escapeCount++;
    }

    void MoveSmooth()
    {
        t += Time.deltaTime * moveSpeed;

        float smoothT = Mathf.SmoothStep(0f, 1f, t);

        transform.position = Vector3.Lerp(startPos, targetPos, smoothT);

        if (smoothT >= 1f)
        {
            isMoving = false;
        }
    }

    void OpenDoor()
    {
        doorPivot.localRotation = Quaternion.Slerp(
            doorPivot.localRotation,
            openRot,
            Time.deltaTime * openSpeed
        );
    }
}