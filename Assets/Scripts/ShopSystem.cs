using UnityEngine;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    private GameManager gameManager;

    //Used to hold list of available upgrades for the shop.
    public EngineUpgrade[] engineUpgrades;
    public FinUpgrade[] finUpgrades;
    public HullUpgrade[] hullUpgrades;
    public DefenseUpgrade[] defenseUpgrades;

    [Header("UI Last Run Text Elements")]
    public TextMeshProUGUI distanceToNextText;
    public TextMeshProUGUI verticalDistanceText;
    public TextMeshProUGUI obstaclesDestroyedText;

    private void Start()
    {
        // Updates Shop UI for last run stats.
        gameManager = Object.FindAnyObjectByType<GameManager>();
        distanceToNextText.text 
            = "Distance to Next Area: " + gameManager.distanceToNextBiome.ToString();
        verticalDistanceText.text 
            = "Vertical Travelled: " + gameManager.verticalDistanceTraveled.ToString();
        obstaclesDestroyedText.text
            = "Obstacles Destroyed: " + gameManager.obstaclesDestroyed.ToString();
    }
}

// For reference...
// Engine = Max Fuel/Top Speed
// Fins = Max Turn Speed
// Hull = Health/Speed Loss from Obstacles
// Defense System = Other Countermeasures
[System.Serializable]
public class EngineUpgrade
{
    public string name;
    public string description;
    public float maxFuel;
    public float topSpeed;
    public int cost;
    public bool isBase;
}

[System.Serializable]
public class FinUpgrade
{
    public string name;
    public string description;
    public float maxTurnSpeed;
    public int cost;
    public bool isBase;
}

[System.Serializable]
public class HullUpgrade
{
    public string name;
    public string description;
    public float health;
    public float speedLossMultiplier;
    public int cost;
    public bool isBase;
}

[System.Serializable]
public class DefenseUpgrade
{
    public string name;
    public string description;
    public GameObject countermeasurePrefab;
    public int cost;
    public bool isBase;
}