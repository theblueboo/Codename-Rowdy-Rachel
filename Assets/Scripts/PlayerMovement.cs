using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float horizontal;
    private float speed;
    private float maxSpeed = 4f;
    private float jumpingPower = 8f;
    private bool isFacingRight = true;
    private float dashSpeed = 2.0f;
    float accelleration = 4.0f;

    IEnumerator SpeedIncrease()
    {
        if (horizontal != 0 && accelleration > maxSpeed)
        {
            accelleration = accelleration + 1;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SpeedIncrease());
        }
        else if (horizontal == 0)
        { accelleration = 1.5f;
          rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        //speed = horizontal * accelleration;

        //if (speed >= maxSpeed)
        //{
        //    speed = maxSpeed;
        //}



        if (!isFacingRight && horizontal > 0f )
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }

        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if ( context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }


    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position,0.2f, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        //Define horizontal

        horizontal = context.ReadValue<Vector2>().x;

    }

    public void Dash(InputAction.CallbackContext context)
    {
        // Activates the increase in momentum 
        if (context.performed)
        {
            maxSpeed= maxSpeed * dashSpeed;
        }
        else if (context.canceled)
        {
            maxSpeed = maxSpeed / dashSpeed;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {

    }
}
