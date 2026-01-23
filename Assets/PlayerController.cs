using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    float movementX;
    float movementY;
    float movementR;

    [SerializeField] float speed = 5.0f;
    [SerializeField] float jumpForce = 5.0f;
    [SerializeField] float torqueForce = 50f;
    Rigidbody2D rb;
    bool isGrounded;
    int score = 0;

    Animator animator;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // float movementDistanceX = movementX * speed * Time.deltaTime;
        // float movementDistanceY = movementY * speed * Time.deltaTime;
        // transform.position = new Vector2(transform.position.x + movementDistanceX, transform.position.y + movementDistanceY);
        rb.linearVelocity = new Vector2(movementX*speed, rb.linearVelocity.y);
        if (!Mathf.Approximately(movementX, 0f))
        {
            animator.SetBool("isRunning", true);
            spriteRenderer.flipX = movementX < 0;
        } else
        {
            animator.SetBool("isRunning", false);
        }
        if (movementY > 0f)
        {
            animator.SetBool("isJumping", true);
        } else
        {
            animator.SetBool("isJumping", false);
        }
        if (isGrounded && movementY > 0)
        {
            // rb.AddForce(new Vector2(0, 100));
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
        rb.AddTorque(-movementR * torqueForce);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // If the surface normal points up (y > 0.5), it's a floor
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;
                    break;
                }
            }
        }

        if (collision.gameObject.CompareTag("Sticky Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Sticky Ground"))
        {
            isGrounded = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.CompareTag("Collectible"))
        {
            score++;
            Destroy(collision.gameObject);
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

    void OnRotate(InputValue value) {
        movementR = value.Get<float>();
        Debug.Log("Movement R = " + movementR);
    }
}

