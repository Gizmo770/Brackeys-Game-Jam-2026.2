using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UpgradeSystem;

public class CashHover : MonoBehaviour
{
    public GameObject lastRunStats;
    public TextMeshProUGUI upgradeNameAndDescriptionText;

    public void DisplayUpgradeInfoText()
    {
        upgradeNameAndDescriptionText.text = "Current Cash: $" + GameManager.Instance.money + "\n"
            + "Earn more cash by travelling far!";
        lastRunStats.GameObject().SetActive(false);
        upgradeNameAndDescriptionText.enabled = true;
    }

    public void HideUpgradeInfoText()
    {
        upgradeNameAndDescriptionText.enabled = false;
        lastRunStats.GameObject().SetActive(true);
    }
}
