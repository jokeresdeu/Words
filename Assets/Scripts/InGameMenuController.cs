using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameMenuController : MonoBehaviour
{
    [SerializeField] Sprite soundOn;
    [SerializeField] Sprite soundOf;
    [SerializeField] Image sound;
    string lvlKey;
    [SerializeField] GameObject inGameShop;
    private void Start()
    {
        PlayerPrefs.SetInt("Pause", 0);
        lvlKey = SceneManager.GetActiveScene().buildIndex.ToString();
        if (PlayerPrefs.GetInt("Sound", 0) == 0)
            sound.sprite = soundOn;
        else sound.sprite = soundOf;
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }

    public void Sound()
    {
        int x;
        x = PlayerPrefs.GetInt("Sound", 0);
        if(x==0)
        {
            x = 1;
            sound.sprite = soundOf;
        }
        else if(x==1)
        {
            x = 0;
            sound.sprite = soundOn;
        }
        PlayerPrefs.SetInt("Sound", x);
    }

    public void Reset()
    {
        PlayerPrefs.DeleteKey("Save" + lvlKey);
        int totalWords = PlayerPrefs.GetInt("FindedWords", 0);
        int words = PlayerPrefs.GetInt("Words" + lvlKey);
        totalWords -= words;
        PlayerPrefs.DeleteKey("Words" + lvlKey);
        PlayerPrefs.SetInt("FindedWords", totalWords);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void InGameShop()
    {
        int pause = PlayerPrefs.GetInt("Pause", 0);
        if (pause == 1 && !inGameShop.activeInHierarchy)
            return;
        PlayerPrefs.SetInt("Pause", Mathf.Abs(pause - 1));
        inGameShop.SetActive(!inGameShop.activeInHierarchy);
    }
}
