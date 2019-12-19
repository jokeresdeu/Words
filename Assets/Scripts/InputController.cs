using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputController : MonoBehaviour
{
    FindedWord[] findedWords;
    LettersController[] letters;
    [TextArea][SerializeField] string wordsList;

    AudioManager audioManager;
    [SerializeField] TMP_Text inputZone;
    [SerializeField] TMP_Text wordsAmount;
    [SerializeField] TMP_Text totalWordsAmount;


    [SerializeField] GameObject noWordsZone;
    [SerializeField] GameObject plaseForScore;
    [SerializeField] GameObject textPrefab;
    [SerializeField] GameObject wordIsFinded;
    [SerializeField] GameObject wordZone;
    [SerializeField] GameObject findedWordsZone;
   
    [SerializeField] string lvlKey;
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

    public void CheckWord()
    {
        if (wordsTemp.Contains(currentWord))
        {
            int findedWordsAmount = PlayerPrefs.GetInt("Words" + lvlKey);
            int tillTip =  PlayerPrefs.GetInt("TillHint",0);
            audioManager.Play("AddedWord");
            wordsTemp.Remove(currentWord);
            foreach (FindedWord finded in findedWords)
            {
                if (finded.SavedWord == currentWord)
                {
                    finded.AddWord(currentWord);
                    string toSave = PlayerPrefs.GetString("Save" + lvlKey);
                    toSave += (currentWord + ",");
                    findedWordsAmount++;
                    PlayerPrefs.SetString("Save" + lvlKey, toSave);
                    PlayerPrefs.SetInt("Words" + lvlKey, findedWordsAmount);
                    tillTip++;
                    if (tillTip ==5)
                    {
                        Debug.Log("Here");
                        tillTip = 0;
                        PlayerPrefs.SetInt("Hints", PlayerPrefs.GetInt("Hints") + 1);
                    }
                    PlayerPrefs.SetInt("TillHint", tillTip);
                    x++;
                    wordsAmount.text = x.ToString();
                    Instantiate(textPrefab, plaseForScore.transform);
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

    public void ActivateMessage()
    {
        audioManager.Play("OpenWindow");
        wordIsFinded.SetActive(!wordIsFinded.activeInHierarchy);
    }

    public void AddLetter(char letter)
    {
        audioManager.Play("Letter");
        currentWord += letter;
        inputZone.text = currentWord;
    }

    public void NoWordsZone()
    {
        audioManager.Play("NoWord");
        noWordsZone.SetActive(!noWordsZone.activeInHierarchy);
        noWordsZone.GetComponent<NoWordController>().TakeWord(currentWord);
    }

    public void RemoveWord()
    {
        currentWord = "";
        inputZone.text = currentWord;
        foreach (LettersController letter in letters)
        {
            letter.gameObject.SetActive(true);
        }
    }
}
