using UnityEngine;

public class PlayBounce : MonoBehaviour
{
    public void PlayBounceSFX()
    {
        SfxManager.Instance.PlayMinigameBounce();
    }
}
