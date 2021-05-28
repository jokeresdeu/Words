using Firebase.Database;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;


public class GameDatacreator : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    char[] _lineSpliter = new char[] { '\n' };
    char[] _pairSpliter = new char[] { '\t' };
    private GameData _gameData;

    string _newLvlsPath;
    string _lvlDataPath;
    string _bannedWordsPath;
    string _lvlWordsPath;

    private void Start()
    {
        _gameData = new GameData();
        _newLvlsPath = Path.Combine(Application.dataPath, "NewLvls");
        _lvlDataPath = Path.Combine(Application.dataPath, "GameData/LvlData");
        _bannedWordsPath = Path.Combine(Application.dataPath, "GameData/BannedWords.json");
        _lvlWordsPath = Path.Combine(Application.dataPath, "GameData/LvlWords.json");
    }

    public void SaveNewLvlsToJson()
    {
        string[] files = Directory.GetFiles(_newLvlsPath, "*.txt");
        List<LvlData> list = new List<LvlData>();
        for (int i = 0; i < files.Length; i++)
        {
            string file = File.ReadAllText(files[i], Encoding.UTF8);
            Debug.LogError(file);
            string[] lines = file.Split(_lineSpliter);
            LvlData lvlData = new LvlData();
            Debug.LogError(lines.Length);
            for (int j = 0; j < lines.Length; j++)
            {
                string[] pair = lines[j].Split(_pairSpliter);
                if (pair[0] == string.Empty || pair.Length < 2)
                    continue;
                pair[1] = pair[1].Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                BaseWordData baseWord = new BaseWordData(pair[0], pair[1]);
                lvlData.Words.Add(baseWord);
            }
            string name = Path.GetFileNameWithoutExtension(files[i]);
            string json = JsonConvert.SerializeObject(lvlData, Formatting.Indented);
            File.WriteAllText(_lvlDataPath + "/" + name + ".json", json, Encoding.UTF8);    
        }
       
        foreach (string file in Directory.GetFiles(_newLvlsPath))
        {
            File.Delete(file);
        }
    }

    public void SendDataToDatabase()
    {
        DatabaseReference wordsDataBase = FirebaseDatabase.DefaultInstance.RootReference;

        GameData gameData = new GameData();

        gameData.MainWords = JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(_lvlWordsPath));

        gameData.BannedWords = JsonConvert.DeserializeObject<LvlData>(File.ReadAllText(_bannedWordsPath));

        string[] files = Directory.GetFiles(_lvlDataPath, "*.json");

        for(int i = 0; i < files.Length; i++)
        {

            string name = Path.GetFileNameWithoutExtension(files[i]);

            gameData.LvlDatas.Add(name, JsonConvert.DeserializeObject<LvlData>(File.ReadAllText(files[i])));
        }
        string json = JsonConvert.SerializeObject(gameData, Formatting.Indented);
        
        wordsDataBase.Child("GameData").SetRawJsonValueAsync(json).ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
                _text.text = "Fuck";
            else
            {
                _text.text = "Data pushed to Database";
            }
        });
    }
}
