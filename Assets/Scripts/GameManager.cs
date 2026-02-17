using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int money = 400;

    // These are tracked by index to the shop system's upgrade arrays.
    [Header("Owned Upgrades")]
    public List<int> ownedEngineUpgrades = new();
    public List<int> ownedFinUpgrades = new();
    public List<int> ownedHullUpgrades = new();
    public List<int> ownedDefenseUpgrades = new();

    // Same, by index.
    [Header("Equipped Upgrades")]
    public int equippedEngineIndex = 0;
    public int equippedFinIndex = 0;
    public int equippedHullIndex = 0;
    public int equippedDefenseIndex = 0;

    [Header("Applied Player Stats")]
    public float appliedHealth;
    public float appliedMaxFuel;
    public float appliedTopSpeed;
    public float appliedMaxTurnSpeed;
    public float appliedSpeedLossMultiplier;
    public GameObject appliedDefensePrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        EnsureBaseOwned();
    }

    private void EnsureBaseOwned()
    {
        if (!ownedEngineUpgrades.Contains(0)) ownedEngineUpgrades.Add(0);
        if (!ownedFinUpgrades.Contains(0)) ownedFinUpgrades.Add(0);
        if (!ownedHullUpgrades.Contains(0)) ownedHullUpgrades.Add(0);
        if (!ownedDefenseUpgrades.Contains(0)) ownedDefenseUpgrades.Add(0);

        equippedEngineIndex = Mathf.Max(equippedEngineIndex, 0);
        equippedFinIndex = Mathf.Max(equippedFinIndex, 0);
        equippedHullIndex = Mathf.Max(equippedHullIndex, 0);
        equippedDefenseIndex = Mathf.Max(equippedDefenseIndex, 0);
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
            UpgradeSystem.UpgradeType.Engine => equippedEngineIndex == index,
            UpgradeSystem.UpgradeType.Fin => equippedFinIndex == index,
            UpgradeSystem.UpgradeType.Hull => equippedHullIndex == index,
            UpgradeSystem.UpgradeType.Defense => equippedDefenseIndex == index,
            _ => false
        };
    }

    public void EquipUpgrade(UpgradeSystem.UpgradeType type, int index)
    {
        switch (type)
        {
            case UpgradeSystem.UpgradeType.Engine: equippedEngineIndex = index; break;
            case UpgradeSystem.UpgradeType.Fin: equippedFinIndex = index; break;
            case UpgradeSystem.UpgradeType.Hull: equippedHullIndex = index; break;
            case UpgradeSystem.UpgradeType.Defense: equippedDefenseIndex = index; break;
        }
        RecalculateAppliedStats();
    }

    private void RecalculateAppliedStats()
    {
        ShopSystem shop = Object.FindAnyObjectByType<ShopSystem>();

        EngineUpgrade engine = shop.engineUpgrades[equippedEngineIndex];
        appliedMaxFuel = engine.maxFuel;
        appliedTopSpeed = engine.topSpeed;

        FinUpgrade fins = shop.finUpgrades[equippedFinIndex];
        appliedMaxTurnSpeed = fins.maxTurnSpeed;

        HullUpgrade hull = shop.hullUpgrades[equippedHullIndex];
        appliedHealth = hull.health;
        appliedSpeedLossMultiplier = hull.speedLossMultiplier;

        DefenseUpgrade defense = shop.defenseUpgrades[equippedDefenseIndex];
        appliedDefensePrefab = defense.countermeasurePrefab;
    }
}
