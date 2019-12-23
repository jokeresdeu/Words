using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField] GameObject about;
    [SerializeField] GameObject inGameShop;
    [Header("LvlsPanel")]
    [SerializeField] GameObject[] lvlPanels;
    [SerializeField] GameObject right;
    [SerializeField] GameObject leftl;
    [Space]
    [SerializeField] Sprite soundOn;
    [SerializeField] Sprite soundOf;

    [SerializeField] Image sound;

    int activeLvlPanel;
    private void Awake()
    {
        if(!PlayerPrefs.HasKey("Sound"))
           PlayerPrefs.SetInt("Sound", 0);
        if(!PlayerPrefs.HasKey("FindedWords"))
           PlayerPrefs.SetInt("FindedWords", 0);
        if (!PlayerPrefs.HasKey("TillHint"))
            PlayerPrefs.SetInt("TillHint", 0);
    }
    private void Start()
    {
        //PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Pause", 0);
        audioManager = AudioManager.instanse;
        leftl.SetActive(false);
    }
    public void Quit()
    {
        audioManager.Play("Button");
        Application.Quit();
    }
    public void About()
    {
        if (PlayerPrefs.GetInt("Pause", 0) == 1)
            return;
        audioManager.Play("OpenWindow");
        about.SetActive(!about.activeInHierarchy);
    }
    public void Sound()
    {
        audioManager.Play("Button");
        int x;
        x = PlayerPrefs.GetInt("Sound", 0);
        if (x == 0)
            sound.sprite = soundOf;
        else sound.sprite = soundOn;
        x -= 1;
        PlayerPrefs.SetInt("Sound", Mathf.Abs(x));
    }

    public void InGameShop()
    {
        int pause = PlayerPrefs.GetInt("Pause", 0);
        if (pause == 1 && !inGameShop.activeInHierarchy)
            return;
        PlayerPrefs.SetInt("Pause", Mathf.Abs(pause - 1));
        inGameShop.SetActive(!inGameShop.activeInHierarchy);
    }

    public void LvlPanel(int direction)
    {
        if (PlayerPrefs.GetInt("Pause", 0) == 1)
            return;
        audioManager.Play("Button");
        leftl.SetActive(true);
        right.SetActive(true);
        lvlPanels[activeLvlPanel].SetActive(false);
        activeLvlPanel += direction;
        lvlPanels[activeLvlPanel].SetActive(true);
        if (activeLvlPanel == 0)
            leftl.SetActive(false);
        else if (activeLvlPanel == lvlPanels.Length-1)
            right.SetActive(false);
    }

}
