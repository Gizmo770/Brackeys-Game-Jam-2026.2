using UnityEngine;

[CreateAssetMenu(fileName = "HullUpgrade", menuName = "ShipParts/Hull", order = 1)]
public class HullUpgrade : ScriptableObject
{
    public string partName;
    public string description;
    public float speedLossMultiplier;
    public float maxFuel;
    public int cost;
    public Sprite sprite;
}