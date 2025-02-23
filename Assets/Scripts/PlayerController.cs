using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("MOVEMET SETTINGS")]
    [SerializeField, Range(0f, 100f)] private float _maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float _maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float _maxAirAcceleration = 20f;

    private Vector2 _direction, _desiredVelocity, _velocity;
    private float _maxSpeedChange, _acceleration;


    [Header("JUMP SETTINGS")]
    [SerializeField, Range(0f, 20f)] private float jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
    [SerializeField, Range(0f, 10f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 10f)] private float upwardMovementMultiplier = 1.7f;

    [Space]

    [SerializeField, Range(0, 1)] private float jumpCutHeight = 0.5f;

    [SerializeField] private float groundRememberTime = 0.3f;
    [SerializeField] private float jumpPressedRememberTime = 0.2f;

    float _groundRememberTime;
    float _jumpPressedRememberTime;

    private int _jumpPhase;
    private float defaultGravityScale, jumpSpeed;

    private bool jumpPressed;

    [Header("GROUND DETECTION")]
    [SerializeField] private float groundDetectionRadius = 0.3f;
    [SerializeField] private Vector3 groundDetectionOffset = new Vector3(0, -1f, 0);
    [SerializeField] private LayerMask groundIgnoreLayerMask;

    [Header("WALL CLIMBING")]
    [SerializeField] private float wallDetectionRadius = 0.7f;
    [SerializeField] private float wallHangGravity = 0.2f;
    [SerializeField] private float wallHangTime = 1f;
    [SerializeField] private Vector2 wallJumpForce; //X is the force on sideways, and Y is the force on upwards direction.
    [SerializeField] private LayerMask wallLayer;

    bool climbingWall;
    float _wallHangTime;

    WallGrabDirection wallGrabDirection;

    private enum WallGrabDirection { None, Left, Right }


    //Components
    private Rigidbody2D rb;

    //Debugging
    Vector2 playerGroundedPoint;
    Vector2 playerLandingPoint;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        defaultGravityScale = 1f;
    }

    void Update()
    {
        if (!climbingWall)
        {
            HandleMovementInput();
            HandleJumpInput();
        }

        HandleWallClimbInput();



        //Debugging
        if (IsGrounded())
        {
            playerGroundedPoint = transform.position + groundDetectionOffset;
        }
        else
        {
            playerLandingPoint = transform.position + groundDetectionOffset;
        }
    }

    private void FixedUpdate()
    {
        if (!climbingWall)
        {
            MovePlayer();
            HandleJumpPhysics();
        }

        ClimbingBehaviour();
    }

    private void HandleMovementInput()
    {
        _direction.x = Input.GetAxisRaw("Horizontal");
        _desiredVelocity = new Vector3(_direction.x, 0f) * _maxSpeed;

        if (_direction.x != 0)
            transform.localScale = new Vector2(_direction.x > 0 ? 1 : -1, transform.localScale.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position + (Vector3)groundDetectionOffset, groundDetectionRadius, groundIgnoreLayerMask);
    }


    private void HandleJumpInput()
    {
        jumpPressed = Input.GetButtonDown("Jump");

        if (jumpPressed)
        {
            _jumpPressedRememberTime = jumpPressedRememberTime;

        }

        if (IsGrounded())
        {
            _groundRememberTime = groundRememberTime;
        }

        if (Input.GetButtonUp("Jump"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * jumpCutHeight);
        }
    }

    private void HandleWallClimbInput()
    {
        jumpPressed |= Input.GetButtonDown("Jump");
    }


    private void MovePlayer()
    {
        _velocity = rb.linearVelocity;

        _acceleration = IsGrounded() ? _maxAcceleration : _maxAirAcceleration;
        _maxSpeedChange = _acceleration * Time.deltaTime;
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

        rb.linearVelocity = _velocity;
    }

    private void HandleJumpPhysics()
    {
        if (IsGrounded())
        {
            _jumpPhase = 0;
        }
        else
        {
            _groundRememberTime -= Time.deltaTime;
            _jumpPressedRememberTime -= Time.deltaTime;
        }

        if (jumpPressed && _groundRememberTime > 0 || _jumpPressedRememberTime > 0)
        {
            JumpAction();
        }


        if (rb.linearVelocity.y > 0)
        {
            rb.gravityScale = upwardMovementMultiplier;
        }
        else if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = downwardMovementMultiplier;
        }
        else if (rb.linearVelocity.y == 0)
        {
            rb.gravityScale = defaultGravityScale;
        }

        rb.linearVelocity = _velocity;
    }

    private void JumpAction()
    {
        if (IsGrounded() || _jumpPhase < maxAirJumps)
        {
            _jumpPhase += 1;

            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);

            if (_velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - _velocity.y, 0f);
            }
            else if (_velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(rb.linearVelocity.y);
            }
            _velocity.y += jumpSpeed;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climbable Wall"))
        {
            // Wall grab conditions:
            if (!IsGrounded() && wallGrabDirection != WallGrabDirection.None && transform.position.y > playerGroundedPoint.y)
            {
                GrabWall();
            }
        }

        if (collision.gameObject.tag == "Spike") //Reset Player Position if Collided to a spike
        {
            GameManager.instance.TeleportPlayerTo(GameManager.instance.startingPoint.position);
            CameraManager.instance.ResetToDefaultCamera();
        }
    }


    private void GrabWall()
    {
        climbingWall = true;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = wallHangGravity;
        _wallHangTime = wallHangTime;
    }

    private void ClimbingBehaviour()
    {
        // Detect walls on both sides
        RaycastHit2D leftWallDetector = Physics2D.Raycast(transform.position, -transform.right, wallDetectionRadius, wallLayer);
        RaycastHit2D rightWallDetector = Physics2D.Raycast(transform.position, transform.right, wallDetectionRadius, wallLayer);

        // Determine which wall the player is touching
        if (leftWallDetector.collider != null)
            wallGrabDirection = WallGrabDirection.Left;
        else if (rightWallDetector.collider != null)
            wallGrabDirection = WallGrabDirection.Right;
        else
            wallGrabDirection = WallGrabDirection.None;

        

        if (!climbingWall) return;

        // Decrease wall hang time
        _wallHangTime -= Time.deltaTime;
        if (_wallHangTime <= 0)
        {
            rb.gravityScale = defaultGravityScale;
            climbingWall = false;


            return;
        }

        // Handle wall jumping
        if (jumpPressed && wallGrabDirection != WallGrabDirection.None)
        {
            Vector2 jumpDirection = wallGrabDirection == WallGrabDirection.Left ? Vector2.right : Vector2.left;
            rb.AddForce(jumpDirection * wallJumpForce.x, ForceMode2D.Impulse);
            rb.AddForce(Vector2.up * wallJumpForce.y, ForceMode2D.Impulse);

            rb.gravityScale = upwardMovementMultiplier;

            _wallHangTime = wallHangTime;

            jumpPressed = false;

            //climbingWall = false;
        }

    }


    private void OnDrawGizmos()
    {
        //Ground Detection Gizmos
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + groundDetectionOffset, groundDetectionRadius);

        //Wall Side Detection        
        Gizmos.color = wallGrabDirection == WallGrabDirection.Left ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (-transform.right * wallDetectionRadius));

        Gizmos.color = wallGrabDirection == WallGrabDirection.Right ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (transform.right * wallDetectionRadius));


        if (playerGroundedPoint != Vector2.zero && !IsGrounded()) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(playerGroundedPoint, playerGroundedPoint + (Vector2.up * 2f));
            Gizmos.DrawWireSphere(playerGroundedPoint, groundDetectionRadius);
        }

        if (playerLandingPoint != Vector2.zero && IsGrounded()) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(playerLandingPoint, playerLandingPoint + (Vector2.up * 2f));
            Gizmos.DrawWireSphere(playerLandingPoint, groundDetectionRadius);
        }


    }
}
