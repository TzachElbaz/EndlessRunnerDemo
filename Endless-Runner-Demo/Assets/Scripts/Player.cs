using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private Animator _animation;
    private BoxCollider2D _boxCollider;

    [Header("Jumping")]
    [SerializeField] public float jumpForce = 20f;
    public int jumpRemaining = 2;

    [Header("Running")]
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float maxXVelocity = 100f;
    private float maxAcceleration = 10f;

    [Header("Collision")]
    [SerializeField] private float _groundCheckDistance = 0.1f;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.5f, 0.1f);
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private Color _groundCheckColor;
    private Vector2 standingSize;
    private Vector2 rollingSize;
    private Vector2 standingOffset;
    private Vector2 rollingOffset;

    [Header("Rolling")]
    [SerializeField] private float rollDuration = .6f;
    private bool isRolling = false;
    private bool isGrounded;

    [HideInInspector]
    public Vector2 velocity;
    [HideInInspector]
    public float distance = 0f;

    private Vector2 startingPosition;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animation = GetComponentInChildren<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Start()
    {
        SetColliders();
        startingPosition = transform.position;
    }

    private void Update()
    {
        //isGrounded = GroundCheck();
        //if (isGrounded)
        //    jumpRemaining = 2;

        HandleInput();
        PlayerAnimation();

        if (transform.position.y < -3.45f)
        {
            ReturnToStart();
        }
    }

    private void FixedUpdate()
    {

        float velocityRatio = velocity.x / maxXVelocity;
        float currentAcceleration = maxAcceleration * (1 - velocityRatio);

        velocity.x += currentAcceleration * Time.fixedDeltaTime;
        velocity.x = Mathf.Min(velocity.x, maxXVelocity);

        distance += velocity.x * Time.fixedDeltaTime;
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanPlayerJump())
        {
            isGrounded = false;
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Roll();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            StopCoroutine(RollRutine());
            EndRolling();
        }
    }

    private void Jump()
    {
        jumpRemaining -= 1;
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

    private void PlayerAnimation()
    {
        _animation.SetBool("isRolling", isRolling);
    }

    private void Roll()
    {
        if (!isGrounded)
            return;

        _boxCollider.size = rollingSize;
        _boxCollider.offset = rollingOffset;
        StopAllCoroutines();
        StartCoroutine(RollRutine());
    }

    private IEnumerator RollRutine()
    {
        isRolling = true;
        yield return new WaitForSeconds(rollDuration);
        EndRolling();
    }

    private void EndRolling()
    {
        isRolling = false;
        _boxCollider.size = standingSize;
        _boxCollider.offset = standingOffset;

    }

    private void SetColliders()
    {
        standingSize = new Vector2(_boxCollider.size.x, _boxCollider.size.y);
        standingOffset = new Vector2(_boxCollider.offset.x, _boxCollider.offset.y);
        rollingSize = new Vector2(_boxCollider.size.x, _boxCollider.size.y / 2);
        rollingOffset = new Vector2(_boxCollider.offset.x, _boxCollider.offset.y / 2);
    }

    private bool CanPlayerJump()
    {
        if (isRolling)
            return false;

        return isGrounded || jumpRemaining > 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && GroundCheck())
        {
            isGrounded = true;
            jumpRemaining = 2;
        }
    }

    private void ReturnToStart()
    {
        Vector2 pos = transform.position;
        pos.y = startingPosition.y + 20f;
        transform.position = pos;

        _rigidbody.linearVelocity = Vector2.zero;
    }

}