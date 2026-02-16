using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 playerInput;
    
    [HideInInspector] public bool isPlayerMoving;

    [Header("Engine")]
    public float currentMaxSpeed;
    public float topSpeed;
    public float acceleration;
    public float decceleration;

    [Header("Fins")]
    public float turnSpeed;
    public float maxTurnSpeed;
    public float maneuverability = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            isPlayerMoving = true;
        }
        else
        {
            isPlayerMoving = false;
        }
    }

    private void FixedUpdate()
    {
        // Get Input
        playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        Thrust();
        Rotation();
        ApplyManeuverability();
    }

    private void Thrust()
    {
        // Forward movement
        if(playerInput.y > 0)
        {
            if (rb.linearVelocity.magnitude < currentMaxSpeed)
            {
                rb.AddForce(rb.transform.up * acceleration, ForceMode2D.Force);
            }
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

    // Adds a force opposite to the lateral velocity to simulate grip and prevent sliding
    private void ApplyManeuverability()
    {
        float lateralSpeed = Vector2.Dot(rb.linearVelocity, rb.transform.right);
        Vector2 gripForce = -rb.transform.right * lateralSpeed * maneuverability;
        rb.AddForce(gripForce, ForceMode2D.Force);
    }

}
