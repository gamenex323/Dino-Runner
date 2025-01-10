using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.1f;
    public float gameSpeed { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreGameOverText;
    [SerializeField] private TextMeshProUGUI hiscoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    //[SerializeField] private Button retryButton;

    [SerializeField] private GameObject[] healthIcons;
    [SerializeField] private GameObject GameOverPanel;
    [SerializeField] private GameObject GamePausePanel;

    private Player player;
    private Spawner spawner;

    private float score;
    public float Score => score;

    private bool isSpeedBoostActive = false;
    private bool isShieldActive = false;
    private bool isDoubleJumpActive = false;

    private float speedBoostDuration = 5f;
    private float shieldDuration = 5f;
    private float doubleJumpDuration = 5f;

    private float speedBoostTimer;
    private float shieldTimer;
    private float doubleJumpTimer;
    [SerializeField] GameObject [] Maps;
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

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawner>();

        NewGame();
    }

    public void NewGame()
    {
        DG.Tweening.DOVirtual.DelayedCall(25, () => UpdateMap());
        GameOverPanel.SetActive(false);

        player.Jumper.SetActive(false);
        player.DisableDoubleJump();
        isDoubleJumpActive = false;

        player.Shield.SetActive(false);
        player.DisableShield();
        isShieldActive = false;

        player.Booster.SetActive(false);
        isSpeedBoostActive = false;
        
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        score = 0f;
        gameSpeed = initialGameSpeed;
        enabled = true;

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        //retryButton.gameObject.SetActive(false);

        UpdateHiscore();
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);
        GameOverPanel.SetActive(true);
        //gameOverText.gameObject.SetActive(true);
        //retryButton.gameObject.SetActive(true);
        //MenuManager.Instance.GameOver.SetActive(true);
        UpdateHiscore();
    }

    private void Update()
    {
        if(!isSpeedBoostActive)
            gameSpeed += gameSpeedIncrease * Time.deltaTime;
        score += gameSpeed * Time.deltaTime;
        scoreText.text = Mathf.FloorToInt(score).ToString("D5");
        scoreGameOverText.text = Mathf.FloorToInt(score).ToString("D5");
        
        // Handle powerup timers
        HandlePowerupTimers();
    }
    int j = 0;
    void UpdateMap()
    {
        //if (score % 100 == 0)
        {
            for (int i = 0; i < Maps.Length; i++)
            {
                Maps[i].SetActive(false);
            }
            if(j >= Maps.Length)
            {
                j = 0;
            }
            Maps[j].SetActive(true);
            j++;
        }
        DG.Tweening.DOVirtual.DelayedCall(25, () => UpdateMap());
    }
    private void UpdateHiscore()
    {
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {
            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }
        LeaderboardManager.Instance.AddScore("YOU", hiscore);
        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString("D5");
    }

    private void HandlePowerupTimers()
    {
        // Speed Boost
        if (isSpeedBoostActive)
        {
            speedBoostTimer -= Time.deltaTime;
            if (speedBoostTimer <= 0)
            {
                isSpeedBoostActive = false;
                player.Booster.SetActive(false);
                gameSpeed -= 5f; // Revert the speed boost
            }
        }

        // Shield
        if (isShieldActive)
        {
            shieldTimer -= Time.deltaTime;
            if (shieldTimer <= 0)
            {
                isShieldActive = false;
                player.Shield.SetActive(false);
                player.DisableShield(); // Implement in Player script
            }
        }

        // Double Jump
        if (isDoubleJumpActive)
        {
            doubleJumpTimer -= Time.deltaTime;
            if (doubleJumpTimer <= 0)
            {
                isDoubleJumpActive = false;
                player.Jumper.SetActive(false);
                player.DisableDoubleJump(); // Implement in Player script
            }
        }
    }
    // Speed Boost Powerup
    public void ActivateSpeedBoost()
    {
        if (!isSpeedBoostActive)
        {
            isSpeedBoostActive = true;
            player.Booster.SetActive(true);
            speedBoostTimer = speedBoostDuration;
            gameSpeed += 5f; // Increase speed temporarily
        }
    }

    // Shield Powerup
    public void ActivateShield()
    {
        if (!isShieldActive)
        {
            isShieldActive = true;
            player.Shield.SetActive(true);
            shieldTimer = shieldDuration;
            player.EnableShield(); // Implement in Player script
        }
    }

    // Double Jump Powerup
    public void ActivateDoubleJump()
    {
        if (!isDoubleJumpActive)
        {
            isDoubleJumpActive = true;
            player.Jumper.SetActive(true);
            doubleJumpTimer = doubleJumpDuration;
            player.EnableDoubleJump(); // Implement in Player script
        }
    }

    public void UpdateHealthUI(int currentHealth)
    {
        // Update numeric health display
        //healthText.text = $"Health: {currentHealth}";

        // Update heart icons (if applicable)
        for (int i = 0; i < healthIcons.Length; i++)
        {
            healthIcons[i].SetActive(i < currentHealth);
        }
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void OnClickPause() 
    {
        Time.timeScale = 0;
        GamePausePanel.SetActive(true);
    }

    public void OnClickResume() {
        Time.timeScale = 1;
        GamePausePanel.SetActive(true);
    }
}
