using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class WordsSender : MonoBehaviour
{
  
    const string baseURL  = "https://docs.google.com/forms/d/e/1FAIpQLSdnfENxLudkmHHJOpswxuLAbn28UNEMtzW7mMn8di-5YlUS5g/";
    const string entryID1 = "entry.1984918614";
    const string entryID2 = "entry.1113634166";
    const string entryID3 = "entry.932622147";

    #region Singleton
    public static WordsSender instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }
    #endregion
    // Start is called before the first frame update
  

    public void AddWord(string word, int lvl)
    {
        StartCoroutine(SendToForms(word, lvl));
    }
  
    static IEnumerator SendToForms(string word, int lvl)
    {
        Debug.Log("here");
        WWWForm form = new WWWForm();
        switch (lvl)
            {
            case 1:
                form.AddField(entryID1, word);
                break;
            case 2:
                form.AddField(entryID2, word);
                break;
            case 3:
                form.AddField(entryID3, word);
                break;
            default:
                break;
        }
        string responceForm = baseURL+"formResponse";
        using (UnityWebRequest www = UnityWebRequest.Post(responceForm, form))
        {
            yield return www.SendWebRequest();
        }
    }
}
