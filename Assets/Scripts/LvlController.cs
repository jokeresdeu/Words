using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LvlController : MonoBehaviour
{
    bool isLvlOpen;
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
        if(wordsNeeded<= PlayerPrefs.GetInt("FindedWords", 0))
        {
            closeMenu.SetActive(false);
            button.enabled = true;
            wordsFinded.text = PlayerPrefs.GetInt("Words" + lvlKey).ToString() + "/" + totalWords.ToString();
        }
        else
        {
            button.enabled = false;
            wordsToOpen.text = PlayerPrefs.GetInt("Words" + lvlKey).ToString() + "/" + wordsNeeded.ToString();
        }
        
    }
    public void LoadLevel()
    {
        if (PlayerPrefs.GetInt("Pause", 0) == 1)
            return;
        SceneManager.LoadScene(lvlKey.ToString());
    }
}
