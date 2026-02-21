using UnityEngine;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    private GameManager gameManager;

    public TextMeshProUGUI moneyText;

    [Header("UI Last Run Text Elements")]
    public TextMeshProUGUI distanceToNextText;
    public TextMeshProUGUI verticalDistanceText;
    public TextMeshProUGUI obstaclesDestroyedText;

    private void Start()
    {
        // Updates Shop UI for last run stats.
        gameManager = Object.FindAnyObjectByType<GameManager>();
        UpdateMoneyText();
        distanceToNextText.text 
            = "Distance to Next Area: " + gameManager.distanceToNextBiome.ToString();
        verticalDistanceText.text 
            = "Vertical Travelled: " + gameManager.verticalDistanceTraveled.ToString();
        obstaclesDestroyedText.text
            = "Obstacles Destroyed: " + gameManager.obstaclesDestroyed.ToString();
    }

    public void UpdateMoneyText()
    {
        moneyText.text = "Credits Available: " + gameManager.money.ToString();
    }
}


[CreateAssetMenu(fileName = "Items", menuName = "Data/Items", order = 1)]
public class Items : ScriptableObject
{
    public EngineUpgrade[] engineUpgrades;
    public FinUpgrade[] finUpgrades;
    public HullUpgrade[] hullUpgrades;
    public DefenseUpgrade[] defenseUpgrades;
}

// For reference...
// Engine = Max Fuel/Top Speed
// Fins = Max Turn Speed
// Hull = Health/Speed Loss from Obstacles
// Defense System = Other Countermeasures
[CreateAssetMenu(fileName = "EngineUpgrade", menuName = "ShipParts/Engine", order = 1)]
public class EngineUpgrade : ScriptableObject
{
    public string partName;
    public string description;
    public float maxFuel;
    public float topSpeed;
    public int cost;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "FinUpgrade", menuName = "ShipParts/Fin", order = 1)]
public class FinUpgrade : ScriptableObject
{
    public string partName;
    public string description;
    public float maxTurnSpeed;
    public int cost;
    public Sprite sprite1;
    public Sprite sprite2;
}

[CreateAssetMenu(fileName = "HullUpgrade", menuName = "ShipParts/Hull", order = 1)]
public class HullUpgrade : ScriptableObject
{
    public string partName;
    public string description;
    public float health;
    public float speedLossMultiplier;
    public int cost;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "DefenseUpgrade", menuName = "ShipParts/Defense", order = 1)]
public class DefenseUpgrade : ScriptableObject
{
    public string partName;
    public string description;
    public GameObject countermeasurePrefab;
    public int cost;
    public Sprite sprite;
}