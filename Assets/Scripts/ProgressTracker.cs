using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    public float zoneLength;

    public GameObject player;

    //[HideInInspector]
    public int zone;
    //[HideInInspector]
    public float zoneProgress;

    void Update()
    {
        Vector2 playerPosition = player.transform.position;

        zone = playerPosition.y > 0 ? Mathf.FloorToInt(playerPosition.y / zoneLength) : 0;
        zoneProgress = playerPosition.y > 0 ? (playerPosition.y % zoneLength) / zoneLength : 0;

        // If zoneProgress = 4, start a win game.
        // Fade to white, win game scene.
        // Pretty it up, button to return to main menu.
        if(zoneProgress >= 4)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        Object.FindFirstObjectByType<SceneTransition>().TriggerSceneChangeWhite("VictoryScene");
    }
}
