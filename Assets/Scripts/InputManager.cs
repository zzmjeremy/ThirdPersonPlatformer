using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent<Vector3> OnMove = new UnityEvent<Vector3>();  // Event to pass movement direction
    public UnityEvent OnSpacePressed = new UnityEvent();
    public UnityEvent<Vector3> OnRightClick = new UnityEvent<Vector3>(); // Event for dash

    public static InputManager Instance;
    private int score = 0;
    public TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI dashText;  // Reference to Dash status text
    [SerializeField] private MoveIndicator moveIndicator;  // Reference to MoveIndicator script

    void Start()
    {
        UpdateScoreUI();
        //lock the cursor at the start of the game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;  // Unlock the cursor
            Cursor.visible = true;  // Show the cursor
        }

        // Space key to jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed?.Invoke();
        }

        // WASD movement input
        float moveX = Input.GetAxis("Horizontal");  // A/D or Left/Right arrows
        float moveZ = Input.GetAxis("Vertical");    // W/S or Up/Down arrows

        Vector3 movementDirection = new Vector3(moveX, 0, moveZ);  // Create a movement vector

        // Get the movement direction relative to the camera's orientation
        movementDirection = moveIndicator.GetMovementDirection(movementDirection);

        OnMove?.Invoke(movementDirection);  // Pass movement direction to PlayerControl

        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            OnRightClick?.Invoke(movementDirection);  // Pass the same movement direction to Dash
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score:" + score;
        }
    }

    public void UpdateDashText(bool isAvailable)
    {
        if (isAvailable)
        {
            dashText.text = "Press right click to Dash";
            dashText.color = Color.white;  // Normal color when dash is available
        }
        else
        {
            dashText.text = "Dash on Cooldown";
            dashText.color = Color.grey;  // Grey color when dash is on cooldown
        }
    }


}
