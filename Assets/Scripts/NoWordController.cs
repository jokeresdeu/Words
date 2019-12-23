using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoWordController : MonoBehaviour
{
    string noWord;
    [SerializeField]TMP_Text noWordText;
    // Start is called before the first frame update
    public void TakeWord(string word)
    {
        noWord = word;
        noWordText.text = "Слова ~" + noWord + "~ немає в словнику, бажаєте вiдправити його розробникам?";
    }

    public void AddWord()
    {
        //Add Word to dictionary
        PlayerPrefs.SetInt("Pause", 0);
        gameObject.SetActive(false);
    }

    public void DontAdd()
    {
        PlayerPrefs.SetInt("Pause", 0);
        gameObject.SetActive(false);
    }
}
