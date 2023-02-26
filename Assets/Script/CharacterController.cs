using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    public float groundRadius = 0.2f;

    private CharacterSystem characterSystem;

    public float speed = 10f;
    private float moveHorizontal;

    private bool canDash = true;
    public bool canSwap = true;
    private bool canLongDash = true;
    private bool canWallJump = true;
    private bool canWallSlide = true;
    private bool canDownBurst = true;

    private bool isDashing = false;
    private bool isLongDashing = false;
    private bool isWallJumping = false;
    private bool isDownBursting = false;
    private bool isWallSliding = false;

    private float LongDashSpeed = 20f;
    private float DownBurstSpeed = 20f;
    private bool isNearWall = false;
    private bool isNearGround = true;

    public float dashForce = 500f;
    public float dashDuration = 0.5f;
    public float dashTime;

    private float specialTime;
    public float specialDuration = 2f;

    private float wallSlidingSpeed = 10f;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.4f;
    private float wallJumpingDuration = 0.8f;
    private float wallJumpingCounter;
    private Vector2 wallJumpingPower = new Vector2(2f, 8f);

    private bool isJumping = false;
    public float jumpForce = 500f;
    private bool isGrounded = true;
    private int jumpNum = 2;

    private bool isFacingRight = true;


    private Rigidbody2D rb;
    private Animator mAnimator;

    private GameObject currentOneWayPlatform;
    [SerializeField]
    private BoxCollider2D playerCollider;

    public void resetSpeed() {
        
            rb.velocity = new Vector2(0f,0f);
            mAnimator.SetTrigger("Idle");
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        characterSystem = GetComponent<CharacterSystem>();
        int characterId = characterSystem.characterId;
        switchId(characterId);
    }

    void disableAllBool()
    {
        bool isDashing = false;
        bool isLongDashing = false;
        bool isWallJumping = false;
        bool isDownBursting = false;
        bool isWallSliding = false;
        bool isJumping = false;
    }

    void switchId(int characterId)
    {
        if (characterId == 0)
        {
            canDash = true;
            canLongDash = true;
            jumpNum = 1;
        }
        else if (characterId == 1)
        {
            canDash = false;
            canWallJump = true;
            canWallSlide = true;
            canDownBurst = true;
            //jumpNum = 2;
        }
        else if (characterId == 2)
        {
            canDash = true;
            canWallJump = true;
            canWallSlide = true;
            canLongDash = true;
            canDownBurst = true;
            //jumpNum = 2;
        }
    }

    void Update()
    {
        if (!isLongDashing && !isDownBursting)
        {
            if (Input.GetButtonDown("Jump") && Input.GetKey("down"))
            {
                if (currentOneWayPlatform != null)
                {
                    StartCoroutine(DisableCollision());
                }
            }

            if (Input.GetButtonDown("Jump") && jumpNum > 0)
            {
                //Debug.Log("Jump" + jumpNum);
                //rb.AddForce(new Vector2(0, jumpForce));
                mAnimator.SetTrigger("Jump");
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                isJumping = true;
                isGrounded = false;
                jumpNum--;
            }

            if (Input.GetKeyDown(KeyCode.Z) && canLongDash && !isLongDashing)
            {
                disableAllBool();
                longDash();
            }

            if (Input.GetKeyDown(KeyCode.X) && canDownBurst && !isDownBursting)
            {
                disableAllBool();
                downBurst();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing)
            {
                disableAllBool();
                isDashing = true;
                dash();
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                mAnimator.SetTrigger("WalkIdle");
                characterSystem.restart();
            }

            if (Input.GetKeyDown(KeyCode.Q) && canSwap)
            {
                CameraFollow.instance.swapTarget();
            }

            moveHorizontal = Input.GetAxis("Horizontal");
            if (moveHorizontal < 0)
            {
                mAnimator.SetTrigger("StartWalk");
                isFacingRight = false;
                transform.localScale = new Vector2(-1.75f, transform.localScale.y);
            }
            else if (moveHorizontal > 0)
            {
                mAnimator.SetTrigger("StartWalk");
                isFacingRight = true;
                transform.localScale = new Vector2(1.75f, transform.localScale.y);
            }
            else
            {
                mAnimator.SetTrigger("WalkIdle");
            }
            isGrounded = IsGrounded();

            if (isDashing && Time.time >= dashTime)
            {
                isDashing = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }



            if (IsGrounded())
            {
                isJumping = false;
                int characterId = characterSystem.characterId;
                jumpNum = characterId == 0 ? 1 : 2;
                mAnimator.SetTrigger("Contact");
            }
            else
            {
                mAnimator.ResetTrigger("Contact");
            }

            WallSlide();
            WallJump();
            if (!isWallJumping)
            {
                Flip();
            }
        }
        else
        {
            if (isDownBursting && Time.time >= specialTime)
            {
                isDownBursting = false;
                rb.velocity = new Vector2(0, 0);
                rb.gravityScale = 1.0f;
            }

            if (isLongDashing && Time.time >= specialTime)
            {
                isLongDashing = false;
                rb.velocity = new Vector2(0, 0);
                rb.gravityScale = 1.0f;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLongDashing || isDownBursting)
        {
            if (isLongDashing)
            {
                //rb.AddForce(new Vector2(transform.localScale.x * LongDashSpeed, 0));
                rb.velocity = new Vector2(transform.localScale.x * LongDashSpeed, 0);
            }
            else if (isDownBursting)
            {
                //rb.AddForce(new Vector2(0, -1 * transform.localScale.x * DownBurstSpeed));
                rb.velocity = new Vector2(0, -1 * Mathf.Abs(transform.localScale.x) * DownBurstSpeed);
            }
            return;
        }

        if (!isWallJumping)
        {
            rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
        }
    }

    public void downBurst()
    {
        specialTime = Time.time + specialDuration;
        mAnimator.SetTrigger("DownSmash");
        if (!isNearGround)
        {
            rb.gravityScale = 0.0f;
            isDownBursting = true;
        }
    }

    public void longDash()
    {
        specialTime = Time.time + specialDuration;
        mAnimator.SetTrigger("Dash");
        if (!isNearWall)
        {
            Debug.Log("hi");
            rb.gravityScale = 0.0f;
            isLongDashing = true;
        }
    }

    public void dash()
    {
        isDashing = true;
        dashTime = Time.time + dashDuration;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(moveHorizontal * dashForce, 0));
        //play animation TODO
    }

    public void StopMovement()
    {
        moveHorizontal = 0;
        rb.velocity = new Vector2(0f, 0f);
        mAnimator.SetTrigger("WalkIdle");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
        //destroy gameobject if object is breakable
        if (collision.gameObject.CompareTag("Breakable"))
        {
            //Debug.Log("hello");
            //TODO: play animation
            if (isDownBursting || isLongDashing)
            {
                Destroy(collision.gameObject);
            }
        }


        //stop if ground object
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 7)
        {

            if (isDownBursting || isLongDashing)
            {
                //Debug.Log("yooo");
                rb.velocity = new Vector2(0f, 0f);
                isDownBursting = false;
                isLongDashing = false;
                rb.gravityScale = 1.0f;
            }
            if (collision.gameObject.layer == 6)
            {
                isNearGround = true;
            }
            if (collision.gameObject.layer == 7)
            {
                isNearWall = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }

        if (collision.gameObject.layer == 6)
        {
            isNearGround = false;
        }
        if (collision.gameObject.layer == 7)
        {
            isNearWall = false;
        }

    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.25f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && moveHorizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            mAnimator.SetTrigger("ContactWall");
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            //Debug.Log("Jumpjump");
            mAnimator.SetTrigger("WallJump");
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && moveHorizontal < 0f || !isFacingRight && moveHorizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
