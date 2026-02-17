using UnityEngine;
using TMPro;

public class UpgradeSystem : MonoBehaviour
{
    private GameManager gameManager;
    public ShopSystem shopSystem;

    public enum UpgradeType { Engine, Fin, Hull, Defense }
    public UpgradeType upgradeType;

    // TODO: Add description and cost display if necessary.
    [Header("UI")]
    public TextMeshProUGUI upgradeNameText;
    public TextMeshProUGUI purchaseEquipText;

    private int currentIndex = 0;

    void Start()
    {
        gameManager = GameManager.Instance;

        if (shopSystem == null)
        {
            shopSystem = FindAnyObjectByType<ShopSystem>();
        }

        RefreshUI();
    }

    public void PrevUpgrade()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            RefreshUI();
        }
    }

    public void NextUpgrade()
    {
        if (currentIndex < GetNumUpgradesAvailable() - 1)
        {
            currentIndex++;
            RefreshUI();
        }
    }

    public void PurchaseEquip()
    {
        // Equip if owned.
        if(gameManager.OwnsUpgrade(upgradeType, currentIndex))
        {
            gameManager.EquipUpgrade(upgradeType, currentIndex);
            RefreshUI();
            return;
        }

        // Purchase if not owned.
        int upgradeCost = GetCurrentCost();
        if(gameManager.money >= upgradeCost)
        {
            gameManager.money -= upgradeCost;
            gameManager.AddOwned(upgradeType, currentIndex);
            gameManager.EquipUpgrade(upgradeType, currentIndex);
            RefreshUI();
        }
    }

    private int GetCurrentCost()
    {
        switch (upgradeType)
        {
            case UpgradeType.Engine: return shopSystem.engineUpgrades[currentIndex].cost;
            case UpgradeType.Fin: return shopSystem.finUpgrades[currentIndex].cost;
            case UpgradeType.Hull: return shopSystem.hullUpgrades[currentIndex].cost;
            case UpgradeType.Defense: return shopSystem.defenseUpgrades[currentIndex].cost;
            default: return int.MaxValue;
        }
    }

    private int GetNumUpgradesAvailable()
    {
        switch (upgradeType)
        {
            case UpgradeType.Engine:
                return shopSystem.engineUpgrades.Length;

            case UpgradeType.Fin:
                return shopSystem.finUpgrades.Length;

            case UpgradeType.Hull:
                return shopSystem.hullUpgrades.Length;

            case UpgradeType.Defense:
                return shopSystem.defenseUpgrades.Length;

            default:
                return 0;
        }
    }

    private void RefreshUI()
    {
        switch (upgradeType)
        {
            case UpgradeType.Engine:
                upgradeNameText.text = shopSystem.engineUpgrades[currentIndex].name;
                break;

            case UpgradeType.Fin:
                upgradeNameText.text = shopSystem.finUpgrades[currentIndex].name;
                break;

            case UpgradeType.Hull:
                upgradeNameText.text = shopSystem.hullUpgrades[currentIndex].name;
                break;

            case UpgradeType.Defense:
                upgradeNameText.text = shopSystem.defenseUpgrades[currentIndex].name;
                break;
        }


        if (gameManager.IsUpgradeEquipped(upgradeType, currentIndex))
        {
            purchaseEquipText.text = "EQUIPPED";
        }
        else if (gameManager.OwnsUpgrade(upgradeType, currentIndex))
        {
            purchaseEquipText.text = "Equip";
        }
        else
        {
            purchaseEquipText.text = $"Purchase ({GetCurrentCost()})";
        }
    }
}
