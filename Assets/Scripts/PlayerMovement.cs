using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 playerInput;
    private bool launchStarted = false;
    public bool canMove = false;

    [Header("Engine")]
    public float currentMaxSpeed;
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
        rb = GetComponent<Rigidbody2D>();
        canMove = false;
        launchStarted = false;
    }

    private void FixedUpdate()
    {
        // Get Input
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // TODO: Remove debug.
        currentFuel = maxFuel;

        if(canMove)
        {
            Thrust();
            Rotation();
        }
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
        if (rb.linearVelocity.magnitude > currentMaxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * currentMaxSpeed;
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
