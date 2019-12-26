using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LettersController : MonoBehaviour
{
    InputController inputController;
    [SerializeField]char letter;
    public char Letter { get { return letter; } }
    TMP_Text text;

    void Start()
    {
        inputController = InputController.instance;
        text = GetComponentInChildren<TMP_Text>();

    }
    public void UseLetter()
    {
        if (PlayerPrefs.GetInt("Pause", 0) == 1)
            return;
        inputController.AddLetter(letter, this);
        gameObject.SetActive(false);
    } 
}
