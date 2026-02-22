using System.Collections.Generic;
using UnityEngine;

public class Backdrop : MonoBehaviour
{
    public float planetXOffset;
    public float planetTravelDistance;
    public float planetXTravelDistance;
    public List<Color> tintColors;
    public List<Sprite> cloudSprites;
    public List<Sprite> planetSprites;

    public GameObject player;
    public ProgressTracker progressTracker;

    public GameObject backdrop1;
    public GameObject backdrop2;
    public GameObject backdrop3;
    public GameObject backdrop4;
    public GameObject backdrop5;
    public GameObject backdrop6;
    public GameObject backdrop7;
    public GameObject backdrop8;
    public GameObject backdrop9;

    public List<SpriteRenderer> tints;
    public List<SpriteRenderer> clouds;
    public GameObject planet;

    void Update()
    {
        Vector2 playerPosition = player.transform.position;
        float xOffset = playerPosition.x % 1000;
        float yOffset = playerPosition.y % 1000;


        backdrop1.transform.position = new Vector2(playerPosition.x - (xOffset/2), playerPosition.y - (yOffset / 2));
        backdrop2.transform.position = (Vector2)backdrop1.transform.position + Vector2.up * 1000;
        backdrop3.transform.position = (Vector2)backdrop1.transform.position + Vector2.up * -1000;
;
        backdrop4.transform.position = (Vector2)backdrop1.transform.position + Vector2.right * 1000;
        backdrop5.transform.position = (Vector2)backdrop1.transform.position + Vector2.right * 1000 + Vector2.up * 1000;
        backdrop6.transform.position = (Vector2)backdrop1.transform.position + Vector2.right * 1000 + Vector2.up * -1000;

        backdrop7.transform.position = (Vector2)backdrop1.transform.position + Vector2.right * -1000;
        backdrop8.transform.position = (Vector2)backdrop1.transform.position + Vector2.right * -1000 + Vector2.up * 1000;
        backdrop9.transform.position = (Vector2)backdrop1.transform.position + Vector2.right * -1000 + Vector2.up * -1000;

        int zone = progressTracker.zone;
        float zoneProgress = progressTracker.zoneProgress;

        if(progressTracker.zone >= 4)
        {
            return;
        }
        Color tintColor = new Color(tintColors[zone].r, tintColors[zone].g, tintColors[zone].b, tintColors[zone].a * (1 - Mathf.Abs(.5f - zoneProgress) * 2));
        foreach (SpriteRenderer tint in tints)
        {
            tint.color = tintColor;
        }

        Color cloudColor = new Color(1, 1, 1, 1 - (Mathf.Abs(.5f - zoneProgress) * 2));
        foreach (SpriteRenderer cloud in clouds)
        {
            cloud.sprite = cloudSprites[zone];
            cloud.color = cloudColor;
            cloud.size = new Vector2(500, 500);
        }

        planet.GetComponent<SpriteRenderer>().sprite = planetSprites[zone];
        float planetYOffset = planetTravelDistance - (planetTravelDistance * (zoneProgress * 2));
        planet.transform.position = new Vector2(playerPosition.x + planetXOffset - (playerPosition.x / 10) + (planetXTravelDistance - (planetXTravelDistance * (zoneProgress * 2))), playerPosition.y + planetYOffset);
    }
}
