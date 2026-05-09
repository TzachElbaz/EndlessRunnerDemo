using UnityEngine;

public class Collectables : MonoBehaviour
{
    CollectablesManager collectables;
    [SerializeField] private int _colectableId;   
    [SerializeField] SO_Collectable _so;
    
   

    private void Awake()
    {
        collectables = GameObject.FindAnyObjectByType<CollectablesManager>();      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        collectables.SetCollectable(_colectableId);
        Destroy(gameObject);
    }
}
