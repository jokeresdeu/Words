using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LettersController : MonoBehaviour
{
    InputController inputController;
    [SerializeField]char letter;
    void Start()
    {
        inputController = InputController.instance;
    }
    public void UseLetter()
    {
        if (PlayerPrefs.GetInt("Pause", 0) == 1)
            return;
        inputController.AddLetter(letter);
        gameObject.SetActive(false);
    }
}
