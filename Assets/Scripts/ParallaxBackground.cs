using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cameraTransform;

    [Range(0f, 1f)]
    public float parallaxStrength = 0.5f;

    private Vector3 lastCameraPosition;

    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;
    }

    private void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCameraPosition;

        transform.position += new Vector3(delta.x * parallaxStrength, 0f, 0f);

        lastCameraPosition = cameraTransform.position;
    }
}