using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;

    [Header("Jump")]
    public float jumpForce = 5f;

    [Header("Ground")]
    public LayerMask groundLayer;
    public float groundDistance = 0.2f;

    [Header("Player")]
    public float normalHeight = 2f;

    private Rigidbody rb;
    private CapsuleCollider capsule;

    private bool grounded;
    private Vector2 input;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();

        rb.freezeRotation = true;

        capsule.height = normalHeight;
    }

    private void Update()
    {
        GetInput();
        CheckGround();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void GetInput()
    {
        input = Vector2.zero;

        if (Keyboard.current == null)
            return;

        if (Keyboard.current.wKey.isPressed)
            input.y += 1;

        if (Keyboard.current.sKey.isPressed)
            input.y -= 1;

        if (Keyboard.current.aKey.isPressed)
            input.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            input.x += 1;

        input = Vector2.ClampMagnitude(input, 1f);

        // Jump
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump();
        }
    }

    private void Move()
    {
        float speed = walkSpeed;

        // Run
        if (Keyboard.current != null &&
            Keyboard.current.leftShiftKey.isPressed &&
            input.y > 0)
        {
            speed = runSpeed;
        }

        Vector3 direction =
            transform.forward * input.y +
            transform.right * input.x;

        direction.Normalize();

        Vector3 velocity = rb.linearVelocity;

        velocity.x = direction.x * speed;
        velocity.z = direction.z * speed;

        rb.linearVelocity = velocity;
    }

    private void Jump()
    {
        if (!grounded)
            return;

        Vector3 velocity = rb.linearVelocity;

        velocity.y = 0f;

        rb.linearVelocity = velocity;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void CheckGround()
    {
        Vector3 start = capsule.bounds.center;

        float distance =
            capsule.bounds.extents.y + groundDistance;

        grounded = Physics.Raycast(
            start,
            Vector3.down,
            distance,
            groundLayer
        );

        Debug.DrawRay(
            start,
            Vector3.down * distance,
            grounded ? Color.green : Color.red
        );
    }
}
