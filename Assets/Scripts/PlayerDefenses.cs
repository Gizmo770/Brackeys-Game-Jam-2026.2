using UnityEngine;

public class PlayerDefenses : MonoBehaviour
{
    [Header("Shield Generator")]
    public float shieldRegenTime;
    public GameObject bubbleGameObject;
    [Header("Dash")]
    public float dashForce;
    public float dashRegenTime;

    [Header("Blaster")]
    public Vector2 blasterOffset;
    public float blasterTime;
    public GameObject blasterGameObject;


    private PlayerMovement playerMovement;

    private float shieldTimer;
    private float dashTimer;
    private float blasterTimer;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        shieldTimer = shieldRegenTime;
        blasterTimer = blasterTime;
    }

    void Update()
    {
        if (playerMovement.canMove)
        {
            if (playerMovement.defenseLevel >= 1)
            {
                if (playerMovement.currentBubbleShield)
                {
                    shieldTimer = shieldRegenTime;
                }
                else
                {
                    shieldTimer -= Time.deltaTime;
                    if (shieldTimer <= 0)
                    {
                        shieldTimer = shieldRegenTime;
                        GameObject bubble = Instantiate(bubbleGameObject, playerMovement.transform);
                        playerMovement.currentBubbleShield = bubble;
                    }
                }
            }

            if (playerMovement.defenseLevel >= 2)
            {
                if (dashTimer <= 0 && Input.GetKeyDown(KeyCode.LeftShift) && playerMovement.currentFuel > 0)
                {
                    if (Input.GetKey(KeyCode.D))
                    {
                        playerMovement.RightThrust();
                        playerMovement.rb.AddForce(playerMovement.transform.right * dashForce, ForceMode2D.Impulse);
                        playerMovement.currentFuel -= 8f;
                        dashTimer = dashRegenTime;
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        playerMovement.LeftThrust();
                        playerMovement.rb.AddForce(-playerMovement.transform.right * dashForce, ForceMode2D.Impulse);
                        playerMovement.currentFuel -= 8f;
                        dashTimer = dashRegenTime;
                    }
                }
                else
                {
                    dashTimer -= Time.deltaTime;
                }
            }

            if (playerMovement.defenseLevel >= 3)
            {
                if (blasterTimer <= 0)
                {
                    RaycastHit2D[] hits = Physics2D.RaycastAll((Vector2)playerMovement.transform.position + blasterOffset, playerMovement.transform.up, 20);
                    foreach(RaycastHit2D hit in hits)
                    {
                        if (hit && hit.collider.tag == "Enemy")
                        {
                            Instantiate(blasterGameObject, (Vector2)playerMovement.transform.position + blasterOffset, playerMovement.transform.rotation);
                            blasterTimer = blasterTime;
                            break;
                        }
                    }
                }
                else
                {
                    blasterTimer -= Time.deltaTime;
                }
            }
        }
    }
}
