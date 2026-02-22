using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    private bool iWinGame = false;

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

        if(zone >= 4)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        if(!iWinGame)
        {
            iWinGame = true;
            Object.FindFirstObjectByType<SceneTransition>().TriggerSceneChangeWhite("VictoryScene");
            FindAnyObjectByType<PlayerMovement>().canMove = false;
            SfxManager.Instance.FadeOutThrusters();
        }
    }
}
