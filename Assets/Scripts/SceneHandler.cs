using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    public RectTransform fader;

    private void Start()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha (fader, 1, 0);
        LeanTween.alpha (fader, 0, 0.5f).setOnComplete (() => {
            fader.gameObject.SetActive (false);
        });
    }

    public void TransitionToPlay()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() =>
        {
            Invoke("Play", 0.5f);
        });
    }

    public void TransitionToHTP()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() =>
        {
            Invoke("HTP", 0.5f);
        });
    }

    public void TransitionBackToMenu()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() =>
        {
            Invoke("Menu", 0.5f);
        });
    }


    public void TransitionToCredits()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() =>
        {
            Invoke("Credits", 0.5f);
        });
    }


    public void Exit()
    {
        Application.Quit();
    }

    private void Credits()
    {
        SceneManager.LoadScene(3);
    }

    private void Menu()
    {
        SceneManager.LoadScene(0);
    }


    private void HTP()
    {
        SceneManager.LoadScene(2);
    }

    private void Play()
    {
        SceneManager.LoadScene(1);
    }
}
