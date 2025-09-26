using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    public PlayerMovementStats RachelStats;
    [SerializeField] private Collider2D _feetColl;
    [SerializeField] private Collider2D _bodyColl;

    private Rigidbody2D _rb;

    //Movement variables
    private Vector2 _movementVelocity;
    private Vector2 _movementSpeed;
    private bool _isFacingRight;

    //Collision Check Variables
    private RaycastHit2D _groundhit;
    private RaycastHit2D _headhit;
    private bool _isGrounded;
    private bool _squishedHead;
    private bool _squishedBody;

    private void Awake()
    {
        _isFacingRight = true;

        _rb = GetComponent<Rigidbody2D>();
    }

    #region Movement

    private void Move(float acceleration, float deceleration, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);

            Vector2 targetVelocity = Vector2.zero;
            if(InputManager.RunIsHeld)
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * RachelStats.MaxRunSpeed;
            }
            else { targetVelocity = new Vector2(moveInput.x, 0f) * RachelStats.MaxWalkSpeed; }

            _movementVelocity = Vector2.Lerp(_movementVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_movementVelocity.x, _rb.velocity.y);
        }

        else if ( moveInput == Vector2.zero)
        {
            _movementVelocity = Vector2.Lerp(_movementVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            _rb.velocity = new Vector2(_movementVelocity.x, _rb.velocity.y);
        }
    }

    private void TurnCheck(Vector2 moveInput)
    {
        if (_isFacingRight && moveInput.x <0)
        {
            Turn(false);
        }

        else if (!_isFacingRight && moveInput.x > 0) 
        {
            Turn(true);
        }
    }

    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            _isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else
        {
            _isFacingRight = false;
            transform.Rotate(0f,-180f,0f);
        }
    }

    #endregion

    #region Collision

    private void IsGrounded()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.center.x, RachelStats.GroundDetectionRayLength); ;

        _groundhit = Physics2D.BoxCast(boxCastOrigin, boxCastSize,0f,Vector2.down,RachelStats.GroundDetectionRayLength,RachelStats.GroundLayer);
        if (_groundhit.collider != null )
        {
            _isGrounded = true;
        }
        else { _isGrounded = false; }

        #region Collision Debug Visuals

        //if (RachelStats.DebugShowIsGroundedBox)
        //{
        //    Color rayColor;
        //    if (_isGrounded)
        //    {
        //        rayColor = Color.green;
        //    }
        //    else { rayColor = Color.red;}

        //    Debug.DrawRay(new Vector2(boxCastOrigin.x - boxCastSize.x / 2, boxCastOrigin.y), Vector2.down * RachelStats.GroundDetectionRayLength, rayColor);


        #endregion
    }

    private void CollisionCheck()
    {

    }
    #endregion
}
