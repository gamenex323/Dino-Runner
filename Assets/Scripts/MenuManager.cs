using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject boardPrefab;
    public Transform Content;
    [Header("All Canvas")]
    [Space(3)]
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject Setting;
    [SerializeField] GameObject Leaderboard;
    //[SerializeField] GameObject GamePlay;
    //[SerializeField] GameObject Pause;
    //[SerializeField] public GameObject GameOver;
    public static MenuManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        if(PlayerPrefs.GetInt("FirstRun") == 0)
        {
            PlayerPrefs.SetInt("FirstRun", 1);
            PlayerPrefs.SetFloat("Music", 1);
            PlayerPrefs.SetFloat("Sound", 1);
        }
        LoadMusicAndSound();
    }
    public void OnClickPlay()
    {
        PlayClickSound();
        SceneManager.LoadScene(1);
    }
    public void OnClickSetting()
    {
        PlayClickSound();

        MainMenu.SetActive(false);
        Setting.SetActive(true);
    }
    public void OnClickLeaderBoard()
    {
        PlayClickSound();

        MainMenu.SetActive(false);
        Leaderboard.SetActive(true);
        LeaderboardManager.Instance.OnClickLeaderBoard();
    }
    public void OnClickExit()
    {
        PlayClickSound();

        Application.Quit();
    }
    public void OnClickMainMenu()
    {
        PlayClickSound();
        Leaderboard.SetActive(false);
        Setting.SetActive(false);  
        MainMenu.SetActive(true);
    }

    #region Setting Functions
    [Header("Setting Sliders")]
    [Space(3)]
    [SerializeField] Slider MusicSlider;
    [SerializeField] AudioSource MusicSource;
    [SerializeField] Slider SoundSlider;
    [SerializeField] AudioSource SoundSource;
    public void OnChangeMusic(float value)
    {
        MusicSource.volume = value;
        PlayerPrefs.SetFloat("Music", value);
    }
    public void OnChangeSound(float value)
    {
        SoundSource.volume = value;
        PlayerPrefs.SetFloat("Sound", value);
    }
    public void LoadMusicAndSound()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("Music");
        SoundSlider.value = PlayerPrefs.GetFloat("Sound");
    }
    public void PlayClickSound()
    {
        SoundSource.Play();
    }
    #endregion
}
