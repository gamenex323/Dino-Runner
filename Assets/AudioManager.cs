using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioManager instance;
    public AudioSource buttonSound;
    public AudioSource bgm;
    public AudioSource hitSound;
    public AudioSource jumpSound;
    public AudioSource slideSound;
    public AudioClip coinCollect;

    void Start()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
        SetAudio();

    }

    // Update is called once per frame
    public void SetAudio()
    {
        buttonSound.volume = PlayerPrefs.GetFloat("Sound");
        bgm.volume = PlayerPrefs.GetFloat("Music");
        hitSound.volume = PlayerPrefs.GetFloat("Sound");
        jumpSound.volume = PlayerPrefs.GetFloat("Sound");
        slideSound.volume = PlayerPrefs.GetFloat("Sound");
    }
    public void PlayClickSound()
    {
        SetAudio();
        buttonSound.Play();
    }
    public void PlayBGM()
    {
        SetAudio();
        bgm.Play();
    }
    public void HitSound()
    {
        hitSound.Play();
    }
    public void JumpSound()
    {
        jumpSound.Play();
    }
    public void SlideSound()
    {
        slideSound.Play();
    }
    public void CoinCollectSound()
    {
        GameObject audioObject = new GameObject("Coin Sound");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = coinCollect;
        audioSource.Play();
        Destroy(audioObject, coinCollect.length);
    }
}
