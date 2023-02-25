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

    public float speed = 10f;
    private float moveHorizontal;

    private bool canDash = true;
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

    private float wallSlidingSpeed = 20f;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingDuration = 0.4f;
    private float wallJumpingCounter;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mAnimator = GetComponent<Animator>();
        int characterId = CharacterSystem.instance.characterId;
        switchId(characterId);
    }

    void disableAllBool() {
        bool isDashing = false;
        bool isLongDashing = false;
        bool isWallJumping = false;
        bool isDownBursting = false;
        bool isWallSliding = false;
        bool isJumping = false;
    }

    void switchId(int characterId) {
        if (characterId == 0) {
            canDash = true;
            canLongDash = true;
            jumpNum = 1;
        } else if (characterId == 1) {
            canDash = false;
            canWallJump = true;
            canWallSlide = true;
            canDownBurst = true;
            //jumpNum = 2;
        } else if (characterId == 2) {
            canDash = true;
            canWallJump = true;
            canWallSlide = true;
            canLongDash = true;
            canDownBurst = true;
            //jumpNum = 2;
        }
    }

    void Update() {
        if (!isLongDashing && !isDownBursting) {
            if (Input.GetButtonDown("Jump") && Input.GetKey("down")) {
                if (currentOneWayPlatform != null) {
                    StartCoroutine(DisableCollision());
                }
            }

            if (Input.GetButtonDown("Jump") && jumpNum > 0) {
                //Debug.Log("Jump" + jumpNum);
                //rb.AddForce(new Vector2(0, jumpForce));
                mAnimator.SetTrigger("Jump");
                float offset = rb.velocity.y > 0 ? 1f : 0.5f;
                rb.velocity = new Vector2(rb.velocity.x, offset * jumpForce);
                isJumping = true;
                isGrounded = false;
                jumpNum--;
            }

            if (Input.GetKeyDown(KeyCode.Z) && canLongDash && !isLongDashing) {
                disableAllBool();
                longDash();
            }

            if (Input.GetKeyDown(KeyCode.X) && canDownBurst && !isDownBursting) {
                disableAllBool();
                downBurst();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing) {
                disableAllBool();
                isDashing = true;
                dash();
            }

            if (Input.GetKeyDown(KeyCode.F)) {
                mAnimator.SetTrigger("WalkIdle");
                CharacterSystem.instance.restart();
            }

            moveHorizontal = Input.GetAxis("Horizontal");
            if (moveHorizontal < 0) {
                mAnimator.SetTrigger("StartWalk");
                isFacingRight = false;
                transform.localScale = new Vector2(-1, transform.localScale.y);
            } else if (moveHorizontal > 0) {
                mAnimator.SetTrigger("StartWalk");
                isFacingRight = true;
                transform.localScale = new Vector2(1, transform.localScale.y);
            } else {
                mAnimator.SetTrigger("WalkIdle");
            }
            isGrounded = IsGrounded();

            if (isDashing && Time.time >= dashTime) {
                isDashing = false;
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }

            if (IsGrounded()) {
                isJumping = false;
                int characterId = CharacterSystem.instance.characterId;
                jumpNum = characterId == 0 ? 1 : 2;
                mAnimator.SetTrigger("contact");
            }

            WallSlide();
            WallJump();
            if (!isWallJumping) {
                Flip();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isLongDashing || isDownBursting) {
            if (isLongDashing) {
                rb.AddForce(new Vector2(transform.localScale.x * LongDashSpeed, 0));
            } else if (isDownBursting) {
                rb.AddForce(new Vector2(0, -1 * transform.localScale.x * DownBurstSpeed));
            }
            return;
        }

        if (!isWallJumping) {
            rb.velocity = new Vector2(moveHorizontal * speed, rb.velocity.y);
        }
    }

    public void downBurst() {
        mAnimator.SetTrigger("DownSmash");
        if (!isNearGround) {
            
            rb.gravityScale = 0.0f;
            isDownBursting = true;
        }
    }

    public void longDash() {
        mAnimator.SetTrigger("Dash");
        if (!isNearWall) {
            
            rb.gravityScale = 0.0f;
            isLongDashing = true;
        }
    }

    public void dash() {
        isDashing = true;
        dashTime = Time.time + dashDuration;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(new Vector2(moveHorizontal * dashForce, 0));
        //play animation TODO
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("OneWayPlatform")) {
            currentOneWayPlatform = collision.gameObject;
        }
        //destroy gameobject if object is breakable
        if (collision.gameObject.CompareTag("Breakable")) {
            //TODO: play animation
            if (isDownBursting || isLongDashing) {
                Destroy(collision.gameObject);
            }
        }


        //stop if ground object
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 7) {
            if (isDownBursting || isLongDashing) {
                rb.velocity = new Vector2(0f, 0f);
                isDownBursting = false;
                isLongDashing = false;
                rb.gravityScale = 1.0f;
            }
            if (collision.gameObject.layer == 6) {
                isNearGround = true;
            }
            if (collision.gameObject.layer == 7) {
                isNearWall = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("OneWayPlatform")) {
            currentOneWayPlatform = null;
        }

        if (collision.gameObject.layer == 6) {
            isNearGround = false;
        }
        if (collision.gameObject.layer == 7) {
            isNearWall = false;
        }

    }

    private IEnumerator DisableCollision() {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 1f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 1f, wallLayer);
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
