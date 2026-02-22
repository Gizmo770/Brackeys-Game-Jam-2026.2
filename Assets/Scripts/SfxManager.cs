using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public float fadeOutDuration = 1f;

    public float defaultShopThemeVolume = 1f;
    private bool fadeInShopTheme = false;

    public float defaultSpaceThemeVolume = 1f;
    private bool fadeInSpaceTheme = false;

    public float defaultThrustingVolume = 1f;
    public bool fadeInThrusting = false;

    public AudioSource bubblePop;
    public AudioSource bubbleRegenerate;
    public AudioSource burstThrust;
    public AudioSource buttonDown;
    public AudioSource error;
    public AudioSource hitObstacle;
    public AudioSource laser;
    public AudioSource launching;
    public AudioSource minigameBounce;
    public AudioSource minigameDing;
    public AudioSource powerUp;
    public AudioSource purchase;
    public AudioSource shopThemeAirportLounge;
    public AudioSource spaceThemeMesmerizingGalaxyLoop;
    public AudioSource thrustingLooping;

    public static SfxManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        // Shop Theme
        if (!fadeInShopTheme)
        {
            shopThemeAirportLounge.volume -= Time.deltaTime;
        }
        if (fadeInShopTheme)
        {
            shopThemeAirportLounge.volume += Time.deltaTime;
            if (shopThemeAirportLounge.volume >= defaultShopThemeVolume)
            {
                shopThemeAirportLounge.volume = defaultShopThemeVolume;
            }
        }

        // Space Theme
        if (!fadeInSpaceTheme)
        {
            spaceThemeMesmerizingGalaxyLoop.volume -= Time.deltaTime;
        }
        if (fadeInSpaceTheme)
        {
            spaceThemeMesmerizingGalaxyLoop.volume += Time.deltaTime;
            if (spaceThemeMesmerizingGalaxyLoop.volume >= defaultSpaceThemeVolume)
            {
                spaceThemeMesmerizingGalaxyLoop.volume = defaultSpaceThemeVolume;
            }
        }

        // Thrusting Looping
        if (!fadeInThrusting)
        {
            thrustingLooping.volume -= Time.deltaTime;
        }
        if (fadeInThrusting)
        {
            thrustingLooping.volume += Time.deltaTime;
            if (thrustingLooping.volume >= defaultThrustingVolume)
            {
                thrustingLooping.volume = defaultThrustingVolume;
            }
        }
    }

    public void PlayBubblePop() => bubblePop.Play();
    public void PlayBubbleRegenerate() => bubbleRegenerate.Play();
    public void PlayBurstThrust() => burstThrust.Play();
    public void PlayButtonDown() => buttonDown.Play();
    public void PlayError() => error.Play();
    public void PlayHitObstacle() => hitObstacle.Play();
    public void PlayLaser() => laser.Play();
    public void PlayLaunching() => launching.Play();
    public void PlayMinigameBounce() => minigameBounce.Play();
    public void PlayMinigameDing() => minigameDing.Play();
    public void PlayPowerUp() => powerUp.Play();
    public void PlayPurchase() => purchase.Play();
    public void PlayShopThemeAirportLounge() => shopThemeAirportLounge.Play();
    public void PlaySpaceThemeMesmerizingGalaxyLoop() => spaceThemeMesmerizingGalaxyLoop.Play();
    public void PlayThrustingLooping() => thrustingLooping.Play();

    public void FadeOutShopTheme()
    {
        SfxManager.Instance.fadeInShopTheme = false;
    }

    public void FadeInShopTheme()
    {
        PlayShopThemeAirportLounge();
        SfxManager.Instance.fadeInShopTheme = true;
    }

    public void FadeOutSpaceTheme()
    {
        SfxManager.Instance.fadeInSpaceTheme = false;
    }

    public void FadeInSpaceTheme()
    {
        PlaySpaceThemeMesmerizingGalaxyLoop();
        SfxManager.Instance.fadeInSpaceTheme = true;
    }

    public void FadeOutThrusters()
    {
        SfxManager.Instance.fadeInThrusting = false;
    }

    public void FadeInThrusters() { 
        if (!thrustingLooping.isPlaying) {
            PlayThrustingLooping();
        }
        SfxManager.Instance.fadeInThrusting = true;
    }
}
