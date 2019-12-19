using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Words", menuName = "Words")]
public class Words : ScriptableObject
{
    [TextArea] public string words;
    [TextArea] public string hints;
}
