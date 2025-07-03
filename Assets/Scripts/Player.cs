using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float move_speed;
    private float horizontalInput;

    public float jump_force;

    public Transform ground_check;
    public float ground_check_radius;
    public LayerMask ground_layer;
    private bool isGrounded;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(ground_check.position, ground_check_radius, ground_layer);

        horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity=new Vector2(horizontalInput*move_speed, rb.velocity.y);
    
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump_force);
        }

        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(15f, transform.localScale.y, transform.localScale.z);
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-15f, transform.localScale.y, transform.localScale.z);
        }

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("Speed",Mathf.Abs(rb.velocity.x));
    }
}
