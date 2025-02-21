using UnityEngine;
using Unity.Cinemachine;

public class MoveIndicator : MonoBehaviour
{
    [SerializeField] private CinemachineCamera freeLookCamera;  // Reference to the Cinemachine FreeLook camera

    // Method to get the movement direction relative to the camera's orientation
    public Vector3 GetMovementDirection(Vector3 inputDirection)
    {
        // Get the forward and right direction of the camera
        Vector3 forward = freeLookCamera.transform.forward;
        Vector3 right = freeLookCamera.transform.right;

        // Remove vertical component to keep movement on the ground plane
        forward.y = 0;
        right.y = 0;

        // Normalize directions
        forward.Normalize();
        right.Normalize();

        // Calculate movement direction based on the camera's orientation
        Vector3 movementDirection = forward * inputDirection.z + right * inputDirection.x;

        return movementDirection;
    }
}
