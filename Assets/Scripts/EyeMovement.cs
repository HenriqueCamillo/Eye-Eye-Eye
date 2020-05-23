using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    private Rigidbody2D rBody;
    private Animator animator;
    private SpriteRenderer sRenderer;

    [Header("Roll")]
    [SerializeField] float speed;
    [SerializeField] float rollSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float accelerationTime;
    private float runStart;
    private int runDirection;


    [Header("Jump")]
    [SerializeField] float jumpForce;
    [SerializeField] float wallJumpForce;
    [SerializeField] float wallJumpTime;
    [SerializeField] float slideSpeed;
    [SerializeField] float stopSlidingTime;
    [SerializeField] Vector2 wallJumpDirection;
    
    [Header("Jump Gravity Modifiers")]
    [SerializeField] float fallMultiplier;
    [SerializeField] float lowJumpMultiplier;

    [Header("Detectors")]
    [SerializeField] Transform wallDetectorL;
    [SerializeField] Transform wallDetectorR;
    [SerializeField] Transform groundDetector;
    [SerializeField] float detectorRadius;

    private bool isGrounded;
    private bool isOnWall;
    private bool isOnLeftWall;
    private bool isOnRightWall;
    private int wallSide;
    private bool isSliding;
    private bool wallJumping;
    private bool rising;
    private bool isRunning;
    private bool preparingRun;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (rBody.velocity.y <= 0f)
        {
            rising = false;
            if (rBody.velocity.y < 0f)
                animator.SetTrigger("MidAir");
        }
        else
            animator.ResetTrigger("MidAir");


        if (rBody.velocity.x > 0.001f)
            sRenderer.flipX = false;
        else if (rBody.velocity.x < -0.001f)
            sRenderer.flipX = true;

        // Walk sideways
        float horizontalInput = Input.GetAxis("Horizontal");
        float horizontalInputRaw = Input.GetAxisRaw("Horizontal");
        RunningCheck((int)horizontalInputRaw);

        float movementSpeed;
        
        if (isRunning)
            movementSpeed = Mathf.Lerp(Mathf.Abs(rBody.velocity.x), rollSpeed, acceleration);
        else 
            movementSpeed = speed;

        if (!wallJumping)
        {
            int runningDirection = rBody.velocity.x > 0 ? 1 : rBody.velocity.x < 0 ? -1 : 0;
            Debug.Log(runningDirection + " " + horizontalInputRaw);
            if (runningDirection != horizontalInputRaw)
            {
                movementSpeed = Mathf.Lerp(rBody.velocity.x, horizontalInput * speed, acceleration);
                rBody.velocity = new Vector2(movementSpeed, rBody.velocity.y);
                Debug.Log(movementSpeed);
            }
            else
                rBody.velocity = new Vector2(horizontalInput * movementSpeed, rBody.velocity.y);
        }
        else 
            rBody.velocity = Vector2.Lerp(rBody.velocity, new Vector2(horizontalInput * movementSpeed, rBody.velocity.y), Time.deltaTime);


        // Wall and Ground detenction
        isGrounded = Physics2D.OverlapCircle(groundDetector.position, detectorRadius);
        animator.SetBool("Grounded", isGrounded);

        isOnLeftWall =  Physics2D.OverlapCircle(wallDetectorL.position, detectorRadius);
        isOnRightWall = Physics2D.OverlapCircle(wallDetectorR.position, detectorRadius);

        isOnWall = isOnLeftWall || isOnRightWall;
        wallSide = !isOnWall ? 0 : isOnLeftWall ? -1 : 1;

        if (isGrounded)
            animator.SetTrigger("Fall");
        else
            animator.ResetTrigger("Fall");

        // Checks if should slide
        if (wallSide == horizontalInputRaw)
        {
            isSliding = true;
            CancelInvoke(nameof(StopSliding));
        }
        else if (isSliding)
            Invoke(nameof(StopSliding), stopSlidingTime);

        // Wall slide
        if (isOnWall && !isGrounded && isSliding)
        {
            if (!rising)
                rBody.velocity = new Vector2(rBody.velocity.x, -slideSpeed);
            else
                rBody.velocity = Vector2.Lerp(rBody.velocity, new Vector2(rBody.velocity.x, -slideSpeed), Time.deltaTime);

            animator.SetBool("Sliding", true);

            if (isOnLeftWall)
                sRenderer.flipX = false;
            else
                sRenderer.flipX = true;
        }
        else
            animator.SetBool("Sliding", false);

        // Jump
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isGrounded)
            {
                rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);
                animator.SetTrigger("Jump");

                if (rBody.velocity.y > 0f)
                    rising = true;
            }
            else if (isOnWall && isSliding)
            {
                Vector2 jumpDirection = new Vector2(wallJumpDirection.x * -wallSide, wallJumpDirection.y);
                rBody.velocity = jumpDirection.normalized * wallJumpForce;
                StartCoroutine(SetWallJumpFlag());
                animator.ResetTrigger("MidAir");
                animator.SetTrigger("Jump");
                animator.SetBool("Sliding", false);

                if (rBody.velocity.y > 0f)
                    rising = true;
            }

        }

        ApplyJumpGravityModifiers();
    }

    private void RunningCheck(int direction)
    {
        if (!isRunning)
        {
            if (preparingRun)
            {
                if (direction != runDirection || !isGrounded)
                    preparingRun = false;
                else if (Time.time - runStart > accelerationTime)
                {
                    preparingRun = false;
                    isRunning = true;
                    animator.SetBool("Running", true);
                }
            }
            else if (direction != 0 && isGrounded)
            {
                preparingRun = true; 
                runDirection = direction; 
                runStart = Time.time;
            }
        }
        else if (runDirection != direction)
        {
            isRunning = false;
            animator.SetBool("Running", false);
        }
    }

    private IEnumerator SetWallJumpFlag()
    {
        wallJumping = true;
        yield return new WaitForSeconds(wallJumpTime);
        wallJumping = false;
    }
    
    private void StopSliding()
    {
        isSliding = false;
    }

    private void ApplyJumpGravityModifiers()
    {
        if (rBody.velocity.y < 0f)
            rBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        else if (rBody.velocity.y > 0f && !Input.GetKey(KeyCode.Z))
            rBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
    }

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.name.Equals ("Platform"))
			this.transform.parent = col.transform;
	}

	void OnCollisionExit2D(Collision2D col)
	{
		if (col.gameObject.name.Equals ("Platform"))
			this.transform.parent = null;
	}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(wallDetectorL.position, detectorRadius);
        Gizmos.DrawWireSphere(wallDetectorR.position, detectorRadius);
        Gizmos.DrawWireSphere(groundDetector.position, detectorRadius);
    }
}