using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    
    public Transform groundCheck;
    public float groundRadius = 0.2f;

    public float speed = 10f;
    private float moveHorizontal;

    private bool canDash = true;

    private bool isDashing = false;
    public float dashForce = 500f;
    public float dashDuration = 0.5f;
    public float dashTime;
    private bool isJumping = false;
    public float jumpForce = 500f;
    private bool isGrounded = true;
    private int jumpNum = 2;
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
        if (characterId == 0) {
            canDash = true;
            jumpNum = 1;
        } else if (characterId == 1) {
            canDash = false;
        } else if (characterId == 2) {
            canDash = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, 5);
        moveHorizontal = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(moveHorizontal, 0);
        rb.velocity = new Vector2(movement.x * speed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && jumpNum > 0) {
            rb.AddForce(new Vector2(0, jumpForce));
            isJumping = true;
            isGrounded = false;
            jumpNum--;
        }

        if (Input.GetButtonDown("Jump") && Input.GetKey("down")) {
            if (currentOneWayPlatform != null) {
                StartCoroutine(DisableCollision());
            }
        }

        if (isDashing && Time.time >= dashTime) {
            isDashing = false;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        if (isGrounded) {
            isJumping = false;
            int characterId = CharacterSystem.instance.characterId;
            jumpNum = characterId == 0 ? 1 : 2;
            //mAnimator.SetTrigger("contact");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && !isDashing) {
            dash();
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
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("OneWayPlatform")) {
            currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision() {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    
}
