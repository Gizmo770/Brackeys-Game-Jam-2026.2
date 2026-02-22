using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static ShipStats ShipStats;
    public bool dead;

    [HideInInspector]
    public Rigidbody2D rb;
    private Vector2 playerInput;
    private bool launchStarted = false;
    public bool canMove = false;

    [Header("Engine")]
    public float topSpeed;
    public float minSpeed = 2;
    public float speedLossMultiplier = 1;
    public float acceleration;
    public float decceleration;
    public float currentFuel = 0f;
    public float maxFuel = 100f;
    public float fuelConsumptionRate = 10f;
    [HideInInspector] public GameObject currentBubbleShield;
    [HideInInspector] public float candyBoostMultiplier = 1f;
    public float candyBoostDecayRate = .5f;
    [HideInInspector] public float defenseLevel = 0f;

    [Header("Fins")]
    public float turnSpeed;
    public float maxTurnSpeed;

    [Header("Sprites")]
    public SpriteRenderer hullSprite;
    public SpriteRenderer thrusterSprite;
    public SpriteRenderer fin1Sprite;
    public SpriteRenderer fin2Sprite;
    public SpriteRenderer defenseSprite;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.EnsureBaseOwned();

        rb = GetComponent<Rigidbody2D>();
        canMove = false;
        launchStarted = false;
        candyBoostMultiplier = 1f;

        RecalculateAppliedStats();
    }

    private void FixedUpdate()
    {
        // Get Input
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (canMove)
        {
            Thrust();
            Rotation();
        }
    }

    public void RecalculateAppliedStats()
    {
        EngineUpgrade engine = ShipStats.engineUpgrade;
        topSpeed = engine.topSpeed;
        acceleration = engine.acceleration;
        thrusterSprite.sprite = engine.sprite;

        FinUpgrade fins = ShipStats.finUpgrade;
        maxTurnSpeed = fins.maxTurnSpeed;
        turnSpeed = fins.maxTurnSpeed;
        fin1Sprite.sprite = fins.sprite1;
        fin2Sprite.sprite = fins.sprite2;

        HullUpgrade hull = ShipStats.hullUpgrade;
        speedLossMultiplier = ShipStats.hullUpgrade.speedLossMultiplier;
        hullSprite.sprite = hull.sprite;
        maxFuel = hull.maxFuel;
        currentFuel = maxFuel;

        DefenseUpgrade defense = ShipStats.defenseUpgrade;
        defenseSprite.sprite = defense.sprite;
        defenseLevel = defense.defenseLevel;

    }

    private void Thrust()
    {
        // Forward movement
        if(playerInput.y > 0 && currentFuel >= 0f)
        {
            rb.AddForce(rb.transform.up * acceleration, ForceMode2D.Force);
            DrainFuel();
        }
        else
        {            
            if (rb.linearVelocity.magnitude < 0.05f)
            {
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                rb.AddForce(-rb.linearVelocity.normalized * decceleration, ForceMode2D.Force);
            }
        }

        // Speed limiter
        if (rb.linearVelocity.magnitude > topSpeed * candyBoostMultiplier)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * topSpeed;
        }

        if(candyBoostMultiplier > 1f)
        {
            candyBoostMultiplier -= Time.fixedDeltaTime * candyBoostDecayRate;
        }
    }

    private void Rotation()
    {
        rb.AddTorque(-playerInput.x * turnSpeed);

        if (rb.angularVelocity > maxTurnSpeed)
        {
            rb.angularVelocity = maxTurnSpeed;
        }
        else if (rb.angularVelocity < -maxTurnSpeed)
        {
            rb.angularVelocity = -maxTurnSpeed;
        }

        if (playerInput.x == 0)
        {
            rb.angularVelocity = 0;
        }
    }

    private void DrainFuel()
    {
        currentFuel -= fuelConsumptionRate * Time.fixedDeltaTime;
    }

    public void ApplySpeedLossFromObstacle(float speedLost)
    {

        if ((rb.linearVelocity.magnitude - (speedLost * speedLossMultiplier) <= minSpeed))
        {
            rb.linearVelocity = rb.linearVelocity.normalized * minSpeed;
        }
        else
        {
            rb.linearVelocity = rb.linearVelocity.normalized * (rb.linearVelocity.magnitude - (speedLost * speedLossMultiplier));
        }
    }

    public void ApplySpeedBoost(float speedBoostAmount, float candyBoostMultiplier)
    {
        this.candyBoostMultiplier = candyBoostMultiplier;
        rb.AddForce(rb.transform.up * speedBoostAmount, ForceMode2D.Impulse);
    }

    public void ApplyFuelPercentBoost(float percentFuelBoost)
    {
        currentFuel += maxFuel * percentFuelBoost;

        if(currentFuel > maxFuel)
        {
            currentFuel = maxFuel;
        }
    }

    public IEnumerator Launch(float launchAngle, float launchSpeed, float timeBeforeLaunch)
    {
        if(!launchStarted)
        {
            launchStarted = true;
            rb.rotation = launchAngle;
            yield return new WaitForSeconds(timeBeforeLaunch);
            rb.AddForce(rb.transform.up * launchSpeed, ForceMode2D.Impulse);
            canMove = true;
        }
    }
}
