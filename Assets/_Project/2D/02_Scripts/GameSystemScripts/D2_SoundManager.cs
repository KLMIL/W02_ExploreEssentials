using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;


public class D2_SoundManager : MonoBehaviour
{
    public static D2_SoundManager Instance { get; private set; }
    //private AudioSource audio;

    // bullet Audio
    //public AudioClip backgroundHitSound;
    //public AudioClip pointCollectSound;
    //public AudioClip bombCollectSound;
    //public AudioClip bombSfx;
    //public AudioClip magneticSfx;
    //public AudioClip knockBackSfx;

    public List<AudioClip> audios;



    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        //audio = GetComponent<AudioSource>();   
    }


    public void PlaySFX(AudioClip sfx, float volume, float time)
    {
        PlaySFX(sfx, volume);
        Invoke("StopPlay", time);
    }
    public void PlaySFX(AudioClip sfx, float volume)
    {
        GetComponent<AudioSource>().PlayOneShot(sfx, volume);
    }

    public void PlaySFX(AudioClip sfx)
    {
        PlaySFX(sfx, 0.8f);
    }


    private void StopPlay()
    {
        GetComponent<AudioSource>().Stop();
    }
}
