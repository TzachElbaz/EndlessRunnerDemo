using UnityEngine;

[CreateAssetMenu(fileName = "SO_Obstecl", menuName = "Scriptable Objects/SO_Obstecl")]
public class SO_Obstecl : ScriptableObject
{
    [SerializeField] public bool _IsObsteclCourse;
    [SerializeField] public float _GenerateDistance;
}
