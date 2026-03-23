using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Jumping")]
    [SerializeField] private float gravity;
    [SerializeField] private float jumpVelocity = 20;
    [SerializeField] private float maxHoldJumpTime = 0.4f;
    [SerializeField] private float jumpGroundThreshold = 1;

    private Vector2 velocity;
    private float groundHeight = 10;
    private float holdJumpTimer = 0.0f;
    private bool isGrounded = true;
    private bool isHoldongJump = false;




    void Start()
    {

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

    }

    // For Movement
    private void FixedUpdate()
    {
        var position = transform.position;
        if (!isGrounded)
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
