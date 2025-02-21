using UnityEngine;

public class SpinObject : MonoBehaviour
{

    [SerializeField] private float speed = 100f; // Base speed of rotation
    public int scoreValue = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.Rotate(Vector3.up, speed * Time.deltaTime);
        //transform.Rotate(Vector3.right, 2 * speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("player"))
        {
            InputManager.Instance.AddScore(scoreValue);
        }
    }

}
