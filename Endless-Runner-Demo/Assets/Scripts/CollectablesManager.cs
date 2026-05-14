using UnityEngine;
using static RunGameManeger;

public class CollectablesManager : MonoBehaviour
{
    Player player;
    public static CollectablesManager instance;
    public int _coinCount;
    public bool[] _colectableList;
    [SerializeField] private Animator[] _anmation;
    [SerializeField] public SO_Collectable[] _soForestCollectableList;
    [SerializeField] public SO_Collectable[] _soDesertCollectableList;
    [SerializeField] private float _collectableSpawnDistant;
    

    public bool _isCollectableAvalable;

    private void OnEnable()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        RunGameManeger.OnChangeErea += OnchangeErea;
    }
    private void OnDisable()
    {
        RunGameManeger.OnChangeErea -= OnchangeErea;
    }
    private void FixedUpdate()
    {
        if(player._collectableSpawnClock >= _collectableSpawnDistant)
        {
            _isCollectableAvalable = true;
        }
    }
    public void SetCollectable(int id)
    {
        _colectableList[id] = true;
        

        _anmation[id].SetBool("isVisible",true);
        if (_colectableList[0] && _colectableList[1] && _colectableList[2] && _colectableList[3])
        {
            RunGameManeger.Instance.InvokeCangeErea();
            
        }
        
    }
    public void CollectableReset()
    {
        
        for (int i = 0; i < _colectableList.Length; i++)
        {
            _colectableList[i] = false;
            _anmation[i].SetBool("isVisible", false);
        }
        
    }
    private void OnchangeErea()
    {
        CollectableReset();
        for (int i = 0; i < _colectableList.Length; i++)
        {
            _anmation[i].SetInteger("erea", (((int)RunGameManeger.Instance._nextScreen)));
        }
    } 

}
