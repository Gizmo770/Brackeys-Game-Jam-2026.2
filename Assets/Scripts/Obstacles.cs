using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public float speedLoss;
    public float moveSpeed;
    public float rotateSpeed;
    public List<Sprite> sprites;

    private Vector2 moveDirection;
    private int rotateDirection;

    void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
        rotateDirection = Random.Range(1,2) == 0 ? 1 : -1;
        moveDirection = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + rotateDirection * rotateSpeed * Time.deltaTime);
        transform.position = (Vector2)transform.position + (moveSpeed * moveDirection * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Shield")
        {
            Destroy(collision.gameObject);
            Destroy(this.gameObject);
        }
        else if (collision.tag == "Player")
        {
            PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();

            if(playerMovement.currentBubbleShield == null)
            {
                playerMovement.ApplySpeedLossFromObstacle(speedLoss);
                Destroy(gameObject);
            }
        }
    }
}
