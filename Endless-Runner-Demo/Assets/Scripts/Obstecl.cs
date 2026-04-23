using UnityEngine;

public class Obstecl : MonoBehaviour
{
    [SerializeField] public bool _IsObsteclCourse;
    [SerializeField] public float _GenerateDistance;
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

        if (position.x < -30) Destroy(gameObject);

        transform.position = position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector2 endPosition = new Vector2 (transform.position.x + _GenerateDistance, transform.position.y);
        Gizmos.DrawLine(transform.position, endPosition);
        Gizmos.DrawLine(endPosition, new Vector2(endPosition.x, endPosition.y + 5));
    }
}
