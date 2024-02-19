using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int moveSpeed;

    private bool isGrounded;
    [SerializeField] private float jumpVelocity;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    private bool hovering;
    [SerializeField] private float hoverFallSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Walk();
        Jump();
        Hover();
    }

    private void Jump()
    {
        if (Input.GetKeyDown("space") && isGrounded)
        {
            rb.velocity = Vector2.up * jumpVelocity;
            isGrounded = false;
            animator.SetTrigger("jump");
            animator.SetBool("isGrounded", isGrounded);
        }
        if (rb.velocity.y < 0 && !hovering)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1);
        }
        else if (rb.velocity.y > 0 && !Input.GetKey("space"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1);
        }
        animator.SetFloat("verticalSpeed", rb.velocity.y);
        animator.SetBool("hovering", hovering);
    }

    private void Hover()
    {
        if (rb.velocity.y < 0 && Input.GetKey("space"))
        {
            hovering = true;
            rb.velocity = new Vector2(rb.velocity.x, hoverFallSpeed);
        } else
        {
            hovering = false;
        }
    }

    private void Walk()
    {
        rb.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), rb.velocity.y);
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } else if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        animator.SetFloat("horizontalSpeed", Mathf.Abs(rb.velocity.x));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isGrounded = true;
            animator.SetBool("isGrounded", isGrounded);
        }
    }
}