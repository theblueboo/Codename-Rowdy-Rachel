using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
    float acceleration = 1.5f;
    private const float baseAcceleration = 1.5f;

    private bool isDashing = false;

    private TextMeshProUGUI speedStats;

    IEnumerator AccelerationIncrease()
    {
        if (horizontal != 0 && acceleration < maxSpeed)
        {
            acceleration++;
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(AccelerationIncrease());
        }
        
        if (horizontal == 0)
        { acceleration = baseAcceleration;
          rb.velocity = new Vector2(0f, rb.velocity.y);
          isDashing = false;
        }
    }

    void Awake()
    {
        speedStats = GameObject.Find("SpeedStats").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
        speed = horizontal * acceleration;

        if (speed >= maxSpeed)
        {
            speed = maxSpeed;
        }



        if (!isFacingRight && horizontal > 0f )
        {
            Flip();
        }
        else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }

        rb.velocity = new Vector2(speed, rb.velocity.y);
        if (speedStats != null)
        {
            speedStats.text = "Current Acceleration: " + acceleration + "\nCurrent Speed: " + speed + "\nCurrent MaxSpeed: " + maxSpeed + "\nHorizontal: " + horizontal;
        }
        
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
        acceleration = baseAcceleration;
        isDashing = false;
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    public void Move(InputAction.CallbackContext context)
    {
        //Define horizontal
        // Activates the increase in momentum 
        if (isDashing == false)
        {
            StartCoroutine(AccelerationIncrease());
            isDashing = true;
        }

        horizontal = context.ReadValue<Vector2>().x;
        
    }

    public void Dash(InputAction.CallbackContext context)
    {

        maxSpeed = 8f;
        if (context.canceled)
        {
            maxSpeed = 4f;
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {

    }
}
