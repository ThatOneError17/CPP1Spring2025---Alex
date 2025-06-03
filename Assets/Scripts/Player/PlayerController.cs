using UnityEditor.Tilemaps;
using UnityEngine;

//RequireComponent can only have 3
[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(SpriteRenderer), typeof(Animator))]
[RequireComponent(typeof(Collider2D))]

public class PlayerController : MonoBehaviour
{
    [Range(3, 10)]
    public float speed = 6.0f;

    [Range(1, 20)]
    public float jumpForce = 10f;

    [Range(0.01f, 0.2f)]
    public float groundCheckRadius = 0.02f;
    public LayerMask isGroundLayer;
    public bool isGrounded;
    

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    GroundCheck groundCheck;


    //private Vector2 groundCheckPos => new Vector2(collider.bounds.min.x + collider.bounds.extents.x, collider.bounds.min.y);
    //private Transform groundCheckTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();


        groundCheck = new GroundCheck(LayerMask.GetMask("Ground"), GetComponent<Collider2D>(), rb, ref groundCheckRadius);


        //Setting all "Ground" as a layermask
        //isGroundLayer = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.isPaused)
            return; //Should ignore all other update related functions if game is paused

        //For animation stuff, gettinfo from base layer of animations, which we only have 0
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        float hInput = Input.GetAxis("Horizontal");

        groundCheck.CheckIsGrounded();

        //For checking if Fire is playing
        if (curPlayingClips.Length > 0)
        {
            if (!(curPlayingClips[0].clip.name == "Fire"))
            {
                //apply physics and mechanics
                rb.linearVelocity = new Vector2(hInput * speed, rb.linearVelocity.y);

                if (Input.GetButtonDown("Fire1") && groundCheck.IsGrounded) anim.SetTrigger("Fire");
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }


        }

        //isGrounded = CheckIsGrounded();



        float attack = Input.GetAxis("Fire1");

  



        rb.linearVelocity = new Vector2(hInput * speed, rb.linearVelocity.y);

        //Flips if hInput is less than 0, and does not equal 0, therefore it won't automatically flip to default when no input is detected
        if (hInput != 0) spriteRenderer.flipX = (hInput < 0);



        //anim.SetBool("isGrounded", CheckIsGrounded());
        anim.SetBool("isRunning", checkIsRunning());
        anim.SetBool("isAttacking", checkIsAttacking());
        anim.SetBool("isJumpAttacking", checkIsJumpAttacking());

        anim.SetBool("isGrounded", groundCheck.IsGrounded);

        if (groundCheck.IsGrounded == false && Input.GetButtonDown("Fire1")) anim.SetTrigger("JumpAttack");
       

        if (Input.GetButtonDown("Jump") && groundCheck.IsGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }


        ////Checks if grounded and returns true or false
        //bool CheckIsGrounded()
        //{
        //    //Checks if it's grounded, if false we enter
        //    if (!isGrounded)
        //    {
        //        //if we're not jumping and touching ground, will return a true
        //        if (rb.linearVelocityY <= 0)
        //        {
        //            return Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, isGroundLayer);
        //        }

        //    }
        //    //Will return a true if grounded
        //    return isGrounded = Physics2D.OverlapCircle(groundCheckPos, groundCheckRadius, isGroundLayer);
        //}

        //Checks if "running" and returns true or false
        bool checkIsRunning()
        {

            if (Mathf.Abs(hInput) > 0)
            {
                return true;
            }

            else
            {
                return false;
            }

        }







        //Checks if attacking and returns true or false
        bool checkIsAttacking()
        {
            //If attacking returns true
            if (attack > 0f)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        //Checks if attacking and jumping and returns true or false
        bool checkIsJumpAttacking()
        {
            //If attacking and not on ground will return true
            if (attack > 0f && !isGrounded)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

    }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Squish") && rb.linearVelocityY < 0)
        {
            collision.enabled = false;
            collision.gameObject.GetComponentInParent<Enemy>().TakeDamage(9999, Enemy.DamageType.JumpedOn);
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
        }
    }



}

