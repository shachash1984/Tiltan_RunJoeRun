using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    

    private AudioSource[] audioSources;
    public AudioSource backgroundMusic; 
    public AudioSource coinCollectSound;
    public AudioSource jumpSound;
    public AudioSource clickSound;

    // Use this for initialization

    void Awake()
    {
        audioSources = GetComponents<AudioSource>();
        backgroundMusic = audioSources[0];
        coinCollectSound = audioSources[1];
        jumpSound = audioSources[2];
        clickSound = audioSources[3];
    }
	void Start () {

        
        PlayBGMusic();
	}
	
	public void PlayCoinCollectSound()
    {
        if (DataManager.S.GetSFX() == 1)
            coinCollectSound.Play();
    }

    public void PlayJumpSound()
    {
        if (DataManager.S.GetSFX() == 1)
            jumpSound.Play();
    }

    public void PlayBGMusic()
    {
        if (DataManager.S.GetMusic() == 1)
            backgroundMusic.Play();
    }

    public void PlayClickSound()
    {
        if (DataManager.S.GetSFX() == 1)
            clickSound.Play();
    }

    public void StopBGMusic()
    {
        backgroundMusic.Stop();
    }

}
