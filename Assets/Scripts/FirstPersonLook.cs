using UnityEngine;

// Pune scriptul asta pe root-ul personajului (acelasi obiect care are Rigidbody/CharacterController)
public class FirstPersonLook : MonoBehaviour
{
    [Header("Referinte")]
    [Tooltip("Empty GameObject copil al personajului, pozitionat la nivelul ochilor")]
    public Transform eyePivot;

    [Header("Setari mouse")]
    public float mouseSensitivity = 2f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Yaw - roteste corpul personajului (aceasta rotatie e folosita si de scriptul de miscare)
        transform.Rotate(Vector3.up * mouseX);

        // Pitch - roteste doar camera, nu si corpul
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        eyePivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
