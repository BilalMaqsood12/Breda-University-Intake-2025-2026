using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("MOVEMET SETTINGS")]
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

    [Header("Ground Detection")]
    [SerializeField] private float groundDetectionRadius = 0.3f;
    [SerializeField] private Vector3 groundDetectionOffset = new Vector3(0, -1f, 0);


    //Components
    private Rigidbody2D rb;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        defaultGravityScale = 1f;
    }

    void Update()
    {
        HandleMovementInput();
        HandleJumpInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        HandleJumpPhysics();
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
        Collider2D collider = Physics2D.OverlapCircle(transform.position + (Vector3)groundDetectionOffset, groundDetectionRadius);
        return collider != null && !collider.CompareTag("Player"); // Ignore player object
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

    private void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + groundDetectionOffset, groundDetectionRadius);
    }
}
