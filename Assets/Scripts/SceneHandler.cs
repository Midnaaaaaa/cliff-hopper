using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{

    public RectTransform fader;

    public void TransitionToPlay()
    {
        fader.gameObject.SetActive(true);
        LeanTween.alpha(fader, 0, 0);
        LeanTween.alpha(fader, 1, 0.5f).setOnComplete(() =>
        {
            Invoke("Play", 0.5f);
        });
    }

    private void Play()
    {
        SceneManager.LoadScene(1);
    }
}
