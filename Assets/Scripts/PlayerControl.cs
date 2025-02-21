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
        inputManager.OnMove.AddListener(MovePlayer);  // Subscribe to movement event

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
        if (jumpCount < maxJumps) // Check if the player can jump
        {
            // Allow second jump only if the first jump has completed (when velocity.y <= 0)
            if (jumpCount == 0 || (jumpCount == 1 && playerRB.linearVelocity.y <= 0f))
            {
                // Reset vertical velocity before jumping to ensure consistent height
                playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x, 0f, playerRB.linearVelocity.z);
                playerRB.AddForce(transform.up * jumpForce, ForceMode.Impulse); // Apply force to achieve desired jump height
                jumpCount++; // Increment the jump count after each jump
            }
        }
    }

    private void MovePlayer(Vector3 direction)
    {
        // Apply movement in the X and Z direction without affecting vertical velocity
        Vector3 movement = direction * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }
}
