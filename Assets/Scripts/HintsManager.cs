using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintsManager : MonoBehaviour
{
    [TextArea][SerializeField] string hints;
    [SerializeField] TMP_Text hintsText;
    [SerializeField] GameObject hintsField;
    [SerializeField] TMP_Text hintsAmountText;
    
    [SerializeField] Animator animator;
    string[] tempHints;
    string[] tempWords;
    Dictionary<string, string> hintsList = new Dictionary<string, string>();

    InputController input;

    void Start()
    {
        input = InputController.instance;
        tempHints = hints.Split('!');
        for (int x = 0; x<tempHints.Length; x++)
        {
            hintsList.Add(input.Words[x], tempHints[x]);
        }
    }
    private void Update()
    {
        hintsAmountText.text = PlayerPrefs.GetInt("Hints", 0).ToString()+"x";
    }
    public void ShowHint()
    {
        if (PlayerPrefs.GetInt("Pause", 0) == 1 && hintsField.activeInHierarchy == false)
            return;
        int hintsAmount = PlayerPrefs.GetInt("Hints", 0);
        if(!hintsField.activeInHierarchy)
        {
            PlayerPrefs.SetInt("Pause", 1);
            if(hintsAmount > 0 )
            {
                hintsField.SetActive(true);
                string tempWord = input.WordsTemp[Random.Range(0, input.WordsTemp.Count)];
                hintsText.text = hintsList[tempWord];
                hintsAmount--;
                PlayerPrefs.SetInt("Hints", hintsAmount);
            }
            else
            {
                animator.SetBool("NoHints", true);
                Invoke("AnimationOff", 1f);
            }
                
        }
        else
        {
            PlayerPrefs.SetInt("Pause", 0);
            hintsField.SetActive(false);
        }
       
            
    }
    private void AnimationOff()
    {
        animator.SetBool("NoHints", false);
    }
}
