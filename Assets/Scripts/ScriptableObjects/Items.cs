using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Data/Items", order = 1)]
public class Items : ScriptableObject
{
    public EngineUpgrade[] engineUpgrades;
    public FinUpgrade[] finUpgrades;
    public HullUpgrade[] hullUpgrades;
    public DefenseUpgrade[] defenseUpgrades;
}