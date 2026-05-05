using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class MenuCameraController : MonoBehaviour
{
    public static MenuCameraController Instance;

    public float moveSpeed = 5f;
    public bool canMove = false;

    [Header("Points")]
    public Transform menuPoint;
    public Transform storyPoint;

    [Header("Bounds")]
    public Transform background;

    private float minX;
    private float maxX;

    private Coroutine moveRoutine;
    private bool inputLocked;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CalculateBounds();

        if (menuPoint != null)
            transform.position = menuPoint.position;
    }

    private void Update()
    {
        if (!canMove || inputLocked) return;

        float move = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed)
                move = -1f;
            else if (Keyboard.current.rightArrowKey.isPressed)
                move = 1f;
        }

        if (move == 0f) return;

        Vector3 pos = transform.position;
        pos.x += move * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }

    public void MoveToLeftPanel()
    {
        if (storyPoint == null) return;
        StartMove(storyPoint.position);
    }

    public void MoveBack()
    {
        if (menuPoint == null) return;
        StartMove(menuPoint.position);
    }

    void StartMove(Vector3 target)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        canMove = false;
        inputLocked = true;

        moveRoutine = StartCoroutine(MoveCamera(target));
    }

    private IEnumerator MoveCamera(Vector3 target)
    {
        float t = 0f;
        Vector3 startPos = transform.position;

        float y = startPos.y;
        float z = startPos.z;

        Vector3 finalTarget = new Vector3(target.x, y, z);

        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            float x = Mathf.Lerp(startPos.x, finalTarget.x, t);
            transform.position = new Vector3(x, y, z);
            yield return null;
        }

        transform.position = finalTarget;

        yield return new WaitUntil(() =>
            Keyboard.current == null ||
            (!Keyboard.current.leftArrowKey.isPressed && !Keyboard.current.rightArrowKey.isPressed)
        );

        inputLocked = false;
        canMove = true;
        moveRoutine = null;
    }

    void CalculateBounds()
    {
        if (background == null) return;

        Renderer rend = background.GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("No Renderer on background!");
            return;
        }

        Bounds bounds = rend.bounds;
        minX = bounds.min.x;
        maxX = bounds.max.x;

        if (Camera.main != null && Camera.main.orthographic)
        {
            float camHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
            minX += camHalfWidth;
            maxX -= camHalfWidth;
        }
    }
}