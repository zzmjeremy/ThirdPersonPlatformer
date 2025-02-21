using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent<Vector3> OnMove = new UnityEvent<Vector3>();  // Event to pass movement direction
    public UnityEvent OnSpacePressed = new UnityEvent();

    [SerializeField] private MoveIndicator moveIndicator;  // Reference to MoveIndicator script

    void Start() { }

    void Update()
    {
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
    }
}
