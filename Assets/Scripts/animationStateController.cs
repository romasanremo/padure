
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Viteza")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float crouchSpeed = 1f;
    
    [Header("Raycast")]
    public float rayDistance = 0.5f;
    
    [Header("Salt")]
    public float jumpForce = 5f;
    
    private Rigidbody rb;
    private bool isGrounded = true;
    private bool isCrouching = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
    {
        // Citeste input direct din tastatura cu Input System
        bool forwardPressed = Keyboard.current.wKey.isPressed;
        bool backPressed = Keyboard.current.sKey.isPressed;
        bool leftPressed = Keyboard.current.aKey.isPressed;
        bool rightPressed = Keyboard.current.dKey.isPressed;
        bool runPressed = Keyboard.current.leftShiftKey.isPressed;
        bool crouchPressed = Keyboard.current.cKey.isPressed;

        // Determina viteza
        float currentSpeed = walkSpeed;
        if (crouchPressed)
        {
            currentSpeed = crouchSpeed;
            isCrouching = true;
        }
        else if (runPressed && (forwardPressed || backPressed))
        {
            currentSpeed = runSpeed;
            isCrouching = false;
        }
        else
        {
            isCrouching = false;
        }

        // === INAINTE ===
        if (forwardPressed)
        {
            if (!IsPathBlocked(transform.forward))
                transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }

        // === INAPOI ===
        if (backPressed)
        {
            if (!IsPathBlocked(-transform.forward))
                transform.Translate(Vector3.back * currentSpeed * Time.deltaTime);
        }

        // === DREAPTA ===
        if (rightPressed)
        {
            if (!IsPathBlocked(transform.right))
                transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);
        }

        // === STANGA ===
        if (leftPressed)
        {
            if (!IsPathBlocked(-transform.right))
                transform.Translate(Vector3.left * currentSpeed * Time.deltaTime);
        }
    }

    void HandleJump()
    {
        // wasPressedThisFrame pentru detectare apasare
        if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded && !isCrouching)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    // ===== RAYCAST CHECK =====
    bool IsPathBlocked(Vector3 direction)
    {
        if (direction.magnitude < 0.1f) return false;

        return Physics.Raycast(
            transform.position + Vector3.up * 0.5f,
            direction.normalized,
            rayDistance
        );
    }
}