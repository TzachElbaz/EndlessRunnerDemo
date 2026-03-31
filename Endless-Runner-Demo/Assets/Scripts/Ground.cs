using UnityEngine;

public class Ground : MonoBehaviour
{

    private float groundHeight;
    public float GroundHeight => groundHeight;

    private void Awake()
    {
        groundHeight = transform.position.y + transform.lossyScale.y/2;
    }
}
