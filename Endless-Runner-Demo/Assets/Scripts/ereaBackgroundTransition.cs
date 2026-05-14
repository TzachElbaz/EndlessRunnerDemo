using UnityEngine;

public class ereaBackgroundTransition : MonoBehaviour
{
    Player player;
    [SerializeField] private float depth = 1;
    private Vector2 _startLocation;
    private void Awake()
    {
        _startLocation = transform.position;
        player = GameObject.FindAnyObjectByType<Player>();
    }

    private void FixedUpdate()
    {
        float realVelocity = player.velocity.x / depth;
        Vector2 position = transform.position;

        position.x -= realVelocity * Time.fixedDeltaTime;

        if (position.x < -70)
        {
            transform.position = _startLocation;
            gameObject.SetActive(false);
        }
            

        transform.position = position;
    }
}
