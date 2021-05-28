using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class GameLvlPanel : MonoBehaviour
{
    private const string ProposedWordDescription = "Запропоноване вами слово перебуває на модерації у розробників."; 

    [Header("Letters")]
    [SerializeField] private Transform[] _lettersHolders;
    [SerializeField] private LetterButton _letterPrefab;
    
    [Header("Words")]
    [SerializeField] private Transform _hiddenWordsHolder;
    [SerializeField] private Transform _proposedWordsHolder;
    [SerializeField] private Transform _bannedWordsHolder;
    [SerializeField] private TextButton _hiddenWordPrefab;
   

    [Header("Main word")]
    [SerializeField] private TMP_Text _inputWord;
    [SerializeField] private Button _deleteWord;
    [SerializeField] private Button _deleteLastLetter;
    [SerializeField] private Button _checkWord;

    private string _currentWord;
    private ServiceManager _serviceManager; 
   
    private Dictionary<WordData, TextButton> _hiddenWords = new Dictionary<WordData, TextButton>();
    private List<GameObject> _hiddenLetters = new List<GameObject>();

    public void InitPlayScene()
    {
        Debug.LogError("Initing everething");
        _serviceManager = ServiceManager.Instanse;
        _inputWord.text = string.Empty;

        try
        {
            char[] letters = _serviceManager.LvlDataController.CurrentLvlData.MainWord.ToCharArray();
            int counter = 0;
            for (int i = 0; i < letters.Length; i++)
            {
                if (letters[i] == '-')
                {
                    counter++;
                    continue;
                }

                LetterButton letterButton = Instantiate(_letterPrefab, _lettersHolders[counter]).GetComponentInChildren<LetterButton>();
                letterButton.SetLetter(letters[i]);
                letterButton.Button.onClick.AddListener(() => AddLetterToWord(letterButton));
            }
        }
        catch
        {
            Debug.LogError("LettersFucked");
        }
        

        List<string> approvedWord = new List<string>();

        try
        {
            for (int i = 0; i < _serviceManager.LvlDataController.CurrentLvlData.LvlWords.Count; i++)
            {
                if (_serviceManager.LvlDataController.CurrentLvlData.LvlWords[i].WordStatus == WordStatus.ApprovedWord)
                {
                    approvedWord.Add(_serviceManager.LvlDataController.CurrentLvlData.LvlWords[i].Word);
                    _serviceManager.LvlDataController.CurrentLvlData.LvlWords[i].WordStatus = WordStatus.FindedWord;
                }
                CreateAndInitWordHolder(_serviceManager.LvlDataController.CurrentLvlData.LvlWords[i], _hiddenWordsHolder);
            }
        }
        catch
        {
            Debug.LogError("Hidden words fucked");
        }
     
       
        try
        {
            for (int i = 0; i < _serviceManager.LvlDataController.CurrentLvlData.ProposedWords.Count; i++)
            {
                CreateAndInitWordHolder(new WordData(_serviceManager.LvlDataController.CurrentLvlData.ProposedWords[i], ProposedWordDescription, WordStatus.Default), _proposedWordsHolder);
            }
        }
        catch
        {
            Debug.LogError("Propposed words fucked");
        }
       
        try
        {
            for (int i = 0; i < _serviceManager.LvlDataController.CurrentLvlData.BannedWords.Count; i++)
            {
                WordData wordData = new WordData(_serviceManager.LvlDataController.CurrentLvlData.BannedWords[i].Word, _serviceManager.LvlDataController.CurrentLvlData.BannedWords[i].Description, WordStatus.Default);
                CreateAndInitWordHolder(wordData, _bannedWordsHolder);
            }
        }
        catch
        {
            Debug.LogError("BannedWords fucked");
        }
        
        try
        {
            if (approvedWord.Count != 0)
            {
                //Show ui with approwred words
            }

            _checkWord.onClick.AddListener(CheckWord);
            _deleteLastLetter.onClick.AddListener(DeleteLastLetter);
            _deleteWord.onClick.AddListener(DeleteWord);
        }
        catch
        {
            Debug.LogError("Shit");
        }
       
    }

    private void CreateAndInitWordHolder(WordData wordData, Transform parrent)
    {
        string text = wordData.Word;
        TextButton wordButton = Instantiate(_hiddenWordPrefab, parrent);

        if(wordData.WordStatus == WordStatus.HiddenWord)
        {
            text = string.Empty;
            for (int j = 0; j < wordData.Word.Length; j++)
            {
                text += "-";
            }
            _hiddenWords.Add(wordData, wordButton);
        }

        wordButton.Text.text = text;
        wordButton.Button.onClick.AddListener(() => ShowDescription(wordData));
    }

    private void CheckWord()
    {
        string current = _currentWord;
        DeleteWord();
        WordData wordData = _serviceManager.LvlDataController.CurrentLvlData.LvlWords.Find(word => word.Word == current);

        if (wordData != null && wordData.WordStatus == WordStatus.HiddenWord)
        {
            _hiddenWords[wordData].Text.text = wordData.Word;
            //add count/points
            return;
        }
        else
        {
            BaseWordData word = _serviceManager.FireBaseServices.DataBase.IfNotBannedSend(current);

            if (word == null)
            {
                wordData = new WordData(current, ProposedWordDescription, WordStatus.Default);
                //SendNewWord 
                CreateAndInitWordHolder(wordData, _proposedWordsHolder);
                return;
            }
        }
    }

    private void AddLetterToWord(LetterButton letter)
    {
        _inputWord.text += letter.Letter.ToString();
        _currentWord += letter.Letter.ToString();
        letter.Button.gameObject.SetActive(false);
        _hiddenLetters.Add(letter.Button.gameObject);
    }

    private void DeleteLastLetter()
    {
        if (_hiddenLetters.Count <= 0)
            return;

        string newText = string.Empty;
        for (int i =0; i< _inputWord.text.Length -1; i++)
        {
            newText += _inputWord.text[i];
        }
        _inputWord.text = newText;
        GameObject gameObject = _hiddenLetters[_hiddenLetters.Count-1];
        gameObject.SetActive(true);
        _hiddenLetters.Remove(gameObject);
    }

    private void DeleteWord()
    {
        _inputWord.text = string.Empty;
        foreach (GameObject gameObject in _hiddenLetters)
            gameObject.SetActive(true);
        _hiddenLetters.Clear();
    }

    private void ShowDescription(WordData word)
    {
        
        if(word.WordStatus == WordStatus.HiddenWord) //&& notEnoughCoins
        {
            Debug.LogError("Ваша підказка " + word.Description);
        }
        else //if enough coins show
        {
            Debug.LogError(word.Description);
        }
    }

    private void OnDestroy()
    {
        _checkWord.onClick.RemoveAllListeners();
        _deleteLastLetter.onClick.RemoveAllListeners();
        _deleteWord.onClick.RemoveAllListeners();
        _hiddenLetters.Clear();
        _hiddenWords.Clear();
    }
}
