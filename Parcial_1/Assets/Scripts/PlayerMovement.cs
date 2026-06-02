using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Speed of the player movement
    public float jumpForce = 5f; // Force applied when the player jumps
    public float horizontalInput; // Horizontal input from the player
    public float verticalInput; // Vertical input from the player
    private Rigidbody playerRigidbody; // Reference to the player's Rigidbody component
    public bool isGrounded = true; // Flag to check if the player is on the ground


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>(); // Get the Rigidbody component attached to the player
    }

    // Update is called once per frame
    void Update()
    {
        //player input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        //movement

        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);
        transform.Translate(Vector3.right * Time.deltaTime * speed * horizontalInput);

        //jumping

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Set isGrounded to true when the player collides with the ground
        }
    }

}
