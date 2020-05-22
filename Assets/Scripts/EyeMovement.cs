using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    private Rigidbody2D rBody;
    [SerializeField] float speed;
    [SerializeField] float slideSpeed;

    [SerializeField] float jumpForce;
    [SerializeField] float wallJumpForce;
    [SerializeField] float wallJumpTime;
    [SerializeField] float stopSlidingTime;
    [SerializeField] Vector2 wallJumpDirection;
    
    [Header("Jump Gravity Modifiers")]
    [SerializeField] float fallMultiplier;
    [SerializeField] float lowJumpMultiplier;

    [Header("Detectors")]
    [SerializeField] float groundDetectorOffset;
    [SerializeField] float wallDetectorOffset;
    [SerializeField] float detectorRadius;

    private bool isGrounded;
    private bool isOnWall;
    private bool isOnLeftWall;
    private bool isOnRightWall;
    private int wallSide;
    private bool isSliding;
    private bool wallJumping;
    private bool rising;

    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rBody.velocity.y <= 0f)
            rising = false;

        // Walk sideways
        float horizontalInput = Input.GetAxis("Horizontal");
        float horizontalInputRaw = Input.GetAxisRaw("Horizontal");
        if (!wallJumping)
            rBody.velocity = new Vector2(horizontalInput * speed, rBody.velocity.y);
        else 
            rBody.velocity = Vector2.Lerp(rBody.velocity, new Vector2(horizontalInput * speed, rBody.velocity.y), Time.deltaTime);


        // Wall and Ground detenction
        isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + Vector2.down * groundDetectorOffset, detectorRadius);

        isOnLeftWall =  Physics2D.OverlapCircle((Vector2)transform.position + Vector2.left * wallDetectorOffset, detectorRadius);
        isOnRightWall = Physics2D.OverlapCircle((Vector2)transform.position + Vector2.right * wallDetectorOffset, detectorRadius);

        isOnWall = isOnLeftWall || isOnRightWall;
        wallSide = !isOnWall ? 0 : isOnLeftWall ? -1 : 1;

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
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (isGrounded)
                rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);
            else if (isOnWall && isSliding)
            {
                Vector2 jumpDirection = new Vector2(wallJumpDirection.x * -wallSide, wallJumpDirection.y);
                rBody.velocity = jumpDirection.normalized * wallJumpForce;
                StartCoroutine(SetWallJumpFlag());
            }

            if (rBody.velocity.y > 0f)
                rising = true;
        }

        ApplyJumpGravityModifiers();
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

        Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.left * wallDetectorOffset, detectorRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.right * wallDetectorOffset, detectorRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + Vector2.down * groundDetectorOffset, detectorRadius);
    }
}