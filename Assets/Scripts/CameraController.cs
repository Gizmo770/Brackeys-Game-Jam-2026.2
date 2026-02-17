using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerMovement playerMovement;

    public Transform target;

    [Header("Follow")]
    public float lookAheadDistance = 2f;

    [Header("Zoom")]
    public float normalZoom = 10f;
    public float maxZoomOut = 15f;

    private Camera cam;

    private void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        cam = Camera.main;
        cam.orthographicSize = normalZoom;
    }

    void LateUpdate()
    {
        float currentPlayerSpeed = playerMovement.GetComponent<Rigidbody2D>().linearVelocity.magnitude;

        // Camera follow with look-ahead
        Vector3 targetPosition = target.position + (target.up * lookAheadDistance);
        targetPosition.z = -10;
        transform.position = targetPosition;

        // Speed-based zoom
        float currentSpeed = currentPlayerSpeed;
        float speedPercent = Mathf.Clamp01(currentSpeed / playerMovement.currentSpeed);
        cam.orthographicSize = Mathf.Lerp(normalZoom, maxZoomOut, speedPercent);

        if (Mathf.Abs(cam.orthographicSize - normalZoom) < 0.1f)
        {
            cam.orthographicSize = normalZoom;
        }
    }
}
