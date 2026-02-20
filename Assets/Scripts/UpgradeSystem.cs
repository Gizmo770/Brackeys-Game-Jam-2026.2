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
            shopSystem.UpdateMoneyText();
            RefreshUI();
        }
    }

    private int GetCurrentCost()
    {
        switch (upgradeType)
        {
            case UpgradeType.Engine: return GameManager.Instance.items.engineUpgrades[currentIndex].cost;
            case UpgradeType.Fin: return GameManager.Instance.items.finUpgrades[currentIndex].cost;
            case UpgradeType.Hull: return GameManager.Instance.items.hullUpgrades[currentIndex].cost;
            case UpgradeType.Defense: return GameManager.Instance.items.defenseUpgrades[currentIndex].cost;
            default: return int.MaxValue;
        }
    }

    private int GetNumUpgradesAvailable()
    {
        switch (upgradeType)
        {
            case UpgradeType.Engine:
                return GameManager.Instance.items.engineUpgrades.Length;

            case UpgradeType.Fin:
                return GameManager.Instance.items.finUpgrades.Length;

            case UpgradeType.Hull:
                return GameManager.Instance.items.hullUpgrades.Length;

            case UpgradeType.Defense:
                return GameManager.Instance.items.defenseUpgrades.Length;

            default:
                return 0;
        }
    }

    private void RefreshUI()
    {
        switch (upgradeType)
        {
            case UpgradeType.Engine:
                upgradeNameText.text = GameManager.Instance.items.engineUpgrades[currentIndex].name;
                break;

            case UpgradeType.Fin:
                upgradeNameText.text = GameManager.Instance.items.finUpgrades[currentIndex].name;
                break;

            case UpgradeType.Hull:
                upgradeNameText.text = GameManager.Instance.items.hullUpgrades[currentIndex].name;
                break;

            case UpgradeType.Defense:
                upgradeNameText.text = GameManager.Instance.items.defenseUpgrades[currentIndex].name;
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
