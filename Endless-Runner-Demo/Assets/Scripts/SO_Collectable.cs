using UnityEngine;

[CreateAssetMenu(fileName = "SO_Collectable", menuName = "Scriptable Objects/SO_Collectable")]
public class SO_Collectable : ScriptableObject
{
    [SerializeField, Range(1, 4)] private int _colNum;
    [SerializeField] public SCREEN_COL _zone;
    public enum SCREEN_COL
    {
        FOREST,
        DESERT
    }
}
