using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static ShipStats ShipStats;

    [HideInInspector]
    public Rigidbody2D rb;
    private Vector2 playerInput;
    private bool launchStarted = false;
    public bool canMove = false;

    [Header("Engine")]
    public float topSpeed;
    public float acceleration;
    public float decceleration;

    [Header("Fins")]
    public float turnSpeed;
    public float maxTurnSpeed;

    [Header("Engine")]
    public float currentFuel = 0f;
    public float maxFuel = 100f;
    public float fuelConsumptionRate = 10f;

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

    private void RecalculateAppliedStats()
    {
        EngineUpgrade engine = ShipStats.engineUpgrade;
        maxFuel = engine.maxFuel;
        topSpeed = engine.topSpeed;
        thrusterSprite.sprite = engine.sprite;

        FinUpgrade fins = ShipStats.finUpgrade;
        maxTurnSpeed = fins.maxTurnSpeed;
        fin1Sprite.sprite = fins.sprite1;
        fin2Sprite.sprite = fins.sprite2;

        HullUpgrade hull = ShipStats.hullUpgrade;
        hullSprite.sprite = hull.sprite;
        //appliedHealth = hull.health;
        //appliedSpeedLossMultiplier = hull.speedLossMultiplier;

        DefenseUpgrade defense = ShipStats.defenseUpgrade;
        defenseSprite.sprite = defense.sprite;
        //appliedDefensePrefab = defense.countermeasurePrefab;
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
        if (rb.linearVelocity.magnitude > topSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * topSpeed;
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
