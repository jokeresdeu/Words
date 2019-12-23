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
        if (x == 0)
            sound.sprite = soundOf;
        else sound.sprite = soundOn;
        x -= 1;
        PlayerPrefs.SetInt("Sound", Mathf.Abs(x));
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
