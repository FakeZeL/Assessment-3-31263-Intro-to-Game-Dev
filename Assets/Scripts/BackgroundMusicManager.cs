using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : MonoBehaviour
{
    public AudioClip introMusic;
    public AudioClip normalMusic;

    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        PlayIntroMusic();

    }

    public void PlayIntroMusic()
    {
        audioSource.clip = introMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayNormalMusic()
    {
        audioSource.clip = normalMusic;
        audioSource.loop = true;
        audioSource.Play();
    }
}