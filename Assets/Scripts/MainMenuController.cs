using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuController : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField] TMP_Text hintsAmount;

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

    [Header("reset")]
    [SerializeField] bool reset;
    int activeLvlPanel;
    private void Awake()
    {
        if(!PlayerPrefs.HasKey("Sound"))
           PlayerPrefs.SetInt("Sound", 0);
        if(!PlayerPrefs.HasKey("FindedWords"))
           PlayerPrefs.SetInt("FindedWords", 0);
        if (!PlayerPrefs.HasKey("TillHint"))
            PlayerPrefs.SetInt("TillHint", 0);
        if (!PlayerPrefs.HasKey("Hints"))
            PlayerPrefs.SetInt("Hints", 10);
        if (!PlayerPrefs.HasKey("Panel"))
            PlayerPrefs.SetInt("Panel", 0);

    }
    private void Start()
    {
        if(reset)
            PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Pause", 0);
        audioManager = AudioManager.instanse;
        leftl.SetActive(false);
        LvlPanel(0);
        Debug.Log(PlayerPrefs.GetInt("Sound", 0));
        if (PlayerPrefs.GetInt("Sound", 0) == 0)
            sound.sprite = soundOn;
        else sound.sprite = soundOf;
        SetStartPanel();
    }
    private void Update()
    {
        hintsAmount.text = PlayerPrefs.GetInt("Hints", 0).ToString()+"x";
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
        int x;
        x = PlayerPrefs.GetInt("Sound", 0);
        if (x == 0)
        {
            x = 1;
            sound.sprite = soundOf;
        }
        else if (x == 1)
        {
            x = 0;
            sound.sprite = soundOn;
        }
        PlayerPrefs.SetInt("Sound", x);
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
        activeLvlPanel = PlayerPrefs.GetInt("Panel", 0);
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
        PlayerPrefs.SetInt("Panel", activeLvlPanel);
    }
    void SetStartPanel()
    {
        activeLvlPanel = PlayerPrefs.GetInt("Panel", 0);
        for (int i=0;i<lvlPanels.Length;i++)
        {
            if (i == activeLvlPanel)
                lvlPanels[i].SetActive(true);
            else lvlPanels[i].SetActive(false);
        }
        if (activeLvlPanel == 0)
            leftl.SetActive(false);
        else if (activeLvlPanel == lvlPanels.Length - 1)
            right.SetActive(false);
    }

}
