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

    [SerializeField] private float dashSpeed = 10f;   // Dash speed
    [SerializeField] private float dashCooldown = 3f; // Dash cooldown
    private float lastDashTime = -3f; // last Dash time
    private bool isDashing = false;
    private Vector3 currentMoveDirection;
    [SerializeField] private MoveIndicator moveIndicator;  // Reference to MoveIndicator script

    private void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        inputManager.OnSpacePressed.AddListener(PlayerJump);
        inputManager.OnMove.AddListener(MovePlayer);  // Subscribe to movement event

        // Calculate the jump force based on the desired jump height
        jumpForce = Mathf.Sqrt(2 * Mathf.Abs(Physics.gravity.y) * jumpHeight);
        inputManager.OnRightClick.AddListener(Dash);  
    }

    private void OnCollisionEnter(Collision other)
    {
        // Reset the jump count when the player touches the ground
        if (other.gameObject.CompareTag("Ground"))
        {
            jumpCount = 0; // Allow second jump when on the ground
        }

        if (other.gameObject.tag == "Coin")
        {
            Destroy(other.gameObject);
        }
    }

    private void PlayerJump()
    {
        if (jumpCount < maxJumps) // Check if the player can jump
        {
                // Reset vertical velocity before jumping to ensure consistent height
                playerRB.linearVelocity = new Vector3(playerRB.linearVelocity.x, 0f, playerRB.linearVelocity.z);
                playerRB.AddForce(transform.up * jumpForce, ForceMode.Impulse); // Apply force to achieve desired jump height
                jumpCount++; // Increment the jump count after each jump
        }
    }

    private void MovePlayer(Vector3 direction)
    {
        if (!isDashing) // Prevent movement during dash
        {
            currentMoveDirection = direction;  // Update current move direction
            Vector3 movement = direction * moveSpeed * Time.deltaTime;
            transform.Translate(movement);
        }
    }

    private void Dash(Vector3 dashDirection)
    {
        if (Time.time >= lastDashTime + dashCooldown) // Check if dash is off cooldown
        {
            Debug.Log("Dash triggered"); // Debug log to check if dash is triggered
            isDashing = true;  // Set dashing flag to true

            // If there's no movement, use the camera's forward direction as dash direction
            if (currentMoveDirection == Vector3.zero)
            {
                // Get the camera's forward direction and apply dash speed
                dashDirection = moveIndicator.GetMovementDirection(Vector3.forward); // Camera's forward direction
            }

            // Use current move direction or camera's forward direction for the dash
            playerRB.linearVelocity = new Vector3(dashDirection.x * dashSpeed, playerRB.linearVelocity.y, dashDirection.z * dashSpeed);

            lastDashTime = Time.time; // Update the last dash time

            Invoke("ResetDashing", 0.2f);
            inputManager.UpdateDashText(false);  // Dash on cooldown (gray color)
            Invoke("RestoreDashText", dashCooldown);  // Restore dash text after cooldown
        }
        else
        {
            Debug.Log("Dash on cooldown"); // Debug log to check if dash is on cooldown
        }
    }

    public void RestoreDashText()
    {
        // Automatically restore text color after cooldown
        inputManager.UpdateDashText(true);  // Dash is available, change text color to white
    }


    private void ResetDashing()
    {
        isDashing = false;  // Reset dashing flag after dash is complete
    }

}
