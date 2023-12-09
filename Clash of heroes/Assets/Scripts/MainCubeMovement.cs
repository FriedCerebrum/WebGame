using UnityEngine;

public class MainCubeMovement : MonoBehaviour
{
    public float moveForce = 10f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, groundLayer);

        float horizontalInput = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(horizontalInput, 0);

        rb.AddForce(movement * moveForce);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
