using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class WordsSender : MonoBehaviour
{
  
    const string baseURL  = "https://docs.google.com/forms/d/e/1FAIpQLSdnfENxLudkmHHJOpswxuLAbn28UNEMtzW7mMn8di-5YlUS5g/";
    [SerializeField]string entry; 

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
  

    public void AddWord(string word)
    {
        StartCoroutine(SendToForms(word));
    }
  
    IEnumerator SendToForms(string word)
    {
        Debug.Log("here");
        WWWForm form = new WWWForm();
        form.AddField(entry, word);
        string responceForm = baseURL+"formResponse";
        using (UnityWebRequest www = UnityWebRequest.Post(responceForm, form))
        {
            yield return www.SendWebRequest();
        }
    }
}
