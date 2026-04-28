using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    public Transform playerBody;
    public Transform cameraPivot;

    public float sensitivity = 1f;

    float xRotation;

    private Vector2 lookInput;


    [Header("Head Bob")]
    public float walkBobSpeed = 8f;
    public float walkBobAmount = 0.05f;

    public float runBobSpeed = 12f;
    public float runBobAmount = 0.1f;

    private float bobTimer;
    private Vector3 initialCamPos;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        initialCamPos = cameraPivot.localPosition;
    }

    void Update()
    {
        lookInput = LookInput();

        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        HandleHeadBob();
    }

    Vector2 LookInput()
    {
        var mouse = Mouse.current;

        if (mouse == null) return Vector2.zero;


        return mouse.delta.ReadValue() * 0.002f;
    }


    void HandleHeadBob()
    {
        var keyboard = Keyboard.current;

        bool isMoving = keyboard.wKey.isPressed || keyboard.aKey.isPressed ||
                        keyboard.sKey.isPressed || keyboard.dKey.isPressed;

        if (!isMoving)
        {
            
            cameraPivot.localPosition = Vector3.Lerp(
                cameraPivot.localPosition,
                initialCamPos,
                Time.deltaTime * 5f
            );
            return;
        }

        bool isRunning = keyboard.leftShiftKey.isPressed;

        float speed = isRunning ? runBobSpeed : walkBobSpeed;
        float amount = isRunning ? runBobAmount : walkBobAmount;

        bobTimer += Time.deltaTime * speed;

        float bobY = Mathf.Sin(bobTimer) * amount;
        float bobX = Mathf.Cos(bobTimer / 2f) * amount;

        cameraPivot.localPosition = initialCamPos + new Vector3(bobX, bobY, 0f);
    }

}