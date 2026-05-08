using UnityEngine;
using TMPro;
using System;
public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI moneyText;
    [SerializeField] GameObject Heart1;
    [SerializeField] GameObject Heart2;
    [SerializeField] GameObject Heart3;
    Player player;
    CollectablesManager collectables;

    private void Awake()
    {
        player = GameObject.FindAnyObjectByType<Player>();
        collectables = GameObject.FindAnyObjectByType<CollectablesManager>();
    }

    private void OnEnable()
    {
        Player.OnPlayerHit += UpdateHearts;
    }

    private void OnDisable()
    {
        Player.OnPlayerHit -= UpdateHearts;

    }

    private void FixedUpdate()
    {
        distanceText.text = Mathf.FloorToInt(player.distance) + " m";
        moneyText.text = collectables._coinCount + " e$";
    }

    private void UpdateHearts(int health)
    {

        var heart1 = Heart1.GetComponentInChildren<Animator>();
        var heart2 = Heart2.GetComponentInChildren<Animator>();
        var heart3 = Heart3.GetComponentInChildren<Animator>();
        
        switch (health)
        {
            case 3:
                heart3.SetBool("isActive", true);
                heart2.SetBool("isActive", true);
                heart1.SetBool("isActive", true);
                break;
            case 2:
                heart3.SetBool("isActive", false);
                heart2.SetBool("isActive", true);
                heart1.SetBool("isActive", true);
                break;
            case 1:
                heart3.SetBool("isActive", false);
                heart2.SetBool("isActive", false);
                heart1.SetBool("isActive", true);
                break;
            case 0:
                heart3.SetBool("isActive", false);
                heart2.SetBool("isActive", false);
                heart1.SetBool("isActive", false);
                break;
            default:
                break;
        }
    }

}
