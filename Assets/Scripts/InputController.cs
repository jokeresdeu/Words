using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    FindedWord[] findedWords;
    LettersController[] letters;
    [TextArea][SerializeField] string wordsList;

   
    AudioManager audioManager;

    [SerializeField] TMP_Text inputZone;
    [SerializeField] TMP_Text wordsAmount;
    [SerializeField] TMP_Text totalWordsAmount;

    [SerializeField] GameObject forHints;
    [SerializeField] GameObject noWordsZone;
    [SerializeField] GameObject plaseForScore;
    [SerializeField] GameObject textPrefab;
    [SerializeField] GameObject wordIsFinded;
    [SerializeField] GameObject wordZone;
    [SerializeField] GameObject findedWordsZone;
    [SerializeField] GameObject winWindow;
   
    string lvlKey;
    string currentWord="";

    List<string> words = new List<string>();
    public List<string> Words { get { return words; } }
    List<string> wordsTemp = new List<string>();
    public List<string> WordsTemp { get { return wordsTemp; } }

    int x;
  

    #region Singleton
    public static InputController instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }
    #endregion

    void Start()
    {
        lvlKey = SceneManager.GetActiveScene().buildIndex.ToString();
        findedWords = findedWordsZone.GetComponentsInChildren<FindedWord>();
        letters = wordZone.GetComponentsInChildren<LettersController>();
        string[] temp = wordsList.Split(',');
        audioManager = AudioManager.instanse;
        foreach(string s in temp)
        {
            words.Add(s);
            wordsTemp.Add(s);
        }
        for(int i =0; i<findedWords.Length;i++)
        {
            findedWords[i].SetWord(words[i]);
        }
        Load();
        wordsAmount.text = x.ToString();
        totalWordsAmount.text = words.Count.ToString();
    }

    private void Load()
    {
        if (!PlayerPrefs.HasKey("Save" + lvlKey))
            PlayerPrefs.SetString("Save" + lvlKey, "");
        string save = PlayerPrefs.GetString("Save" + lvlKey);
        string[] temp = save.Split(',');
        if (temp.Length > 0)
        {
            foreach (string s in temp)
            {
                foreach (FindedWord finded in findedWords)
                {
                    if (finded.SavedWord==s)
                    {
                        finded.AddWord(s);
                        x++;
                        wordsTemp.Remove(s);
                        break;
                    }
                }
            }
            wordsAmount.text = x.ToString();
        }
    }

    public void AddLetter(char letter)
    {
        audioManager.Play("Letter");
        currentWord += letter;
        inputZone.text = currentWord;
    }

    public void RemoveWord()
    {
        if (PlayerPrefs.GetInt("Pause", 0) == 1)
            return;
        currentWord = "";
        inputZone.text = currentWord;
        foreach (LettersController letter in letters)
        {
            letter.gameObject.SetActive(true);
        }
    }

    public void CheckWord()
    {
        if (PlayerPrefs.GetInt("Pause", 0) == 1)
            return;
        if (wordsTemp.Contains(currentWord))
        {

            audioManager.Play("AddedWord");
            wordsTemp.Remove(currentWord);
            foreach (FindedWord finded in findedWords)
            {
                if (finded.SavedWord == currentWord)
                {
                    finded.AddWord(currentWord);
                    #region Parameters
                    int findedWordsAmount = PlayerPrefs.GetInt("Words" + lvlKey);
                    int tillTip = PlayerPrefs.GetInt("TillHint", 0);
                    string toSave = PlayerPrefs.GetString("Save" + lvlKey);
                    toSave += (currentWord + ",");
                    findedWordsAmount++;
                    PlayerPrefs.SetString("Save" + lvlKey, toSave);
                    PlayerPrefs.SetInt("Words" + lvlKey, findedWordsAmount);
                    PlayerPrefs.SetInt("FindedWords", PlayerPrefs.GetInt("FindedWords", 0) + 1);
                    tillTip++;
                    if (tillTip ==5)
                    {
                        tillTip = 0;
                        Instantiate(textPrefab, forHints.transform);
                        PlayerPrefs.SetInt("Hints", PlayerPrefs.GetInt("Hints") + 1);
                    }
                    PlayerPrefs.SetInt("TillHint", tillTip);
                    #endregion
                    x++;
                    wordsAmount.text = x.ToString();
                    Instantiate(textPrefab, plaseForScore.transform);
                    if (findedWordsAmount == words.Count+1)
                        Win();
                    break;
                }
            }
        }
        else if (words.Contains(currentWord))
        {
            ActivateMessage();
        }
        else if (currentWord.Length>1)
        {
            NoWordsZone();
        }
        RemoveWord();

    }

    #region Windows

    

    public void ActivateMessage()
    {
        int pause = PlayerPrefs.GetInt("Pause", 0);
        if (pause == 1 && !wordIsFinded.activeInHierarchy)
            return;
        PlayerPrefs.SetInt("Pause", Mathf.Abs(pause-1));
        audioManager.Play("OpenWindow");
        wordIsFinded.SetActive(!wordIsFinded.activeInHierarchy);
    }
    public void NoWordsZone()
    {
        if(noWordsZone.activeInHierarchy)
        {
            noWordsZone.SetActive(false);
            PlayerPrefs.SetInt("Pause",1);
        }
        else
        {
            if (PlayerPrefs.GetInt("Pause", 0) == 1 && !wordIsFinded.activeInHierarchy)
                return;
            audioManager.Play("NoWord");
            PlayerPrefs.SetInt("Pause", 1);
            noWordsZone.SetActive(true);
            noWordsZone.GetComponent<NoWordController>().TakeWord(currentWord);

        }
    }
    void Win()
    {
        PlayerPrefs.SetInt("Pause", 1);
        audioManager.Play("Win");
        winWindow.SetActive(true);
        PlayerPrefs.SetInt("FinishedLvls", PlayerPrefs.GetInt("FinishedLvls", 0) + 1);
    }
    public void CloseWinWindow()
    {
        winWindow.SetActive(false);
        PlayerPrefs.SetInt("Pause", 0);
    }
    #endregion
}
