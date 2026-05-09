using UnityEngine;

public class CollectablesManager : MonoBehaviour
{
    public static CollectablesManager instance;
    public int _coinCount;
    public bool[] _colectableList;
    [SerializeField] private Animator[] _anmation;

    public void SetCollectable(int id)
    {
        _colectableList[id] = true;

        _anmation[id].SetBool("isVisible",true);
        if (_colectableList[0] && _colectableList[1] && _colectableList[2] && _colectableList[3])
        {
            CollectableReset();
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

}
