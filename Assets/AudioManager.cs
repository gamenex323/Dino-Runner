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
    void SetAudio()
    {
        buttonSound.volume = PlayerPrefs.GetFloat("Sound");
        bgm.volume = PlayerPrefs.GetFloat("Music");
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
}
