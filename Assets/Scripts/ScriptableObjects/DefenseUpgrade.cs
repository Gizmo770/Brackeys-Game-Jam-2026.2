using UnityEngine;

[CreateAssetMenu(fileName = "DefenseUpgrade", menuName = "ShipParts/Defense", order = 1)]
public class DefenseUpgrade : ScriptableObject
{
    public string partName;
    public string description;
    public GameObject countermeasurePrefab;
    public int cost;
    public Sprite sprite;
}