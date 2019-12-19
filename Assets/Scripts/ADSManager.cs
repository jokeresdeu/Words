using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Monetization;

public class ADSManager : MonoBehaviour
{
    public static ADSManager instance;
    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
        }
    }
    string store_id = "3398533";
    string video_ad = "video";
    string rewardedVideo_ad = "rewardedVideo";

    void Start()
    {
        Monetization.Initialize(store_id, false);
        InvokeRepeating("PlayVideo", 600f, 600f);
    }

    public void PlayVideo()
    {
        if (PlayerPrefs.GetInt("ADSRemoved", 0) == 1)
            return;
        if (Monetization.IsReady(video_ad))
        {
            ShowAdPlacementContent id = null;
            id = Monetization.GetPlacementContent(video_ad) as ShowAdPlacementContent;
            if (id != null)
            {
                id.Show();
            }
        }
    }
    public void PlayRewardVideo()
    {
        if (Monetization.IsReady(video_ad))
        {
            ShowAdPlacementContent id = null;
            id = Monetization.GetPlacementContent(rewardedVideo_ad) as ShowAdPlacementContent;
            if (id != null)
            {
                id.Show();
                int x;
                x = PlayerPrefs.GetInt("Hints", 0);
                x += 5;
                PlayerPrefs.SetInt("Hints", x);
            }
        }
    }
}
