using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float gravity;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private float jumpVelocity = 20;
    [SerializeField] private float groundHeight = 10;
    
    private bool isGrounded = true;



    void Start()
    {

    }

    // For Inputs
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            Jump();

    }

    // For Movement
    private void FixedUpdate()
    {
        var position = transform.position;
        if (!isGrounded) 
        {
            position.y += velocity.y * Time.fixedDeltaTime;
            velocity.y += gravity * Time.fixedDeltaTime;

            if(position.y <= groundHeight)
            {
                position.y = groundHeight;
                isGrounded = true;
            }
        }

        transform.position = position;
    }

    private void Jump()
    {
        isGrounded = false;
        velocity.y = jumpVelocity;
    }
}
