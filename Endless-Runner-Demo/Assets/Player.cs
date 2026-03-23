using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float gravity;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private float jumpVelocity = 20;
    [SerializeField] private float groundHeight = 10;
    [SerializeField] private float maxHoldJumpTime = 0.4f;

    private float holdJumpTimer = 0.0f;
    private bool isGrounded = true;
    private bool isHoldongJump = false;




    void Start()
    {

    }

    // For Inputs
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
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
            if (position.y <= groundHeight)
            {
                position.y = groundHeight;
                isGrounded = true;
                holdJumpTimer = 0.0f;
            }
        }

        transform.position = position;
    }

    private void JumpSettings()
    {
        isGrounded = false;
        velocity.y = jumpVelocity;
        isHoldongJump = true;
    }
}
