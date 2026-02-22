using UnityEngine;
using UnityEngine.UI;

public class Launchpad : MonoBehaviour
{
    public PlayerMovement playerMovement;

    private bool angleSet = false;
    private bool speedSet = false;

    [Header("Launchpad Settings")]
    public float angleSliderInitSpeed = 1f;
    public float speedSliderInitSpeed = 1f;
    public float timeBeforeLaunch = 0.5f;

    [Header("UI/Animator Elements")]
    public Animator angleSliderAnims;
    public Animator speedSliderAnims;
    public Slider angleSlider;
    public Slider speedSlider;

    private float launchAngle;
    private float launchSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SfxManager.Instance.FadeInSpaceTheme();
        angleSliderAnims.speed = angleSliderInitSpeed;

        if(playerMovement == null)
        {
            playerMovement = Object.FindFirstObjectByType<PlayerMovement>();
        }
        angleSet = false;
        speedSet = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !angleSet)
        {
            angleSliderAnims.speed = 0;
            // LaunchAngle = AngleSlider's value
            launchAngle = angleSlider.value;
            angleSet = true;
            speedSliderAnims.speed = speedSliderInitSpeed;
            SfxManager.Instance.PlayMinigameDing();
            speedSliderAnims.SetTrigger("StartSpeedSlider");
        }
        else if (!speedSet && Input.GetKeyDown(KeyCode.Space))
        {
            {
                speedSliderAnims.speed = 0;
                // Launchspeed = Percent distance from center (5 is max launch speed).
                float distanceFromCenter = Mathf.Abs(speedSlider.value - 5);
                float normalized = distanceFromCenter / 5f;
                launchSpeed = (1f - normalized) * playerMovement.topSpeed;
                speedSet = true;
                SfxManager.Instance.PlayMinigameDing();

                // Initiate launch
                StartCoroutine(playerMovement.Launch(launchAngle, launchSpeed, timeBeforeLaunch));
            }
        }
    }
}
