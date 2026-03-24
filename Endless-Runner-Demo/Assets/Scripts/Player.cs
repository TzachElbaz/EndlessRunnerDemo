using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Jumping")]
    [SerializeField] private float gravity;
    [SerializeField] private float jumpVelocity = 20;
    [SerializeField] private float maxHoldJumpTime = 0.4f;
    [SerializeField] private float jumpGroundThreshold = 1;
    private float holdJumpTimer = 0.0f;
    private bool isHoldongJump = false;
    private float groundHeight = 10;

    [Header("Running")]
    [SerializeField] private float acceleration = 10;
    [SerializeField] private float maxXVelocity = 100;
    private float maxAcceleration = 10;

    public Vector2 velocity;
    private bool isGrounded = true;
    public float distance = 0;


    // For Inputs
    void Update()
    {
        Vector2 position = transform.position;
        float groundDistance = Mathf.Abs(position.y - groundHeight);

        if (Input.GetKeyDown(KeyCode.Space) && (isGrounded || groundDistance <= jumpGroundThreshold))
            JumpSettings();
        if (Input.GetKeyUp(KeyCode.Space))
            isHoldongJump = false;

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
            if (position.y <= groundHeight) // onLanding
            {
                position.y = groundHeight;
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
}
