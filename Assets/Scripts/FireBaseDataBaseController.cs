using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class FireBaseDataBaseController
{
    private const string Users = "Users";
    private const string FirebaseLvlDatasChild = "LvlDatas";

    private const string FirebaseBannedWords = "BannedWords";

    private const string FirebaseProposedWords = "ProposedWords";

    private const string FirebaseMainWords = "MainWords";

    private const string GameData = "GameData";


    private FireBaseServicesLocator _fireBaseServices;
    private DatabaseReference _wordsDataBase;

    private string _userId;

    public List<string> PropposedWords { get; private set; }

    public List<string> MainWords { get; private set; }
    public FireBaseData FireBaseData { get; private set; }
    public List<string> WordsToPropose { get; private set; }
    public LvlData BannedWords { get; private set; }
    public Dictionary<string, List<string>> UserData { get; private set; }


    public FireBaseDataBaseController(ServiceManager serviceManager, FireBaseServicesLocator fireBaseServices)
    {
        _wordsDataBase = FirebaseDatabase.DefaultInstance.RootReference;

        _fireBaseServices = fireBaseServices;

#if UNITY_EDITOR
        _userId = "Admin";
#else
        _userId = fireBaseServices.FirebaseAuthenticator.FirebaseUser.UserId;
#endif
    }

    public async Task GetStartData()
    {

        try
        {
            DataSnapshot mainWordsSnapshot = await _wordsDataBase.Child(GameData).Child(FirebaseMainWords).GetValueAsync();

            MainWords = JsonConvert.DeserializeObject<List<string>>(mainWordsSnapshot.GetRawJsonValue());
        }
        catch
        {
            Debug.LogError("Something wrong with database");
        }

        DataSnapshot userDataSnapshot = await _wordsDataBase.Child(Users).Child(_userId).GetValueAsync();

        if (!userDataSnapshot.HasChildren)
            return;

        try
        {
            UserData = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(userDataSnapshot.GetRawJsonValue());
            if (PlayerPrefsManager.FirstEnter.Get() && UserData != null)
            {
                Debug.LogError("Restoring data");
            }
        }
        catch
        {
            Debug.LogError("something wrong with user data");
        }
        
    }

    public async Task<LvlData> GetLvlData(string mainWord)
    {
        if (BannedWords == null)
        {
            await GetBannedWords();
        }
        Debug.LogError("Banned words");
        if (PropposedWords == null)
        {
            await GetPropposedWords();

            //_wordsDataBase.Child(GameData).Child(FirebaseProposedWords).ChildAdded += HandlePropposedAdded;
        }
        Debug.LogError("Proposed words");
        return await GetLvlDataFromFirebase(mainWord);
    }

    public async Task ProposeWords()
    {
        for(int i =0; i<WordsToPropose.Count; i++)
        {
            if (!PropposedWords.Contains(WordsToPropose[i]))
                PropposedWords.Add(WordsToPropose[i]);
        }
        WordsToPropose.Clear();
        await _wordsDataBase.Child(GameData).Child(FirebaseProposedWords).SetRawJsonValueAsync(JsonConvert.SerializeObject(PropposedWords));
    }

    public async Task SaveUserData(List<string> findedWords, string mainWord)
    {
        await _wordsDataBase.Child(Users).Child(_userId).Child(mainWord).SetRawJsonValueAsync(JsonConvert.SerializeObject(findedWords));
    }

    public BaseWordData IfNotBannedSend(string word)
    {
        BaseWordData bannedWord = BannedWords.Words.Find(banned => banned.Word.Equals(word));
        if (bannedWord == null && !PropposedWords.Contains(word))
            WordsToPropose.Add(word);
        return bannedWord;
    }

#region GettingData
    private async Task<LvlData> GetLvlDataFromFirebase(string mainWord)
    {
        try
        {
            DataSnapshot lvlDataSnapshot = await _wordsDataBase.Child(GameData).Child(FirebaseLvlDatasChild).Child(mainWord).GetValueAsync();
            Debug.LogError("Lvl data prepared");
            return JsonConvert.DeserializeObject<LvlData>(lvlDataSnapshot.GetRawJsonValue());
        }
        catch
        {
            Debug.LogError("Cant take lvlData from firebase");
            return new LvlData();
        }
    }

    private async Task GetBannedWords()
    {
        try
        {
            DataSnapshot lvlDataSnapshot = await _wordsDataBase.Child(GameData).Child(FirebaseBannedWords).GetValueAsync();

            BannedWords = JsonConvert.DeserializeObject<LvlData>(lvlDataSnapshot.GetRawJsonValue());
        }
        catch
        {
            Debug.LogError("Problebs with banned words");
            BannedWords = new LvlData();
        }
        
    }

    public async Task GetPropposedWords()
    {
        try
        {
            DataSnapshot lvlDataSnapshot = await _wordsDataBase.Child(GameData).Child(FirebaseProposedWords).GetValueAsync();

            PropposedWords = JsonConvert.DeserializeObject<List<string>>(lvlDataSnapshot.GetRawJsonValue());
        }
        catch
        {
            Debug.LogError("Problesbs with proposed words");
            PropposedWords = new List<string>();
        }

    }

    private void HandlePropposedAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        if (args.PreviousChildName == FirebaseBannedWords)
        {
            BannedWords = JsonConvert.DeserializeObject<LvlData>(args.Snapshot.GetRawJsonValue());
            return;
        }
    }
#endregion
}
