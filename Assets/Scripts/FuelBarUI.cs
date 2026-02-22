using UnityEngine;
using UnityEngine.UI;

public class FuelBarUI : MonoBehaviour
{
    private Slider fuelSlider;
    private PlayerMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMovement = Object.FindFirstObjectByType<PlayerMovement>();
        fuelSlider = GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        fuelSlider.maxValue = playerMovement.maxFuel;
        fuelSlider.value = playerMovement.currentFuel;
    }
}
