using UnityEngine;

[CreateAssetMenu(fileName = "HullUpgrade", menuName = "ShipParts/Hull", order = 1)]
public class HullUpgrade : ScriptableObject
{
    public string partName;
    public string description;
    public float speedLossMultiplier;
    public int cost;
    public Sprite sprite;
}