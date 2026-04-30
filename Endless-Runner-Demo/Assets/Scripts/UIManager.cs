using UnityEngine;
using TMPro;
public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI moneyText;
    Player player;
    CollectablesManager collectables;

    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        collectables = GameObject.FindAnyObjectByType<CollectablesManager>();
    }

    private void FixedUpdate()
    {
        distanceText.text = Mathf.FloorToInt(player.distance) + " m";
        moneyText.text = collectables._coinCount + " e$";
    }

}
