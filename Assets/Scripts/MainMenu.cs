using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        SfxManager.Instance.FadeInShopTheme();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StopSpaceTheme()
    {
        SfxManager.Instance.FadeOutSpaceTheme();
    }
}
