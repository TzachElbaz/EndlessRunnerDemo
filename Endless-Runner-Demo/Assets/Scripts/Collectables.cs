using UnityEngine;

public class Collectables : MonoBehaviour
{
    CollectablesManager collectables;
    [SerializeField,Range(1,4)] private int _colNum;
    
    [SerializeField] SCREEN_COL _zone;
    [SerializeField] SO_Collectable _so;
    private int _ZoneNum;
    private enum SCREEN_COL
    {
        FOREST,
        DESERT
    }

    private void Awake()
    {
        collectables = GameObject.FindAnyObjectByType<CollectablesManager>();
        //_zone = _so._zone;

        
    }
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (_zone)
            {
                case SCREEN_COL.FOREST:
                    _ZoneNum = 0;
                    break;
                case SCREEN_COL.DESERT:
                    _ZoneNum = 1;
                    break;
            }
            collectables._colectableNum[_ZoneNum, _colNum-1] = true;
            Destroy(gameObject);
        }
    }

    void Update()
    {
        
    }
}
