using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] public List<AudioClip> audios;

    private AudioSource controlAudio;

    public static SoundManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        controlAudio = GetComponent<AudioSource>();
    }

    public void SelectAudio(int index, float vol)
    {
        controlAudio.PlayOneShot(audios[index], vol);
    }
}
