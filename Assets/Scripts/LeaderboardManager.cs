using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    public static LeaderboardManager Instance;
    
    [System.Serializable]
    public class LeaderboardEntry
    {
        public string playerName;
        public float score;

        public LeaderboardEntry(string name, float score)
        {
            this.playerName = name;
            this.score = score;
        }
    }

    //[SerializeField] private TextMeshProUGUI leaderboardText;
    private const string LeaderboardKey = "Leaderboard";

    private List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();
    private const int MaxEntries = 10; // Limit the leaderboard to top 10 entries
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
    public void OnClickLeaderBoard()
    {
        InitializeFakePlayers();
        LoadLeaderboard();
        DisplayLeaderboard();
    }

    public void AddScore(string playerName, float score)
    {
        // Check if player already exists in the leaderboard
        LeaderboardEntry existingEntry = leaderboard.Find(entry => entry.playerName == playerName);

        if (existingEntry != null)
        {
            // If the score is higher than the existing score, update it
            if (score > existingEntry.score)
            {
                existingEntry.score = score;
            }
        }
        else
        {
            // If the player is new, add them to the leaderboard
            leaderboard.Add(new LeaderboardEntry(playerName, score));
        }

        // Sort the leaderboard by score (descending)
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));

        // Trim the leaderboard to the maximum number of entries
        if (leaderboard.Count > MaxEntries)
        {
            leaderboard.RemoveAt(leaderboard.Count - 1);
        }

        // Save and display the updated leaderboard
        SaveLeaderboard();
        DisplayLeaderboard();
    }

    private void InitializeFakePlayers()
    {
        // Check if the leaderboard already exists
        if (!PlayerPrefs.HasKey(LeaderboardKey))
        {
            leaderboard = new List<LeaderboardEntry>
            {
                new LeaderboardEntry("AI_Player1", Random.Range(500, 1000)),
                new LeaderboardEntry("AI_Player2", Random.Range(400, 900)),
                new LeaderboardEntry("AI_Player3", Random.Range(300, 800)),
                new LeaderboardEntry("AI_Player4", Random.Range(200, 700)),
                new LeaderboardEntry("AI_Player5", Random.Range(100, 600))
            };

            SaveLeaderboard(); // Save the initial leaderboard
        }
    }

    private void LoadLeaderboard()
    {
        if (PlayerPrefs.HasKey(LeaderboardKey))
        {
            string json = PlayerPrefs.GetString(LeaderboardKey);
            leaderboard = JsonUtility.FromJson<LeaderboardWrapper>(json).entries;
        }
    }

    private void SaveLeaderboard()
    {
        LeaderboardWrapper wrapper = new LeaderboardWrapper { entries = leaderboard };
        string json = JsonUtility.ToJson(wrapper);
        PlayerPrefs.SetString(LeaderboardKey, json);
        PlayerPrefs.Save();
    }

    private void DisplayLeaderboard()
    {
        ClearLeaders();
        //leaderboardText.text = "Leaderboard\n";
        for (int i = 0; i < leaderboard.Count; i++)
        {
            //leaderboardText.text += $"{i + 1}. {leaderboard[i].playerName} - {Mathf.FloorToInt(leaderboard[i].score)}\n";
            GameObject init = Instantiate(MenuManager.Instance.boardPrefab, MenuManager.Instance.Content);
            init.GetComponent<LeaderboardInfo>().rank.text = (i + 1).ToString();
            init.GetComponent<LeaderboardInfo>().playerName.text = leaderboard[i].playerName;
            Leader.Add(init);
        }
    }

    [System.Serializable]
    private class LeaderboardWrapper
    {
        public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
    }
    public List<GameObject> Leader = new List<GameObject>();
    public void ClearLeaders()
    {
        for (int i = 0; i < Leader.Count; i++)
        {
            Destroy(Leader[i]);
        }
        Leader = new List<GameObject>();
    }
}
