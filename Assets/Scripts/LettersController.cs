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
        inputController.AddLetter(letter);
        gameObject.SetActive(false);
    }
}
