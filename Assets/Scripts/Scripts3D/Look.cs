using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    public Transform playerBody;
    public Transform cameraPivot;

    public float sensitivity = 1f;

    float xRotation;

    private Vector2 lookInput;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
    }

    Vector2 LookInput()
    {
        var mouse = Mouse.current;

        if (mouse == null) return Vector2.zero;


        return mouse.delta.ReadValue() * 0.002f;
    }
}