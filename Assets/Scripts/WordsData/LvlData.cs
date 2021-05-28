using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Newtonsoft.Json;

[Serializable]
public class BaseWordData
{
    public string Word { get; protected set; }
    public string Description { get; protected set; }

    [JsonConstructor]
    public BaseWordData(string word, string description)
    {
        Word = word;
        Description = description;
    }

    public override bool Equals(object obj)
    {
        BaseWordData data = obj as BaseWordData;
        if (data != null)
            return Word.Equals(data.Word);
        string s = obj.ToString();
        return obj != null && s.Equals(Word);
    }
}

[Serializable]
public class WordData : BaseWordData
{
    public WordStatus WordStatus { get; set; }

    [JsonConstructor]
    public WordData(string word, string description, WordStatus wordStatus) : base(word, description)
    {
        WordStatus = wordStatus;
    }
}

[Serializable]
public class LvlData
{
    public int LvlVersion;

    public List<BaseWordData> Words;

    public LvlData()
    {
        LvlVersion = 1;
        Words = new List<BaseWordData>();
    }
}


[Serializable]
public class FireBaseData
{
    GameData GameData;

    public List<string> ProposedWords;

    Dictionary<string, Dictionary<string, List<string>>> Users;
}

[Serializable]
public class GameData
{
    public List<string> MainWords;

    public Dictionary<string, LvlData> LvlDatas;

    public LvlData BannedWords;

    public GameData()
    {
        MainWords = new List<string>();
        LvlDatas = new Dictionary<string, LvlData>();
        BannedWords = new LvlData();
    }
}

[Serializable]
public class GameLvlData
{
    public int LvlVersion; 
    public string MainWord { get; private set; }
    public List<WordData> LvlWords { get; private set; }
    public List<string> ProposedWords { get; private set; }
    public List<BaseWordData> BannedWords { get; private set; }

    [JsonConstructor]
    public GameLvlData(int lvlVersion, string mainWord, List<WordData> hiddenWords, List<string> propsedWords, List<BaseWordData> bannedWords)
    {
        LvlVersion = lvlVersion;
        MainWord = mainWord;
        LvlWords = hiddenWords;
        ProposedWords = propsedWords;
        BannedWords = bannedWords;
    }

    public GameLvlData(LvlData lvlData, string mainWord)
    {
        MainWord = mainWord;
        LvlVersion = lvlData.LvlVersion;
        LvlWords = new List<WordData>();
        for(int i = 0; i < lvlData.Words.Count; i++)
        {
            WordData data = new WordData(lvlData.Words[i].Word, lvlData.Words[i].Description, WordStatus.HiddenWord);
            data.WordStatus = WordStatus.HiddenWord;
            LvlWords.Add(data); 
        }
        ProposedWords = new List<string>();
        BannedWords = new List<BaseWordData>();
    }
}


public enum WordStatus
{
    Default,
    HiddenWord,
    FindedWord,
    ApprovedWord,
}



