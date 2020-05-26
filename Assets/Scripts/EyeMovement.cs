using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    private Rigidbody2D rBody;
    private Animator animator;
    private SpriteRenderer sRenderer;
	private AudioSource tickSource;
    private Player_Health health;

    [Header("Roll")]
    [SerializeField] float speed;
    [SerializeField] float rollSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float accelerationTime;
    [SerializeField] int rollDamage;
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
    [SerializeField] Transform rampDetectorL;
    [SerializeField] Transform rampDetectorR;
    [SerializeField] Transform groundDetectorL;
    [SerializeField] Transform groundDetectorR;
    [SerializeField] float groundDetectorRadius;
    [SerializeField] float wallDetectorRadius;

    private bool isGrounded;
    private bool isOnWall;
    private bool isOnLeftWall;
    private bool isOnRightWall;
    private bool rampLeft;
    private bool rampRight;
    private bool ramp;
    private int wallSide;
    private bool isSliding;
    private bool wallJumping;
    private bool jumping;
    private bool rising;
    private bool isRunning;
    private bool isClimbing;
    private bool preparingRun;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<Player_Health>();
		tickSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        // Don't try to merge the two ifs below. It doesn't make sense, but it doensn't work.
        if (rBody.velocity.y < 0f)
            animator.SetTrigger("MidAir");
        else
            animator.ResetTrigger("MidAir");

        if (rBody.velocity.y <= 0f)
            rising = false;

        if (rBody.velocity.x > 0.001f)
        {
            sRenderer.flipX = false;
            animator.SetBool("Moving", true);
        }
        else if (rBody.velocity.x < -0.001f)
        {
            sRenderer.flipX = true;
            animator.SetBool("Moving", true);
        }
        else
            animator.SetBool("Moving", false);


        // Walk sideways
        float horizontalInput = Input.GetAxis("Horizontal");
        float horizontalInputRaw = Input.GetAxisRaw("Horizontal");
        RunningCheck((int)horizontalInputRaw);

        float movementSpeed;
        isClimbing = false;
        if (isRunning)
            movementSpeed = Mathf.Lerp(Mathf.Abs(rBody.velocity.x), rollSpeed, acceleration);
        else 
            movementSpeed = speed;

        if (!wallJumping)
        {
            int runningDirection = rBody.velocity.x > 0 ? 1 : rBody.velocity.x < 0 ? -1 : 0;
            ramp = runDirection == 1 ? rampRight : runDirection == -1 ? rampLeft : false;
            if (isOnWall && isGrounded && !jumping && ramp)
            {
                isClimbing = true;
                int yDirection = wallSide == horizontalInputRaw ? 1 : -1;

                if (runningDirection != horizontalInputRaw)
                    rBody.velocity = Vector2.Lerp(rBody.velocity,  new Vector2(horizontalInput, yDirection) * speed, acceleration);
                else
                    rBody.velocity = new Vector2(horizontalInput, yDirection) * movementSpeed;
            }
            else if (runningDirection != horizontalInputRaw)
            {
                movementSpeed = Mathf.Lerp(rBody.velocity.x, horizontalInput * speed, acceleration);
                rBody.velocity = new Vector2(movementSpeed, rBody.velocity.y);
            }
            else
                rBody.velocity = new Vector2(horizontalInput * movementSpeed, rBody.velocity.y);
        }
        else 
            rBody.velocity = Vector2.Lerp(rBody.velocity, new Vector2(horizontalInput * movementSpeed, rBody.velocity.y), Time.deltaTime);

        if (isOnWall && !isClimbing)
        {
            isRunning = false;
            animator.SetBool("Running", false);
            preparingRun = false;
        }

        // Wall and Ground detenction
        isGrounded = Physics2D.OverlapCircle(groundDetectorL.position, groundDetectorRadius)
                  || Physics2D.OverlapCircle(groundDetectorR.position, groundDetectorRadius);
        animator.SetBool("Grounded", isGrounded);

        isOnLeftWall =  Physics2D.OverlapCircle(wallDetectorL.position, wallDetectorRadius);
        isOnRightWall = Physics2D.OverlapCircle(wallDetectorR.position, wallDetectorRadius);
        rampLeft = !Physics2D.OverlapCircle(rampDetectorL.position, wallDetectorRadius);
        rampRight = !Physics2D.OverlapCircle(rampDetectorR.position, wallDetectorRadius);

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
            animator.SetBool("Running", false);
            isRunning = false;

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
                StartCoroutine(SetJumpFlag());
                rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);
                if (!isRunning){
                    animator.SetTrigger("Jump");
					tickSource.Play();
				}

                if (rBody.velocity.y > 0f)
                    rising = true;
            }
            else if (isOnWall && isSliding)
            {
                Vector2 jumpDirection = new Vector2(wallJumpDirection.x * -wallSide, wallJumpDirection.y);
                rBody.velocity = jumpDirection.normalized * wallJumpForce;
                StartCoroutine(SetWallJumpFlag());
                animator.ResetTrigger("MidAir");
                
                tickSource.Play();

                if (!isRunning)
                    animator.SetTrigger("Jump");
                animator.SetBool("Sliding", false);

                if (rBody.velocity.y > 0f)
                    rising = true;
            }

        }

        // Open/Close eyes
        if (Input.GetKeyDown(KeyCode.X) || !isRunning)
            EyeCameraController.instance.OpenEyes();
        else if (Input.GetKeyUp(KeyCode.X))
        {
            if (isRunning)
                EyeCameraController.instance.CloseEyes();
        }

        // Roll damage
        if (isRunning && isGrounded && !EyeCameraController.instance.isClosed)
            health.TakeDamage(rollDamage);

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
                    EyeCameraController.instance.CloseEyes();
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
    
    private IEnumerator SetJumpFlag()
    {
        jumping = true;
        yield return new WaitForSeconds(wallJumpTime);
        jumping = false;
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

        Gizmos.DrawWireSphere(wallDetectorL.position, wallDetectorRadius);
        Gizmos.DrawWireSphere(wallDetectorR.position, wallDetectorRadius);
        Gizmos.DrawWireSphere(rampDetectorL.position, wallDetectorRadius);
        Gizmos.DrawWireSphere(rampDetectorR.position, wallDetectorRadius);
        Gizmos.DrawWireSphere(groundDetectorL.position, groundDetectorRadius);
        Gizmos.DrawWireSphere(groundDetectorR.position, groundDetectorRadius);
    }
}