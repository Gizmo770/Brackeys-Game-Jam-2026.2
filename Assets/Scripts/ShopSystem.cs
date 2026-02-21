using UnityEngine;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    private GameManager gameManager;

    public TextMeshProUGUI moneyText;

    [Header("UI Last Run Text Elements")]
    public TextMeshProUGUI cashEarnedText;
    public TextMeshProUGUI verticalDistanceText;
    public TextMeshProUGUI obstaclesDestroyedText;

    private void Start()
    {
        // Updates Shop UI for last run stats.
        gameManager = Object.FindAnyObjectByType<GameManager>();
        UpdateMoneyText();
        cashEarnedText.text 
            = "Cash Earned: $" + gameManager.distanceToNextBiome.ToString();
        verticalDistanceText.text 
            = "Vertical Travelled: " + gameManager.verticalDistanceTraveled.ToString();
        obstaclesDestroyedText.text
            = "Obstacles Destroyed: " + gameManager.obstaclesDestroyed.ToString();
    }

    public void UpdateMoneyText()
    {
        moneyText.text = "$" + gameManager.money.ToString();
    }
}