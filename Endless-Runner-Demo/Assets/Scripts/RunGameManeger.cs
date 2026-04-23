
using Unity.VisualScripting;
using UnityEngine;

public class RunGameManeger : MonoBehaviour
{
    [SerializeField] private Player _Player;
    [SerializeField] private GameObject _PlayerObject;
    [SerializeField] private GameObject[] _forestObstecl;
    [SerializeField] private GameObject[] _forestObsteclCurse;
    [SerializeField] private GameObject[] _desertObstecl;
    [SerializeField] private GameObject[] _desertObsteclCurse;
    [SerializeField] private float _Xspon;
    [SerializeField] private float _Yspon;
    [SerializeField] private int _obstacleCurseCount;

    private GameObject _LastObject;
    private int _obstacleCounter;
    private Vector2 _spawnPoint;

    public SCREEN_ENUM _curentScreen = SCREEN_ENUM.FOREST;
    GameObject[] _curentObstecl;
    GameObject[] _curentObsteclCours;

    public bool _obstaclePause;


    public enum SCREEN_ENUM
    {
        FOREST,
        DESERT
    }

    void Start()
    {
        _obstaclePause = false;
        switch (_curentScreen)
        {
               
            case SCREEN_ENUM.FOREST:
                _curentObstecl = _forestObstecl;
                _curentObsteclCours = _forestObsteclCurse;
            break;

            case SCREEN_ENUM.DESERT:
            _curentObstecl = _desertObstecl;
            _curentObsteclCours = _desertObsteclCurse;
            break;
        }
        _spawnPoint.y = _Yspon;
        _spawnPoint.x = _Xspon;
        GenerateOb();

    }

    
    void Update()
    {
        TimeKiper();
    }

    private void FixedUpdate()
    {
        if (SpawnCheck())
        {          
            GenerateOb();
        }
    }

    private void GenerateOb()
    {
        int rund;
        GameObject Ob;
        if (_obstacleCounter == _obstacleCurseCount)
        {
            rund = Random.Range(0, _curentObsteclCours.Length);
            Ob = Instantiate(_curentObsteclCours[rund]);
            
            _obstacleCounter =0;
        }
        else
        {

            rund = Random.Range(0, _curentObstecl.Length);
            Ob = Instantiate(_curentObstecl[rund]);
            _obstacleCounter ++;
        }
        Ob.transform.position = new Vector2(_spawnPoint.x, _spawnPoint.y);
        _LastObject = Ob;
        

    }

    private void TimeKiper()
    {
        
    }
    private bool SpawnCheck()
    {
        if (_obstaclePause) return false;
        if (_LastObject != null)
        {
            float distans = _LastObject.transform.position.x;
            float genDistans = _LastObject.GetComponent<Obstecl>()._GenerateDistance;
            return (_spawnPoint.x - distans >= genDistans);
        }
        return true;

    }
}
