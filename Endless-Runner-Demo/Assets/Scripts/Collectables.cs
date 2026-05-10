using UnityEngine;

public class Collectables : MonoBehaviour
{
    CollectablesManager collectables;
    [SerializeField, HideInInspector] private int _colectableId;   
    [SerializeField] SO_Collectable _so;
    [SerializeField, HideInInspector] public SO_Collectable.SCREEN_COL _zone;
    [SerializeField, HideInInspector] private Sprite _sprite;



    private void Awake()
    {
        collectables = GameObject.FindAnyObjectByType<CollectablesManager>();
       _colectableId= _so._colectableId;
        _zone = _so._zone;
        _sprite = _so._sprite;
        GetComponentInChildren<SpriteRenderer>().sprite= _sprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        collectables.SetCollectable(_colectableId);
        Destroy(gameObject);
    }
}
