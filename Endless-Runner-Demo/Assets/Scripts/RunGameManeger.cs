
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
    [SerializeField] private int _pregenLength;
    [SerializeField] private float _minLength;
    [SerializeField] private float _addLength;
    private GameObject _LastObject;
    private int _obstacleCounter;
    private Vector2 _spawnPoint;


    

    public SCREEN_ENUM _curentScreen = SCREEN_ENUM.FOREST;
    GameObject[] _curentObstecl;
    GameObject[] _curentObsteclCours;

    public bool _obstaclePause;
    public bool _generateAlt;

  

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
            GenerateObAlt();
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
        int[] pregen= new int[_pregenLength];
        float[] genLength = new float [_pregenLength];
        float length =_minLength;
        GameObject Ob;
        for (int i = 0; i < pregen.Length; i++) 
        {
            rund = Random.Range(0, _curentObstecl.Length);
            if (rund == pregen[i - 1]) 
            {
                if(repetCount < 1) repetCount++;
                else
                {
                    repetCount = 0;
                    while(rund== pregen[i - 1])
                    {
                        rund = Random.Range(0, _curentObstecl.Length);
                    }
                }

            }
            pregen[i] = rund;
            int chain = Random.Range(0, 3);
            Ob = _curentObstecl[rund];
            Obstecl now = Ob.GetComponent<Obstecl>();
            Obstecl prev = _curentObstecl[pregen[i - 1]].GetComponent<Obstecl>();
            if (chain == 0)
            {
                switch (prev._passPoint)
                {
                    case Obstecl.PASS_POINT.UP:
                        switch (now._passPoint)
                        {
                            case Obstecl.PASS_POINT.UP:
                                length = _minLength + 5;
                                break;

                            case Obstecl.PASS_POINT.MIDDLE:
                                length = _minLength;
                                break;

                            case Obstecl.PASS_POINT.DOWN:
                                length = 7;
                                break;

                            case Obstecl.PASS_POINT.UP_MIDDLE:
                                length = _minLength;
                                break;

                            case Obstecl.PASS_POINT.UP_DOWN:
                                length = _minLength;
                                break;

                            case Obstecl.PASS_POINT.MIDDLE_DOWN:
                                length = 7;
                                break;


                        }
                        break;

                    case Obstecl.PASS_POINT.MIDDLE:
                        switch (now._passPoint)
                        {
                            case Obstecl.PASS_POINT.UP:
                                length = 15;
                                break;

                            case Obstecl.PASS_POINT.MIDDLE:
                                length = _minLength;
                                break;

                            case Obstecl.PASS_POINT.DOWN:
                                length = _minLength;
                                break;

                            case Obstecl.PASS_POINT.UP_MIDDLE:
                                if (1 == Random.Range(0, 1)) length = _minLength;
                                else length = 15;
                                break;

                            case Obstecl.PASS_POINT.UP_DOWN:
                                if (1 == Random.Range(0, 1)) length = _minLength;
                                else length = 15;
                                break;

                            case Obstecl.PASS_POINT.MIDDLE_DOWN:
                                if (1 == Random.Range(0, 1)) length = _minLength;
                                else length = 5;
                                break;


                        }
                        break;

                    case Obstecl.PASS_POINT.DOWN:
                        switch (now._passPoint)
                        {
                            case Obstecl.PASS_POINT.UP:
                                length = _minLength / 2;
                                break;

                            case Obstecl.PASS_POINT.MIDDLE:
                                length = _minLength / 3;
                                break;

                            case Obstecl.PASS_POINT.DOWN:
                                length = 5;
                                break;

                            case Obstecl.PASS_POINT.UP_MIDDLE:
                                if (1 == Random.Range(0, 1)) length = _minLength / 2;
                                else length = _minLength / 3;
                                break;

                            case Obstecl.PASS_POINT.UP_DOWN:
                                if (1 == Random.Range(0, 1)) length = 10;
                                else length = 5;
                                break;

                            case Obstecl.PASS_POINT.MIDDLE_DOWN:
                                if (1 == Random.Range(0, 1)) length = 5;
                                else length = _minLength / 3; ;
                                break;


                        }
                        break;

                    case Obstecl.PASS_POINT.UP_MIDDLE:
                        switch (now._passPoint)
                        {
                            case Obstecl.PASS_POINT.UP:
                                if (1 == Random.Range(0, 1)) length = _minLength;
                                else length = 5;
                                break;

                            case Obstecl.PASS_POINT.MIDDLE:
                                length = 15;
                                break;

                            case Obstecl.PASS_POINT.DOWN:
                                length = 7;
                                break;

                            case Obstecl.PASS_POINT.UP_MIDDLE:
                                if (1 == Random.Range(0, 1)) length = 7;
                                else length = 15;
                                break;

                            case Obstecl.PASS_POINT.UP_DOWN:
                                length = 7;
                                break;

                            case Obstecl.PASS_POINT.MIDDLE_DOWN:
                                length = _minLength;
                                break;


                        }
                        break;

                    case Obstecl.PASS_POINT.UP_DOWN:
                        switch (now._passPoint)
                        {
                            case Obstecl.PASS_POINT.UP:
                                length = _minLength / 2;
                                break;

                            case Obstecl.PASS_POINT.MIDDLE:
                                length = _minLength / 3;
                                break;

                            case Obstecl.PASS_POINT.DOWN:
                                length = 7;
                                break;

                            case Obstecl.PASS_POINT.UP_MIDDLE:
                                length = _minLength;
                                break;

                            case Obstecl.PASS_POINT.UP_DOWN:
                                length = _minLength / 2;
                                break;

                            case Obstecl.PASS_POINT.MIDDLE_DOWN:
                                length = _minLength / 3;
                                break;


                        }
                        break;

                    case Obstecl.PASS_POINT.MIDDLE_DOWN:
                        switch (now._passPoint)
                        {
                            case Obstecl.PASS_POINT.UP:
                                if (1 == Random.Range(0, 1)) length = 7;
                                else length = 15;
                                break;

                            case Obstecl.PASS_POINT.MIDDLE:
                                length = _minLength;
                                break;

                            case Obstecl.PASS_POINT.DOWN:
                                length = 7;
                                break;

                            case Obstecl.PASS_POINT.UP_MIDDLE:
                                length = 7;
                                break;

                            case Obstecl.PASS_POINT.UP_DOWN:
                                length = 7;
                                break;

                            case Obstecl.PASS_POINT.MIDDLE_DOWN:
                                length = _minLength / 3;
                                break;


                        }
                        break;


                }
            }
            else 
            {
                length = _minLength + _addLength;
            }
            genLength[i] = length;

            




        }


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
    private bool SpawnCheckAlt()
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
