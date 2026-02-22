using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UpgradeSystem : MonoBehaviour
{
    private GameManager gameManager;
    public ShopSystem shopSystem;

    public enum UpgradeType { Engine, Fin, Hull, Defense }
    public UpgradeType upgradeType;

    [Header("UI")]
    public GameObject lastRunStats;
    public TextMeshProUGUI upgradeNameAndDescriptionText;
    public TextMeshProUGUI purchaseEquipText;

    private int currentIndex = 0;

    void Start()
    {
        gameManager = GameManager.Instance;

        if (shopSystem == null)
        {
            shopSystem = FindAnyObjectByType<ShopSystem>();
        }

        upgradeNameAndDescriptionText.enabled = false;
        lastRunStats.GameObject().SetActive(true);
        currentIndex = SetStartingIndex();
        RefreshUI();
    }

    public void PrevUpgrade()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            // Equip if owned.
            if (gameManager.OwnsUpgrade(upgradeType, currentIndex))
            {
                gameManager.EquipUpgrade(upgradeType, currentIndex);
            }
            RefreshUI();
            SfxManager.Instance.PlayButtonDown();
        }
    }

    public void NextUpgrade()
    {
        if (currentIndex < GetNumUpgradesAvailable() - 1 && gameManager.OwnsUpgrade(upgradeType, currentIndex))
        {
            currentIndex++;
            // Equip if owned.
            if (gameManager.OwnsUpgrade(upgradeType, currentIndex))
            {
                gameManager.EquipUpgrade(upgradeType, currentIndex);
            }
            RefreshUI();
            SfxManager.Instance.PlayButtonDown();
        }
    }

    public void PurchaseEquip()
    {
        // Purchase if not owned.
        int upgradeCost = GetCurrentCost();
        if(gameManager.money >= upgradeCost)
        {
            if(!gameManager.OwnsUpgrade(upgradeType, currentIndex))
            {
                gameManager.money -= upgradeCost;
            }
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

    private int SetStartingIndex()
    {
        for(int i = 0; i < 4; i++)
        {
            switch (upgradeType)
            {
                case UpgradeType.Engine:
                    if (GameManager.Instance.items.engineUpgrades[i] == PlayerMovement.ShipStats.engineUpgrade)
                    {
                        return i;
                    }
                    break;
                case UpgradeType.Fin:
                    if (GameManager.Instance.items.finUpgrades[i] == PlayerMovement.ShipStats.finUpgrade)
                    {
                        return i;
                    }
                    break;
                case UpgradeType.Hull:
                    if (GameManager.Instance.items.hullUpgrades[i] == PlayerMovement.ShipStats.hullUpgrade)
                    {
                        return i;
                    }
                    break;
                case UpgradeType.Defense:
                    if (GameManager.Instance.items.defenseUpgrades[i] == PlayerMovement.ShipStats.defenseUpgrade)
                    {
                        return i;
                    }
                    break;
                default:
                    return 0;
            }
        }
        return 0;
    }

    private void RefreshUI()
    {
        if (gameManager.IsUpgradeEquipped(upgradeType, currentIndex))
        {
            purchaseEquipText.text 
                = GameManager.Instance.GetUpgradeName(upgradeType, currentIndex);
        }
        else
        {
            purchaseEquipText.text = $"${GetCurrentCost()}";
        }
    }

    public void DisplayUpgradeInfoText()
    {
        upgradeNameAndDescriptionText.text = upgradeType switch
        {
            UpgradeType.Engine => $"Name: {GameManager.Instance.items.engineUpgrades[currentIndex].partName}\n{GameManager.Instance.items.engineUpgrades[currentIndex].description}",
            UpgradeType.Fin => $"Name: {GameManager.Instance.items.finUpgrades[currentIndex].partName}\n{GameManager.Instance.items.finUpgrades[currentIndex].description}",
            UpgradeType.Hull => $"Name: {GameManager.Instance.items.hullUpgrades[currentIndex].partName}\n{GameManager.Instance.items.hullUpgrades[currentIndex].description}",
            UpgradeType.Defense => $"Name: {GameManager.Instance.items.defenseUpgrades[currentIndex].partName}\n{GameManager.Instance.items.defenseUpgrades[currentIndex].description}",
            _ => ""
        };
        lastRunStats.GameObject().SetActive(false);
        upgradeNameAndDescriptionText.enabled = true;
    }

    public void HideUpgradeInfoText()
    {
        upgradeNameAndDescriptionText.enabled = false;
        lastRunStats.GameObject().SetActive(true);
    }
}
