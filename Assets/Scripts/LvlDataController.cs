using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class LvlDataController
{
    private ServiceManager _serviceManager;
    public GameLvlData CurrentLvlData { get; private set; }

    public LvlDataController(ServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task<bool> LoadLvl(string lvlWord)
    {
        string localDataString = PlayerPrefsManager.GetStringPref(lvlWord);
        GameLvlData localLvlData = JsonConvert.DeserializeObject<GameLvlData>(localDataString);

        if(!_serviceManager.ConnectionController.HasInternetConnection)
        {
            if(localLvlData == null)
            {
                Debug.LogError("Fuuuuuck");
                ////Ui show need to download panel
                return false;
            }
        }
        
        LvlData serverLvlData = await _serviceManager.FireBaseServices.DataBase.GetLvlData(lvlWord);
        Debug.LogError("We have lvlData");

        if (localLvlData != null)
        {
            if (localLvlData.LvlVersion < serverLvlData.LvlVersion)
            {
                SyncDatas(serverLvlData, localLvlData);
            }
        }
        else
        {
            localLvlData = new GameLvlData(serverLvlData, lvlWord);
        }

        CurrentLvlData = localLvlData;
        return true;
    }

    private void SyncDatas(LvlData serverData, GameLvlData localData)
    {
        int counter = 0;
        for (int i = 0; i < localData.LvlWords.Count; i++)
        {
            if (localData.LvlWords[i + counter].Word == serverData.Words[i + counter].Word)
                continue;

            WordData newWord =  new WordData(serverData.Words[i].Word, serverData.Words[i].Description, WordStatus.HiddenWord);
            if (localData.ProposedWords.Contains(serverData.Words[i].Word))
            {
                localData.ProposedWords.Remove(serverData.Words[i].Word);
                PlayerPrefsManager.CoinsAmount.ChangeValue(1);
                newWord.WordStatus = WordStatus.ApprovedWord;
            }
            localData.LvlWords.Add(newWord);
            counter++;
        }

        if (localData.ProposedWords.Count == 0)
            return;

        for (int i = 0; i < localData.ProposedWords.Count; i++)
        {
            BaseWordData word = _serviceManager.FireBaseServices.DataBase.IfNotBannedSend(localData.ProposedWords[i]);
            if (word != null)
                localData.BannedWords.Add(word);
        }
    }

    public async Task SaveData()
    {
        await _serviceManager.FireBaseServices.DataBase.ProposeWords();
        List<string> findedWords = new List<string>();
        for (int i = 0; i < CurrentLvlData.LvlWords.Count; i++)
        {
            if (CurrentLvlData.LvlWords[i].WordStatus == WordStatus.FindedWord)
            {
                findedWords.Add(CurrentLvlData.LvlWords[i].Word);
            }
        }
        await _serviceManager.FireBaseServices.DataBase.SaveUserData(findedWords, CurrentLvlData.MainWord);
        PlayerPrefsManager.SaveStringPref(CurrentLvlData.MainWord, JsonConvert.SerializeObject(CurrentLvlData));
        findedWords.Clear();
    }
   
}

