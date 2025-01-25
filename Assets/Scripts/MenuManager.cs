using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject boardPrefab;
    public Transform Content;
    [Header("All Canvas")]
    [Space(3)]
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject Setting;
    [SerializeField] GameObject Leaderboard;
    public TextMeshProUGUI shieldAmount;
    public TextMeshProUGUI jumpAmount;
    public TextMeshProUGUI rumFastAmount;
    public TextMeshProUGUI coins;
    public GameObject noCoinsWarning;
    //[SerializeField] GameObject GamePlay;
    //[SerializeField] GameObject Pause;
    //[SerializeField] public GameObject GameOver;
    public static MenuManager Instance { get; private set; }
    private void Awake()
    {
        shieldAmount.text = PlayerPrefs.GetInt("Shield").ToString();
        rumFastAmount.text = PlayerPrefs.GetInt("RunFast").ToString();
        jumpAmount.text = PlayerPrefs.GetInt("Jump").ToString();
        coins.text = PlayerPrefs.GetInt("Coins").ToString();
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
        if (PlayerPrefs.GetInt("FirstRun") == 0)
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

    public void BuyShield(int withCoins)
    {
        if (PlayerPrefs.GetInt("Coins") >= withCoins)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - withCoins);
            coins.text = PlayerPrefs.GetInt("Coins").ToString();

            PlayerPrefs.SetInt("Shield", PlayerPrefs.GetInt("Shield") + 1);
            shieldAmount.text = PlayerPrefs.GetInt("Shield").ToString();

        }
        else
        {
            noCoinsWarning.SetActive(true);
            DG.Tweening.DOVirtual.DelayedCall(2f, () => noCoinsWarning.SetActive(false));
        }
    }
    public void BuyJump(int withCoins)
    {
        if (PlayerPrefs.GetInt("Coins") >= withCoins)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - withCoins);
            coins.text = PlayerPrefs.GetInt("Coins").ToString();

            PlayerPrefs.SetInt("Jump", PlayerPrefs.GetInt("Jump") + 1);
            jumpAmount.text = PlayerPrefs.GetInt("Jump").ToString();


        }
        else
        {
            noCoinsWarning.SetActive(true);
            DG.Tweening.DOVirtual.DelayedCall(2f, () => noCoinsWarning.SetActive(false));
        }
    }
    public void BuyBoost(int withCoins)
    {
        if (PlayerPrefs.GetInt("Coins") >= withCoins)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - withCoins);
            coins.text = PlayerPrefs.GetInt("Coins").ToString();

            PlayerPrefs.SetInt("RunFast", PlayerPrefs.GetInt("RunFast") + 1);
            rumFastAmount.text = PlayerPrefs.GetInt("RunFast").ToString();

        }
        else
        {
            noCoinsWarning.SetActive(true);
            DG.Tweening.DOVirtual.DelayedCall(2f, () => noCoinsWarning.SetActive(false));
        }
    }

    #region Setting Functions
    [Header("Setting Sliders")]
    [Space(3)]
    [SerializeField] Slider MusicSlider;
    [SerializeField] Slider SoundSlider;
    public void OnChangeMusic(float value)
    {
        AudioManager.instance.bgm.volume = value;
        PlayerPrefs.SetFloat("Music", value);
    }
    public void OnChangeSound(float value)
    {
        AudioManager.instance.buttonSound.volume = value;
        PlayerPrefs.SetFloat("Sound", value);
        //jioj
    }
    public void LoadMusicAndSound()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("Music");
        SoundSlider.value = PlayerPrefs.GetFloat("Sound");
    }
    public void PlayClickSound()
    {
        AudioManager.instance.buttonSound.Play();
    }
    #endregion
}
