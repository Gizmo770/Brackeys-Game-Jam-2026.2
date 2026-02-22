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
        SfxManager.Instance.FadeInShopTheme();

        // Updates Shop UI for last run stats.
        gameManager = GameManager.Instance;
        UpdateMoneyText();
        cashEarnedText.text 
            = "Cash Earned: $" + gameManager.moneyGainedLastRun.ToString();
        verticalDistanceText.text 
            = "Vertical Travelled: " + Mathf.FloorToInt(gameManager.verticalDistanceTraveled) + "m";
        obstaclesDestroyedText.text
            = "Obstacles Destroyed: " + gameManager.obstaclesDestroyed.ToString();
    }

    public void UpdateMoneyText()
    {
        moneyText.text = "$" + gameManager.money.ToString();
    }
}