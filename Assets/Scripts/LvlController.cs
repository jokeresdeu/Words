using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LvlController : MonoBehaviour
{
    [SerializeField] string lvlKey;
    [SerializeField] int totalWords;
    [SerializeField] int wordsNeeded;
    [SerializeField] TMP_Text wordsToOpen;
    [SerializeField] TMP_Text wordsFinded;
    [SerializeField] GameObject closeMenu;
    Button button;
    
    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        if (!PlayerPrefs.HasKey("OpenLvl" + lvlKey))
            PlayerPrefs.SetInt("OpenLvl" + lvlKey, 0);
        if (wordsNeeded<= PlayerPrefs.GetInt("FindedWords", 0)||PlayerPrefs.GetInt("OpenLvl"+lvlKey,0)==1)
        {
            OpenLvl();
        }
        else
        {
            wordsToOpen.text = PlayerPrefs.GetInt("FindedWords",0).ToString() + "/" + wordsNeeded.ToString();
        }
        
    }
    public void LoadLevel()
    {
        if (PlayerPrefs.GetInt("Pause", 0) == 1)
            return;
        SceneManager.LoadScene(lvlKey.ToString());
    }
    void OpenLvl()
    {
        closeMenu.SetActive(false);
        button.enabled = true;
        wordsFinded.text = PlayerPrefs.GetInt("Words" + lvlKey).ToString() + "/" + totalWords.ToString();
    }
    public void UseKey()
    {
        
        int hints = PlayerPrefs.GetInt("Hints", 0);
        if (hints < 10)
            return;
        PlayerPrefs.SetInt("OpenLvl" + lvlKey, 1);
        hints -= 10;
        OpenLvl();
        PlayerPrefs.SetInt("Hints", hints);
    }
    
}
