using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private float jumpHeight = 1f; // Maximum jump height
    [SerializeField] private float moveSpeed = 3f;  // Speed of movement
    [SerializeField] private InputManager inputManager;
    private Rigidbody playerRB;
    private int jumpCount = 0; // To track the number of jumps
    private int maxJumps = 2; // Maximum number of jumps (double jump)

    private float jumpForce; // Calculated based on desired jump height
    private bool canDoubleJump = false; // Flag to track if second jump is allowed

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        inputManager.OnSpacePressed.AddListener(PlayerJump);
        inputManager.OnMove.AddListener(PlayerMove);  // Subscribe to movement event

        // Calculate the jump force based on the desired jump height
        jumpForce = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight);
    }

    private void OnCollisionEnter(Collision other)
    {
        // Reset the jump count when the player touches the ground
        if (other.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0;
            canDoubleJump = true; // Allow second jump when on the ground
        }
    }

    private void PlayerJump()
    {
        // Allow jumping only if jump count is less than the max jumps
        if (jumpCount < maxJumps)
        {
            // Allow second jump only if first jump has completed (when velocity.y <= 0)
            if (jumpCount == 0 || (jumpCount == 1 && playerRB.linearVelocity.y <= 0f))
            {
                // Reset vertical velocity before jumping to ensure consistent height
                playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x, 0f, playerRB.linearVelocity.z);
                playerRB.AddForce(transform.up * jumpForce, ForceMode.Impulse); // Apply force to achieve desired jump height
                jumpCount++; // Increment the jump count after each jump
            }
        }
    }

    private void PlayerMove(Vector3 direction)
    {
            Vector3 movement = direction * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
    }
}
