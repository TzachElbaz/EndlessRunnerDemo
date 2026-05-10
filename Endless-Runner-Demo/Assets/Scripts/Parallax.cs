using UnityEngine;

public class Parallax : MonoBehaviour
{
    Player player;
    [SerializeField] private float depth = 1;

    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
    }

    private void FixedUpdate()
    {
        float realVelocity = player.velocity.x / depth;
        Vector2 position = transform.position;

        position.x -= realVelocity * Time.fixedDeltaTime;

        if(position.x < -30)
            position.x = 75;

        transform.position = position;
    }

}
