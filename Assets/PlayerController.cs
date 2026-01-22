using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float movementX;
    float movementY;

    [SerializeField] float speed = 5.0f;
    Rigidbody2D rb;
    bool isGrounded;
    int score = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // float movementDistanceX = movementX * speed * Time.deltaTime;
        // float movementDistanceY = movementY * speed * Time.deltaTime;
        // transform.position = new Vector2(transform.position.x + movementDistanceX, transform.position.y + movementDistanceY);
        rb.linearVelocity = new Vector2(movementX*speed, rb.linearVelocity.y);
        if (isGrounded && movementY > 0)
        {
            rb.AddForce(new Vector2(0, 100));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Collectible"))
        {
            score++;
            collision.gameObject.SetActive(false);
            Debug.Log("Score: " + score);
        }
    }

    void OnMove(InputValue value) {
        Vector2 v = value.Get<Vector2>();
        movementX = v.x;
        movementY = v.y;
        Debug.Log("Movement X = " + movementX);
        Debug.Log("Movement Y = " + movementY);
    }
}

