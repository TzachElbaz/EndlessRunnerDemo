using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Jumping")]
    [SerializeField] public float gravity;
    [SerializeField] public float jumpVelocity = 20;
    [SerializeField] public float maxHoldJumpTime = 0.4f;
    [SerializeField] private float jumpGroundThreshold = 1;
    private float holdJumpTimer = 0.0f;
    private bool isHoldongJump = false;
    private float groundHeight = 10;

    [Header("Running")]
    [SerializeField] private float acceleration = 10;
    [SerializeField] private float maxXVelocity = 100;
    private float maxAcceleration = 10;

    [SerializeField] float landingRayStartingPoint = 1.3f;
    [SerializeField] float fallingRayStartingPoint = 1.3f;

    public Vector2 velocity;
    private bool isGrounded = true;
    public float distance = 0;

    // Stores the player's initial position
    private Vector2 startingPosition;

    private void Start()
    {
        startingPosition = transform.position;
    }

    // For Inputs
    void Update()
    {
        Vector2 position = transform.position;
        float groundDistance = Mathf.Abs(position.y - groundHeight);

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || groundDistance <= jumpGroundThreshold))
            JumpSettings();
        if (Input.GetKeyUp(KeyCode.Space))
            isHoldongJump = false;
        if(transform.position.y < -3.45)
            ReturnToStart();

    }

    // For Movement
    private void FixedUpdate()
    {
        var position = transform.position;
        if (!isGrounded) //Jumping
        {
            if (isHoldongJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if (holdJumpTimer >= maxHoldJumpTime)
                    isHoldongJump = false;
            }

            position.y += velocity.y * Time.fixedDeltaTime;
            if (!isHoldongJump)
                velocity.y += gravity * Time.fixedDeltaTime;

            var ground = GroundCheck(position);
            if (ground != null) // landing
            {
                groundHeight = ground.GroundHeight;
                position.y = groundHeight;
                velocity.y = 0;
                isGrounded = true;
            }

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

            isGrounded = FallCheck(position);
        }

        distance += velocity.x * Time.fixedDeltaTime;

        transform.position = position;
    }

    private void JumpSettings()
    {
        isGrounded = false;
        velocity.y = jumpVelocity;
        isHoldongJump = true;
        holdJumpTimer = 0.0f;
    }

    private Ground GroundCheck(Vector2 position)
    {
        Vector2 rayOrigin = new Vector2(position.x + landingRayStartingPoint, position.y);
        Vector2 rayDirection = Vector2.up;
        float rayDistance = velocity.y * Time.fixedDeltaTime;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);

        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);

        if (hit.collider != null && hit.collider.gameObject.CompareTag("Ground"))
        {
            var ground = hit.collider.gameObject.GetComponent<Ground>();
            return ground;
        }
        return null;

    }
    private bool FallCheck(Vector2 position)
    {
        Vector2 rayOrigin = new Vector2(position.x - fallingRayStartingPoint, position.y);
        Vector2 rayDirection = Vector2.up;
        float rayDistance = velocity.y * Time.fixedDeltaTime;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance);

        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.yellow);

        return !(hit.collider == null); // if not colliding with anything, isGrounded = false
    }

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
