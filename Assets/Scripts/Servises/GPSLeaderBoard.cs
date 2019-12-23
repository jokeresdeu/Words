using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GPSLeaderBoard : MonoBehaviour
{
    string leaderBoardID = "CgkIlMzQ0KkbEAIQAA";
    #region Singleton
    public static GPSLeaderBoard instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        InvokeRepeating("UpdateLeaderBoardScore", 20f, 120f);
    }
    #endregion
    public void OpenLeaderBoard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderBoardID);
    }
    public void UpdateLeaderBoardScore()
    {
        int score = PlayerPrefs.GetInt("FindedWords", 0);
        Social.ReportScore(score, leaderBoardID, (bool success) => { });
    }
}


