using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI distanceText;
    Player player;

    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
    }

    private void FixedUpdate()
    {
        distanceText.text = Mathf.FloorToInt(player.distance) + " m";
    }

}
