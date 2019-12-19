//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using GooglePlayGames;
//using UnityEngine.SocialPlatforms;

//public class GPSLeaderBoard : MonoBehaviour
//{
//    [SerializeField] int lvl;
//    string[] leaderBoardID;
//    #region Singleton
//    public static GPSLeaderBoard instance;
//    private void Awake()
//    {
//        if (instance == null)
//            instance = this;
//        else Destroy(gameObject);
//        leaderBoardID = new string[]{"CgkIibz06cwPEAIQAg", "CgkIibz06cwPEAIQAw", "CgkIibz06cwPEAIQBA", "CgkIibz06cwPEAIQBQ", "CgkIibz06cwPEAIQBg", "CgkIibz06cwPEAIQBw", "CgkIibz06cwPEAIQCA"};
//    }
//    #endregion
//    public void OpenLeaderBoard(int lvl)
//    {
//        PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderBoardID[lvl]);
//    }
//    public void UpdateLeaderBoardScore(string lvlKey)
//    {
//        Social.ReportScore(PlayerPrefs.GetInt("HighScore" + lvlKey, 1), leaderBoardID[lvl], (bool success) => { });
//    }
//}
    //[SerializeField] string leaderBoardID;
    //#region Singleton
    //public static GPSLeaderBoard instance;
    //private void Awake()
    //{
    //    if (instance == null)
    //        instance = this;
    //    else Destroy(gameObject);
    //}
    //#endregion
    //public void OpenLeaderBoard(string lvlKey)
    //{
    //    PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderBoardID);
    //}
    //public void UpdateLeaderBoardScore(string lvlKey)
    //{
    //     Social.ReportScore(PlayerPrefs.GetInt("HighScore"+lvlKey, 1), leaderBoardID, (bool success) =>{});
    //}


