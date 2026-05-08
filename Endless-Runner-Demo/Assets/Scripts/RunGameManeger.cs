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
    [SerializeField] private int _pregenLength;
    [SerializeField] private float _minLength;
    [SerializeField] private float _addLength;
    private GameObject _LastObject;
    private int _obstacleCounter;
    private Vector2 _spawnPoint;


    
    public SCREEN_ENUM _curentScreen = SCREEN_ENUM.FOREST;
    [Header("alt generation")]
    GameObject[] _curentObstecl;
    GameObject[] _curentObsteclCours;
    private int[] pregen ;
    private float[] genLength;
    private int listCount;
    private bool _pregenEmpty;
    [SerializeField] private float _jumpChaineLength;
    [SerializeField] private float _dropChaineLength;
    [SerializeField] private int _obstecalChainChance;
    [SerializeField] private int _obsteclBrakeChance;
    

    public bool _obstaclePause;
    public bool _generateAlt;

    private void OnEnable()
    {
        // Subscribe
        Player.OnPlayerDied += ShowGameOver;
    }

    private void OnDisable()
    {
        // Unsubscribe
        Player.OnPlayerDied -= ShowGameOver;
    }
    private void ShowGameOver()
    {
        Debug.Log("GAME OVER!");
    }

    public enum SCREEN_ENUM
    {
        FOREST,
        DESERT
    }

    void Start()
    {
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
        pregen = new int[_pregenLength];
        genLength = new float[_pregenLength];
        listCount = 0;
        _pregenEmpty = true;
        GenerateOb();

    }

    
    void Update()
    {
        TimeKiper();
    }

    private void FixedUpdate()
    {
        if (!_generateAlt && SpawnCheck())
        {
            GenerateOb();
        }
        else if (_generateAlt && SpawnCheckAlt()) 
        {
            SpawnOB();         
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

    private void GenerateObAlt()
    {
        //int prevLast =1;
        int rund;
        int repetCount = 0;      
        float length =_minLength;
        Obstacle now;
        Obstacle prev= _curentObstecl[pregen[pregen.Length-1]].GetComponent<Obstacle>();
        GameObject Ob;
        for (int i = 0; i < pregen.Length; i++)
        {
            rund = Random.Range(0, _curentObstecl.Length);
            if (i > 0 && rund == pregen[i - 1])
            {
                if (repetCount < 1) repetCount++;
                else
                {
                    repetCount = 0;
                    while (rund == pregen[i - 1])
                    {
                        rund = Random.Range(0, _curentObstecl.Length);
                    }
                }

            }
            pregen[i] = rund;
            int randomObstacleEvent = Random.Range(0, 10);
            Ob = _curentObstecl[rund];
            now = Ob.GetComponent<Obstacle>();
            if (i != 0)
            {
                prev = _curentObstecl[pregen[i - 1]].GetComponent<Obstacle>();
            }


            if (randomObstacleEvent <= _obstecalChainChance)
            {
                length = TwoOBDistantCheck(prev, now);
            }
            else if (randomObstacleEvent <= _obstecalChainChance+_obsteclBrakeChance)
            {
                length = _minLength * Random.Range(2, 5);
            }
            else
            {
                length = now._GenerateDistance;

            }
                genLength[i] = length;
        }
        _pregenEmpty = false;


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
            float genDistans = _LastObject.GetComponent<Obstacle>()._GenerateDistance;
            return (_spawnPoint.x - distans >= genDistans);
        }
        return true;

    }
    private bool SpawnCheckAlt()
    {
        if (_obstaclePause) return false;
        if (_pregenEmpty)
        {
            GenerateObAlt();
        }

        if (_LastObject != null)
        {
            float distans = _LastObject.transform.position.x;
            float genDistans = genLength[listCount];
            return (_spawnPoint.x - distans >= genDistans);
        }

        return true;

    }

    private void SpawnOB()
    {
        GameObject Ob;
        Ob = Instantiate(_curentObstecl[pregen[listCount]]);
        Ob.transform.position = new Vector2(_spawnPoint.x, _spawnPoint.y);
        _LastObject = Ob;
        listCount++;
        if(pregen.Length <= listCount)
        {
            listCount = 0;
            _pregenEmpty = true;
        }
    }
    private float TwoOBDistantCheck(Obstacle OBa, Obstacle OBb)
    {
        float length = _minLength;
        switch (OBa._passPoint)
        {
            case Obstacle.PASS_POINT.UP:
                switch (OBb._passPoint)
                {
                    case Obstacle.PASS_POINT.UP:
                        length = _minLength + _addLength;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE:
                        length = _minLength;
                        break;

                    case Obstacle.PASS_POINT.DOWN:
                        length = _dropChaineLength;
                        break;

                    case Obstacle.PASS_POINT.UP_MIDDLE:
                        length = _minLength;
                        break;

                    case Obstacle.PASS_POINT.UP_DOWN:
                        length = _minLength;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE_DOWN:
                        length = _addLength+_dropChaineLength;
                        break;


                }
                break;

            case Obstacle.PASS_POINT.MIDDLE:
                switch (OBb._passPoint)
                {
                    case Obstacle.PASS_POINT.UP:
                        length = _jumpChaineLength;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE:
                        length = _minLength;
                        break;

                    case Obstacle.PASS_POINT.DOWN:
                        length = _dropChaineLength;
                        break;

                    case Obstacle.PASS_POINT.UP_MIDDLE:
                        if (1 == Random.Range(0, 2)) length = _jumpChaineLength;
                        else length = _minLength;
                        break;

                    case Obstacle.PASS_POINT.UP_DOWN:
                        length = _jumpChaineLength;                        
                        break;

                    case Obstacle.PASS_POINT.MIDDLE_DOWN:
                        if (1 == Random.Range(0, 2)) length = _addLength;
                        else length = _dropChaineLength;
                        break;


                }
                break;

            case Obstacle.PASS_POINT.DOWN:
                switch (OBb._passPoint)
                {
                    case Obstacle.PASS_POINT.UP:
                        length = _minLength;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE:
                        length = _minLength ;
                        break;

                    case Obstacle.PASS_POINT.DOWN:
                        length = _addLength;
                        break;

                    case Obstacle.PASS_POINT.UP_MIDDLE:
                        if (1 == Random.Range(0, 2)) length = _minLength ;
                        else length = _minLength +_addLength;
                        break;

                    case Obstacle.PASS_POINT.UP_DOWN:
                        if (1 == Random.Range(0, 2)) length = _addLength;
                        else length = _minLength;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE_DOWN:
                        if (1 == Random.Range(0, 2)) length = _addLength;
                        else length = _minLength ;
                        break;


                }
                break;

            case Obstacle.PASS_POINT.UP_MIDDLE:
                switch (OBb._passPoint)
                {
                    case Obstacle.PASS_POINT.UP:
                        
                       length = _jumpChaineLength;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE:
                        length = _minLength;
                        break;

                    case Obstacle.PASS_POINT.DOWN:
                        length = _dropChaineLength;
                        break;

                    case Obstacle.PASS_POINT.UP_MIDDLE:
                        if (1 == Random.Range(0, 2)) length = _minLength;
                        else length = _jumpChaineLength;
                        break;

                    case Obstacle.PASS_POINT.UP_DOWN:
                        if (1 == Random.Range(0, 2)) length = _dropChaineLength;
                        else length = _jumpChaineLength;
                        
                        break;

                    case Obstacle.PASS_POINT.MIDDLE_DOWN:
                        length = _dropChaineLength+ _addLength;
                        break;


                }
                break;

            case Obstacle.PASS_POINT.UP_DOWN:
                switch (OBb._passPoint)
                {
                    case Obstacle.PASS_POINT.UP:
                        length = _minLength ;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE:
                        length = _minLength ;
                        break;

                    case Obstacle.PASS_POINT.DOWN:
                        length = _dropChaineLength;
                        break;

                    case Obstacle.PASS_POINT.UP_MIDDLE:
                        length = _minLength;
                        break;

                    case Obstacle.PASS_POINT.UP_DOWN:
                        length = _minLength ;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE_DOWN:
                        length = _dropChaineLength;
                        break;


                }
                break;

            case Obstacle.PASS_POINT.MIDDLE_DOWN:
                switch (OBb._passPoint)
                {
                    case Obstacle.PASS_POINT.UP:
                        if (1 == Random.Range(0, 2)) length = _jumpChaineLength;
                        else length = _minLength;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE:
                        length = _minLength;
                        break;

                    case Obstacle.PASS_POINT.DOWN:
                        length = _dropChaineLength;
                        break;

                    case Obstacle.PASS_POINT.UP_MIDDLE:
                        length = _jumpChaineLength;
                        break;

                    case Obstacle.PASS_POINT.UP_DOWN:
                        length = _jumpChaineLength;
                        break;

                    case Obstacle.PASS_POINT.MIDDLE_DOWN:
                        length = _dropChaineLength;
                        break;


                }
                break;
        }
        return length;
    }
}
