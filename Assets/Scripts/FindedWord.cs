using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FindedWord : MonoBehaviour
{
    string savedWord;
    public string SavedWord { get { return savedWord; } }
    bool isBusy;
    public bool IsBusy { get { return isBusy; } }
    TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TMP_Text>();
    }
    public void SetWord(string word)
    {
        savedWord = word;
        string tempText = "";
        int x = savedWord.Length;
        for (int i = 0; i < x; i++)
            tempText += "-";
        text.text = tempText;
    }
    public void AddWord(string word)
    {
        text.text = savedWord;
        isBusy = true;
    }
}
