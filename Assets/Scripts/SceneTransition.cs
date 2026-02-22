using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private Animator animator;
    private string nextScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerSceneChange(string sceneToChangeTo)
    {
        this.nextScene = sceneToChangeTo;
        animator.SetTrigger("SceneTransOut");
    }

    public void TriggerSceneChangeWhite(string sceneToChangeTo)
    {
        this.nextScene = sceneToChangeTo;
        animator.SetTrigger("SceneTransOutWhite");
    }

    public void LoadNextScene()
    {
       SceneManager.LoadScene(nextScene);
    }
}
