using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerMovement playerMovement;

    [Header("Follow")]
    public float moveSpeed;
    public float maxLookAheadDistance;

    [Header("Zoom")]
    public float zoomSpeed;
    public float normalZoom;
    public float maxZoomOut;
    public float maxZoomPlayerSpeed;

    private Camera cam;
    private Vector3 offset;

    private void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
        cam = Camera.main;
        cam.orthographicSize = normalZoom;
    }

    void LateUpdate()
    {
        float playerYSpeed = Mathf.Clamp(playerMovement.GetComponent<Rigidbody2D>().linearVelocity.y, -maxZoomPlayerSpeed, maxZoomPlayerSpeed);
        float zoomPercent = 1 - ((maxZoomPlayerSpeed - playerYSpeed) / maxZoomPlayerSpeed);

        float targetZoom = normalZoom + ((maxZoomOut - normalZoom) * Mathf.Abs(zoomPercent));
        cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, targetZoom, zoomSpeed * Time.deltaTime);

        Vector3 targetOffset = Vector3.up * (maxLookAheadDistance * zoomPercent);
        offset = Vector3.MoveTowards(offset, targetOffset, moveSpeed * Time.deltaTime);
        Vector3 targetPosition = playerMovement.transform.position + offset;
        targetPosition.z = -10;
        transform.position = targetPosition;
    }
}
