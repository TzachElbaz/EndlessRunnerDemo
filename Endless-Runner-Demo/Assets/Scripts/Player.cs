using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [Header("Jumping")]
    [SerializeField] public float gravity;
    [SerializeField] public float jumpForce = 20f;
    [SerializeField] public float maxHoldJumpTime = 0.4f;

    private float holdJumpTimer = 0f;
    private bool isHoldingJump = false;

    [Header("Running")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxXVelocity = 100f;
    private float maxAcceleration = 10f;

    [Header("Collision")]
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private Color _groundCheckColor;

    private bool isGrounded;

    public Vector2 velocity;
    public float distance = 0f;

    private Vector2 startingPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        startingPosition = transform.position;
        gravity = _rigidbody.gravityScale;
    }

    private void Update()
    {
        isGrounded = GroundCheck();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            JumpStart();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
            // Cut upward momentum if still going up
            if (_rigidbody.linearVelocity.y > 0f)
            {
                _rigidbody.linearVelocity = new Vector2(
                    _rigidbody.linearVelocity.x,
                    _rigidbody.linearVelocity.y * 0.5f
                );
            }
        }

        if (transform.position.y < -3.45f)
        {
            ReturnToStart();
        }
    }

    private void FixedUpdate()
    {
        // Running (affects distance only)
        float velocityRatio = velocity.x / maxXVelocity;
        float currentAcceleration = maxAcceleration * (1 - velocityRatio);

        velocity.x += currentAcceleration * Time.fixedDeltaTime;
        velocity.x = Mathf.Min(velocity.x, maxXVelocity);

        distance += velocity.x * Time.fixedDeltaTime;

        // Jump and gravity
        if (!isGrounded)
        {
            if (isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;

                if (holdJumpTimer < maxHoldJumpTime)
                {
                    _rigidbody.AddForce(Vector2.up * jumpForce * 0.5f * Time.fixedDeltaTime, ForceMode2D.Force);
                }
                else
                {
                    isHoldingJump = false;
                }
            }

            _rigidbody.linearVelocity += Vector2.up * gravity * Time.fixedDeltaTime;
        }
    }

    private void JumpStart()
    {
        isHoldingJump = true;
        holdJumpTimer = 0f;

        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocity.x, 0f);
        _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }


    private bool GroundCheck()
    {
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _groundCheckDistance);

        RaycastHit2D hit = Physics2D.BoxCast(origin, _groundCheckSize, 0f, Vector2.down, _groundCheckDistance, _groundLayers);
        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _groundCheckColor;
        Vector2 origin = new Vector2(transform.position.x, transform.position.y - _groundCheckDistance);
        Gizmos.DrawWireCube(origin, _groundCheckSize);
    }


    //private bool GroundCheck()
    //{
    //    Vector2 origin = new Vector2(transform.position.x + _landingRayStartingPoint, transform.position.y);
    //    return Physics2D.Raycast(origin, Vector2.down, _groundCheckDistance, _groundLayers);
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = _groundCheckColor;
    //    Vector2 origin = new Vector2(transform.position.x + _landingRayStartingPoint, transform.position.y);
    //    Gizmos.DrawLine(origin, origin + Vector2.down * _groundCheckDistance);
    //}

    private void ReturnToStart()
    {
        Vector2 pos = transform.position;
        pos.y = startingPosition.y + 20f;
        transform.position = pos;

        _rigidbody.linearVelocity = Vector2.zero;
    }
}