using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    [SerializeField]protected Button _button;
    public Button Button => _button;

    private void Start()
    {
        //Add sound 
    }

    private void OnDestroy()
    {
        Button.onClick.RemoveAllListeners();
    }
}
