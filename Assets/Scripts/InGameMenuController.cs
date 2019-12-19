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
    [SerializeField] string lvlKey;
    [SerializeField] GameObject inGameShop;

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void InGameShop()
    {
        inGameShop.SetActive(!inGameShop.activeInHierarchy);
    }
}
