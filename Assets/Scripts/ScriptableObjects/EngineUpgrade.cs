using UnityEngine;

[CreateAssetMenu(fileName = "EngineUpgrade", menuName = "ShipParts/Engine", order = 1)]
public class EngineUpgrade : ScriptableObject
{
    public string partName;
    public string description;
    public float topSpeed;
    public float acceleration;
    public float maxFuel;
    public int cost;
    public Sprite sprite;
}