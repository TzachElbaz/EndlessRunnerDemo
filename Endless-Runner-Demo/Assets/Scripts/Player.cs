using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private BoxCollider2D _boxCollider;

    [Header("Jumping")]
    [SerializeField] public float gravity;
    [SerializeField] public float jumpForce = 20;
    [SerializeField] public float maxHoldJumpTime = 0.4f;
    [SerializeField] private float jumpGroundThreshold = 1;
    private float holdJumpTimer = 0.0f;
    private bool isHoldongJump = false;
    //private float groundHeight = 10;

    [Header("Running")]
    [SerializeField] private float acceleration = 10;
    [SerializeField] private float maxXVelocity = 100;
    private float maxAcceleration = 10;

    [Header("Collision")]
    [SerializeField] private float _groundCheckDistance;
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private Color _groundCheckColor;
    private bool isGrounded;

    [SerializeField] float landingRayStartingPoint = 1.3f;
    [SerializeField] float fallingRayStartingPoint = 1.3f;

    public Vector2 velocity;
    public float distance = 0;

    // Stores the player's initial position
    private Vector2 startingPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        startingPosition = transform.position;
        gravity = _rigidbody.gravityScale;
    }

    // For Inputs
    void Update()
    {
        Vector2 position = transform.position;
        //float groundDistance = Mathf.Abs(position.y - groundHeight);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded /*(isGrounded || groundDistance <= jumpGroundThreshold)*/)
            JumpSettings();
        if (Input.GetKeyUp(KeyCode.Space))
            isHoldongJump = false;
        if (transform.position.y < -3.45)
            ReturnToStart();
    }

    // For Movement
    private void FixedUpdate()
    {
        var position = transform.position;
        if (!isGrounded) //Jumping
        {
            Jump(position);

            //var ground = GroundCheck(position);
            //if (ground != null) // landing
            //{
            //    groundHeight = ground.GroundHeight;
            //    position.y = groundHeight;
            //    velocity.y = 0;
            //    isGrounded = true;
            //}

        }
        else // Running
        {
            float velocityRacio = velocity.x / maxXVelocity;
            acceleration = maxAcceleration * (1 - velocityRacio);

            velocity.x += acceleration * Time.fixedDeltaTime;
            if (velocity.x >= maxXVelocity)
            {
                velocity.x = maxXVelocity;
            }

            //isGrounded = FallCheck(position);
        }

        distance += velocity.x * Time.fixedDeltaTime;

        transform.position = position;
    }

    private void JumpSettings()
    {
        isGrounded = false;
        velocity.y = jumpForce;
        isHoldongJump = true;
        holdJumpTimer = 0.0f;
    }

    private void Jump(Vector3 position)
    {
        if (isHoldongJump)
        {
            holdJumpTimer += Time.fixedDeltaTime;
            if (holdJumpTimer >= maxHoldJumpTime)
                isHoldongJump = false;
            _rigidbody.linearVelocityY = 0;
            _rigidbody.AddForce(new Vector2(_rigidbody.linearVelocityX, jumpForce), ForceMode2D.Impulse);
        }

        position.y += velocity.y * Time.fixedDeltaTime;
        if (!isHoldongJump)
            velocity.y += gravity * Time.fixedDeltaTime;
    }



    private bool GroundCheck()
    {
        Vector2 origin = new Vector2(transform.position.x + landingRayStartingPoint, transform.position.y);
        return Physics2D.Raycast(origin, Vector2.down, _groundCheckDistance, _groundLayers);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _groundCheckColor;
        Vector2 origin = new Vector2(transform.position.x + landingRayStartingPoint, transform.position.y);
        Gizmos.DrawLine(origin,
            new Vector2(origin.x, origin.y - _groundCheckDistance));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && GroundCheck())
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")
            && _rigidbody.linearVelocityY < 0)
        {
            isGrounded = GroundCheck();
        }
    }

    //private bool FallCheck(Vector2 position)
    //{
    //    Vector2 rayOrigin = new Vector2(position.x - fallingRayStartingPoint, position.y);
    //    Vector2 rayDirection = Vector2.up;
    //    float rayDistance = velocity.y * Time.fixedDeltaTime;
    //    RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);

    //    Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);

    //    return !(hit.collider == null); // if not colliding with anything, isGrounded = false
    //}

    private void ReturnToStart()
    {
        var pos = transform.position;
        pos.y = startingPosition.y + 20f;
        transform.position = pos;
    }

    //private void LogToConsole()
    //{
    //    Debug.Log($"Player position y = {transform.position.y}");
    //    Debug.Log($"Ground height = {groundHeight}");
    //}
}
