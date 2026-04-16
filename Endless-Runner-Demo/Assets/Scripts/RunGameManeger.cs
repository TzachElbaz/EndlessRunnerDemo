
using UnityEngine;

public class RunGameManeger : MonoBehaviour
{
    [SerializeField] Player _Player;
    [SerializeField] GameObject _PlayerObject;
    [SerializeField] GameObject[] _Obstecl;
    [SerializeField] GameObject[] _ObsteclCurse;
    [SerializeField] float _Xspon;
    [SerializeField] float _Yspon;

    GameObject _LastObject;
    Vector2 _spawnPoint;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {       
        _spawnPoint.y = _Yspon;
        _spawnPoint.x = _Xspon;
        GenerateOb();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float distans = _LastObject.transform.position.x;
        Debug.Log("holt");
        float genDistans = _LastObject.GetComponent<Obstecl>()._GenerateDistance;
        if (_spawnPoint.x - distans >= genDistans)
        {
            Debug.Log("geberate");
            GenerateOb();
        }
    }

    private void GenerateOb()
    {
        int rund = Random.Range(0, _Obstecl.Length);
        GameObject Ob = Instantiate(_Obstecl[rund]);
        Ob.transform.position = new Vector2(_spawnPoint.x, _spawnPoint.y);
        _LastObject = Ob;
        Debug.Log("bloop");

    }
}
