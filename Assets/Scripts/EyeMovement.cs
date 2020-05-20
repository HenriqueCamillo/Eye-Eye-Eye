using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeMovement : MonoBehaviour
{
    private Vector2 inputDirection;
    private Rigidbody2D rBody;
    [SerializeField] float speed;

    [SerializeField] float jumpForce;
    
    [Header("Jump Gravity Modifiers")]
    [SerializeField] float fallMultiplier;
    [SerializeField] float lowJumpMultiplier;



    void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        
        // Jump
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            rBody.velocity = new Vector2(rBody.velocity.x, jumpForce);
        }

        // Walk sideways
        rBody.velocity = new Vector2(inputDirection.x * speed, rBody.velocity.y);
    }

    private void ApplyJumpGravityModifiers()
    {
        bool holdingJumpButton = Input.GetAxisRaw("Vertical") == 1f ? true : false;
        if (rBody.velocity.y < 0f)
        {
            rBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }
        else if (rBody.velocity.y > 0f && !holdingJumpButton)
        {
            rBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
        }
    }

}