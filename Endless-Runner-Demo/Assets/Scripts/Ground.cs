using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;

    private float groundHeight;
    private float groundRight;
    private float screenRight;
    private bool didGenerateGround = false;

    public float GroundHeight => groundHeight;

    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();

        groundHeight = transform.position.y + transform.lossyScale.y / 2;
        screenRight = Camera.main.transform.position.x * 2;
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.deltaTime;

        groundRight = transform.position.x + transform.lossyScale.x / 2;

        if (groundRight < 0)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true;
                GenerateGround();
            }
        }

        transform.position = pos;
    }

    private void GenerateGround()
    {
        GameObject go = Instantiate(gameObject);
        Vector2 pos;

        float h1 = player.jumpVelocity * player.maxHoldJumpTime;
        float t = player.jumpVelocity / player.gravity;
        float h2 = player.jumpVelocity * t + (0.5f * (player.gravity * (t * t)));
        float maxJumpHeight = h1 + h2;
        float maxY = player.transform.position.y + maxJumpHeight;
        maxY *= 0.7f;
        float minY = 3;
        float actualY = Random.Range(minY, maxY) - go.transform.lossyScale.y / 2;


        pos.y = actualY;
        pos.x = screenRight + 30;
        go.transform.position = pos;

        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = go.transform.position.y + go.transform.lossyScale.y / 2;
    }
}
