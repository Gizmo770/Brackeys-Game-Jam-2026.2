using UnityEngine;

[CreateAssetMenu(fileName = "FinUpgrade", menuName = "ShipParts/Fin", order = 1)]
public class FinUpgrade : ScriptableObject
{
    public string partName;
    public string description;
    public float maxTurnSpeed;
    public float deceleration;
    public int cost;
    public Sprite sprite1;
    public Sprite sprite2;
}