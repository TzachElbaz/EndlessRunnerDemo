using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] private int _coinValue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    CollectablesManager collectables;
    private void Awake()
    {
        collectables = GameObject.FindAnyObjectByType<CollectablesManager>();
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collectables._coinCount += _coinValue;
            Destroy(gameObject);
        }
    }
}
