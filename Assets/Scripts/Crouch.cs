using UnityEngine;

public class Crouch : MonoBehaviour
{
    public CapsuleCollider playerCollider;
    public Transform cameraHolder;

    public float standHeight = 1.8f;
    public float crouchHeight = 1.0f;
    public float standCameraY = 1.6f;
    public float crouchCameraY = 0.9f;
    public float transitionSpeed = 8f;

    private bool isCrouching = false;
    private float targetHeight;
    private float targetCameraY;

    void Start()
    {
        targetHeight = standHeight;
        targetCameraY = standCameraY;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            targetHeight = isCrouching ? crouchHeight : standHeight;
            targetCameraY = isCrouching ? crouchCameraY : standCameraY;
        }

  
        float newHeight = Mathf.Lerp(playerCollider.height, targetHeight, transitionSpeed * Time.deltaTime);
        playerCollider.height = newHeight;

    
        Vector3 newCenter = playerCollider.center;
        newCenter.y = newHeight / 2f;
        playerCollider.center = newCenter;

        Vector3 camPos = cameraHolder.localPosition;
        camPos.y = Mathf.Lerp(camPos.y, targetCameraY, transitionSpeed * Time.deltaTime);
        cameraHolder.localPosition = camPos;
    }
}