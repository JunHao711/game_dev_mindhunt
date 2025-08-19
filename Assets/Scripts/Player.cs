//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using UnityEngine;

//public class Player : MonoBehaviour
//{
//    private Rigidbody2D rb;
//    public float move_speed;
//    private float horizontalInput;

//    public float jump_force;

//    public Transform groundCheckLeft;
//    public Transform groundCheckRight;
//    public float checkDistance = 0.4f;
//    public LayerMask ground_layer;
//    private bool isGrounded;

//    private Animator anim;
//    private bool facingRight = false;

//    void Start()
//    {
//        rb = GetComponent<Rigidbody2D>();
//        anim = GetComponent<Animator>();
//    }

//    void Update()
//    {
//        // GameLock.InputLocked lock the motion of player
//        if (!Pause.Instance.isPaused && !GameLock.InputLocked)
//        {

//            bool leftGrounded = Physics2D.Raycast(groundCheckLeft.position, Vector2.down, checkDistance, ground_layer);
//            bool rightGrounded = Physics2D.Raycast(groundCheckRight.position, Vector2.down, checkDistance, ground_layer);
//            isGrounded = leftGrounded || rightGrounded;

//            UnityEngine.Debug.DrawRay(groundCheckLeft.position, Vector2.down * checkDistance, leftGrounded ? Color.green : Color.red);
//            UnityEngine.Debug.DrawRay(groundCheckRight.position, Vector2.down * checkDistance, rightGrounded ? Color.green : Color.red);

//            horizontalInput = Input.GetAxis("Horizontal");
//            rb.velocity = new Vector2(horizontalInput * move_speed, rb.velocity.y);

//            if (Input.GetButtonDown("Jump") && isGrounded)
//            {
//                rb.velocity = new Vector2(rb.velocity.x, jump_force);
//            }

//            if (rb.velocity.x > 0)
//            {
//                transform.localScale = new Vector3(15f, transform.localScale.y, transform.localScale.z);
//                facingRight = true;
//            }
//            else if (rb.velocity.x < 0)
//            {
//                transform.localScale = new Vector3(-15f, transform.localScale.y, transform.localScale.z);
//                facingRight = false;
//            }

//        }

//        anim.SetBool("isGrounded", isGrounded);
//        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
//    }

//    public bool isFacingRight()
//    {
//        return facingRight;
//    }
//}

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float move_speed;
    private float horizontalInput;

    public float jump_force;

    public Transform groundCheckLeft;
    public Transform groundCheckRight;
    public float checkDistance = 0.4f;
    public LayerMask ground_layer;
    private bool isGrounded;

    private Animator anim;
    private bool facingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // GameLock.InputLocked lock the motion of player
        if (!Pause.Instance.isPaused && !GameLock.InputLocked)
        {

            bool leftGrounded = Physics2D.Raycast(groundCheckLeft.position, Vector2.down, checkDistance, ground_layer);
            bool rightGrounded = Physics2D.Raycast(groundCheckRight.position, Vector2.down, checkDistance, ground_layer);
            isGrounded = leftGrounded || rightGrounded;

            UnityEngine.Debug.DrawRay(groundCheckLeft.position, Vector2.down * checkDistance, leftGrounded ? Color.green : Color.red);
            UnityEngine.Debug.DrawRay(groundCheckRight.position, Vector2.down * checkDistance, rightGrounded ? Color.green : Color.red);

            horizontalInput = 0f;
            if (PlayerAbilityLock.allowMoveLeft &&
                (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
                horizontalInput = -1f;

            if (PlayerAbilityLock.allowMoveRight &&
                (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
                horizontalInput = 1f;

            rb.velocity = new Vector2(horizontalInput * move_speed, rb.velocity.y);

            if (PlayerAbilityLock.allowJump && Input.GetButtonDown("Jump") && isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jump_force);
            }

            if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(15f, transform.localScale.y, transform.localScale.z);
                facingRight = true;
            }
            else if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-15f, transform.localScale.y, transform.localScale.z);
                facingRight = false;
            }

        }

        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }

    public bool isFacingRight()
    {
        return facingRight;
    }
}