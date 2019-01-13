using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public Image blackImage;
    public Animator animator;

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(Fade(sceneName));
    }

    IEnumerator Fade(string sceneName)
    {
        animator.SetBool("Fade", true);
        yield return new WaitUntil(() => blackImage.color.a == 1);
        SceneManager.LoadScene(sceneName);
    }
}