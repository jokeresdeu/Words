using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoWordController : MonoBehaviour
{
    string noWord;
    string text;
    WordsSender sender;
    int key;
    [SerializeField]TMP_Text noWordText;
    // Start is called before the first frame update
    private void Start()
    {
        sender = WordsSender.instance;
    }
    public void TakeWord(string word, string lvl)
    {
        text = "";
        noWord = word;
        key = int.Parse(lvl);
        noWordText.text = "Слова ~" + noWord + "~ немає в словнику, бажаєте вiдправити його розробникам?";
        text = lvl + " - " + noWord;
    }

    public void AddWord()
    {
        sender.AddWord(noWord, key);
        PlayerPrefs.SetInt("Pause", 0);
        gameObject.SetActive(false);
    }

    public void DontAdd()
    {
        PlayerPrefs.SetInt("Pause", 0);
        gameObject.SetActive(false);
    }
}
