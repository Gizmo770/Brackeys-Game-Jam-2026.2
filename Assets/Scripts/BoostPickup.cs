using UnityEngine;

public class BoostPickup : MonoBehaviour
{
    public enum BoostType { Candy, Bubble, Fuel }
    public BoostType boostType;

    [Header("Candy Boost")]
    public float speedBoostAmount = 10f;
    public float candyBoostMultiplier = 1.5f;

    [Header("Bubble Boost")]
    public GameObject bubbleGameObject;

    [Header("Fuel Boost")]
    public float percentageFuelBoost = .15f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
            SfxManager.Instance.PlayPowerUp();

            switch (boostType)
            {
                case BoostType.Candy:
                    playerMovement.ApplySpeedBoost(speedBoostAmount, candyBoostMultiplier);
                    break;

                case BoostType.Bubble:
                    if(playerMovement.currentBubbleShield == null)
                    {
                        GameObject bubble
                            = Instantiate(bubbleGameObject, playerMovement.transform);
                        SfxManager.Instance.PlayBubbleRegenerate();
                        playerMovement.currentBubbleShield = bubble;
                    }
                    break;

                case BoostType.Fuel:
                    playerMovement.ApplyFuelPercentBoost(percentageFuelBoost);
                    break;
            }

            Destroy(gameObject);
        }
    }
}
