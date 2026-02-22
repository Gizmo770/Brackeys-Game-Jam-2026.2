using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public static GameManager Instance { get; private set; }

    [Header("Money")]
    public int money = 400;
    public float moneyGainPerDistance = 0.5f;

    //Used to hold list of available upgrades for the shop.
    public Items items;

    // These are tracked by index to the shop system's upgrade arrays.
    [Header("Owned Upgrades")]
    public List<int> ownedEngineUpgrades = new();
    public List<int> ownedFinUpgrades = new();
    public List<int> ownedHullUpgrades = new();
    public List<int> ownedDefenseUpgrades = new();

    [Header("Last Run Tracker Stats")]
    public int moneyGainedLastRun;
    public float verticalDistanceTraveled;
    public int obstaclesDestroyed;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Update()
    {
        if (playerMovement == null)
        {
            playerMovement = Object.FindFirstObjectByType<PlayerMovement>();
        }

        // Player loss condition
        if(playerMovement != null)
        {
            if(!playerMovement.dead && playerMovement.currentFuel <= 0 && playerMovement.rb.linearVelocity.magnitude <= .05f)
            {
                playerMovement.dead = true;
                CalculateLastRunStats();
                GetMoney();
                FindFirstObjectByType<SceneTransition>().TriggerSceneChange("ShopScene");
            }
        }
    }

    private void GetMoney()
    {
        moneyGainedLastRun = Mathf.FloorToInt(
            verticalDistanceTraveled * moneyGainPerDistance);
        money += moneyGainedLastRun;
        money += 25;
    }

    public void EnsureBaseOwned()
    {
        if (!ownedEngineUpgrades.Contains(0)) {
            ownedEngineUpgrades.Add(0);
            PlayerMovement.ShipStats.engineUpgrade = items.engineUpgrades[0];
        }
        if (!ownedFinUpgrades.Contains(0)) 
        {
            ownedFinUpgrades.Add(0);
            PlayerMovement.ShipStats.finUpgrade = items.finUpgrades[0];
        }
        if (!ownedHullUpgrades.Contains(0))
        {
            ownedHullUpgrades.Add(0);
            PlayerMovement.ShipStats.hullUpgrade = items.hullUpgrades[0];
        }
        if (!ownedDefenseUpgrades.Contains(0))
        {
            ownedDefenseUpgrades.Add(0);
            PlayerMovement.ShipStats.defenseUpgrade = items.defenseUpgrades[0];
        }
    }


    // Shop/Upgrade System Methods
    public bool OwnsUpgrade(UpgradeSystem.UpgradeType type, int index)
    {
        return type switch
        {
            UpgradeSystem.UpgradeType.Engine => ownedEngineUpgrades.Contains(index),
            UpgradeSystem.UpgradeType.Fin => ownedFinUpgrades.Contains(index),
            UpgradeSystem.UpgradeType.Hull => ownedHullUpgrades.Contains(index),
            UpgradeSystem.UpgradeType.Defense => ownedDefenseUpgrades.Contains(index),
            _ => false
        };
    }

    public void AddOwned(UpgradeSystem.UpgradeType type, int index)
    {
        switch (type)
        {
            case UpgradeSystem.UpgradeType.Engine: ownedEngineUpgrades.Add(index); break;
            case UpgradeSystem.UpgradeType.Fin: ownedFinUpgrades.Add(index); break;
            case UpgradeSystem.UpgradeType.Hull: ownedHullUpgrades.Add(index); break;
            case UpgradeSystem.UpgradeType.Defense: ownedDefenseUpgrades.Add(index); break;
        }
    }

    public bool IsUpgradeEquipped(UpgradeSystem.UpgradeType type, int index)
    {
        return type switch
        {
            UpgradeSystem.UpgradeType.Engine => items.engineUpgrades[index] == PlayerMovement.ShipStats.engineUpgrade,
            UpgradeSystem.UpgradeType.Fin => items.finUpgrades[index] == PlayerMovement.ShipStats.finUpgrade,
            UpgradeSystem.UpgradeType.Hull => items.hullUpgrades[index] == PlayerMovement.ShipStats.hullUpgrade,
            UpgradeSystem.UpgradeType.Defense => items.defenseUpgrades[index] == PlayerMovement.ShipStats.defenseUpgrade,
            _ => false
        };
    }

    public string GetUpgradeName(UpgradeSystem.UpgradeType type, int index)
    {
        return type switch
        {
            UpgradeSystem.UpgradeType.Engine => items.engineUpgrades[index].partName,
            UpgradeSystem.UpgradeType.Fin => items.finUpgrades[index].partName,
            UpgradeSystem.UpgradeType.Hull => items.hullUpgrades[index].partName,
            UpgradeSystem.UpgradeType.Defense => items.defenseUpgrades[index].partName,
            _ => "Unknown Upgrade"
        };
    }

    public void EquipUpgrade(UpgradeSystem.UpgradeType type, int index)
    {
        switch (type)
        {
            case UpgradeSystem.UpgradeType.Engine:
                PlayerMovement.ShipStats.engineUpgrade = items.engineUpgrades[index];
                break;
            case UpgradeSystem.UpgradeType.Fin: 
                PlayerMovement.ShipStats.finUpgrade = items.finUpgrades[index];
                break;
            case UpgradeSystem.UpgradeType.Hull: 
                PlayerMovement.ShipStats.hullUpgrade = items.hullUpgrades[index]; 
                break;
            case UpgradeSystem.UpgradeType.Defense: 
                PlayerMovement.ShipStats.defenseUpgrade = items.defenseUpgrades[index]; 
                break;
        }
        playerMovement.RecalculateAppliedStats();
    }

    private void CalculateLastRunStats()
    {
        // Calculate the vertical distance from the launchpad to the player
        Transform launchPad = Object.FindFirstObjectByType<Launchpad>().transform;
        verticalDistanceTraveled = playerMovement.transform.position.y - launchPad.position.y;

        // TODO: Do calculations for obstacles destroyed.
    }
}
