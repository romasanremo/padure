using UnityEngine;

public class Crouch : MonoBehaviour
{
    public CapsuleCollider playerCollider;
    public Transform cameraHolder;

    public float standHeight = 1.8f;
    public float crouchHeight = 1.0f;

    // IMPORTANT: acestea sunt poziții LOCALE ale camerei
    public float standCameraY = 1.6f;
    public float crouchCameraY = 1.25f;

    public float transitionSpeed = 8f;

    private bool isCrouching;
    private float targetHeight;
    private float targetCameraY;

    void Start()
    {
        targetHeight = standHeight;
        targetCameraY = standCameraY;

        // Asigurăm poziția inițială
        Vector3 camPos = cameraHolder.localPosition;
        camPos.y = standCameraY;
        cameraHolder.localPosition = camPos;

        playerCollider.height = standHeight;
    }

    void Update()
    {
        // Crouch
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;

            if (isCrouching)
            {
                targetHeight = crouchHeight;
                targetCameraY = crouchCameraY;
            }
            else
            {
                targetHeight = standHeight;
                targetCameraY = standCameraY;
            }
        }

        // Schimbăm înălțimea colliderului
        playerCollider.height = Mathf.Lerp(
            playerCollider.height,
            targetHeight,
            transitionSpeed * Time.deltaTime
        );

        // Schimbăm poziția camerei
        Vector3 cameraPosition = cameraHolder.localPosition;

        cameraPosition.y = Mathf.Lerp(
            cameraPosition.y,
            targetCameraY,
            transitionSpeed * Time.deltaTime
        );

        cameraHolder.localPosition = cameraPosition;
    }
}