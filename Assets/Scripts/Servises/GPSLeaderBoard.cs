using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GPSLeaderBoard : MonoBehaviour
{
    string[] leaderBoardID;
    #region Singleton
    public static GPSLeaderBoard instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
        leaderBoardID = new string[] { "CgkIlMzQ0KkbEAIQAA", "CgkIlMzQ0KkbEAIQAQ"};
    }

    #endregion
    public void OpenLeaderBoard(int lvl)
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderBoardID[lvl]);
    }
    public void UpdateLeaderBoardScore(string parametr, int id)
    {
        int score = PlayerPrefs.GetInt(parametr, 0);
        Debug.Log(score);
        Social.ReportScore(score, leaderBoardID[id], (bool success) => { });
    }
}


