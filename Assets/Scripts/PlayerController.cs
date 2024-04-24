using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Values")]
    public float speed;
    public float jumpSpeed;
    public float regularGravity;
    public float fallGravity;
    
    [Header("Transforms")]
    public Transform groundCheck;

    bool onGround = false;

    Vector2 startPosition;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f)) onGround = true;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");

        anim.SetBool("isWalking", h != 0f);
        anim.SetBool("onAir", Mathf.Abs(rb.velocity.y) > 0.02f);

        spriteRenderer.flipX = h < 0;

        rb.velocity = new Vector2(h * speed, rb.velocity.y);

        if(Input.GetButtonDown("Jump") && onGround)
        {
            onGround = false;

            rb.gravityScale = regularGravity;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }

        if (rb.velocity.y < 0f && !onGround) rb.gravityScale = fallGravity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!onGround && collision.gameObject.CompareTag("Ground"))
        {
            if (Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f))
            {
                onGround = true;
                anim.SetBool("onAir", false);

                rb.gravityScale = regularGravity;
            }
        }

        if (collision.gameObject.CompareTag("Respawn"))
        {
            Debug.Log("Lose!");
            transform.position = startPosition;
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            Debug.Log("Win!");
            transform.position = startPosition;
        }
    }
}
