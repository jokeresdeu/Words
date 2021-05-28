using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextButton : SoundButton
{
    [SerializeField] protected TMP_Text _text;

    public TMP_Text Text => _text;
}
