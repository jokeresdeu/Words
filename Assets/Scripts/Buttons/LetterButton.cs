using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterButton : TextButton
{
    public char Letter { get; private set; }

    public void SetLetter(char letter)
    {
        Letter = letter;
        Text.text = letter.ToString();
    }
}
