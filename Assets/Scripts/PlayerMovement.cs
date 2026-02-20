using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static ShipStats ShipStats;

    private Rigidbody2D rb;
    private Vector2 playerInput;
    private bool launchStarted = false;
    public bool canMove = false;

    [Header("Engine")]
    public float currentSpeed;
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

        FinUpgrade fins = ShipStats.finUpgrade;
        maxTurnSpeed = fins.maxTurnSpeed;

        HullUpgrade hull = ShipStats.hullUpgrade;
        //appliedHealth = hull.health;
        //appliedSpeedLossMultiplier = hull.speedLossMultiplier;

        DefenseUpgrade defense = ShipStats.defenseUpgrade;
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

        // Deceleration no reverse
        if (playerInput.y <= 0)
        {
            rb.AddForce(-rb.linearVelocity.normalized * decceleration, ForceMode2D.Force);
            if (rb.linearVelocity.magnitude < 0.05f)
            {
                rb.linearVelocity = Vector2.zero;
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
