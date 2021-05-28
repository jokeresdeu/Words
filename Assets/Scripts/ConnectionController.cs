using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ConnectionController
{
    private bool _firstCheck = true;
    
    private bool _hasInternetConnection;
    public bool FirstCheck => _firstCheck;

    public bool HasInternetConnection
    {
        get
        {
            return _hasInternetConnection;
        }
        private set
        {
            if (value != _hasInternetConnection)
            {
                _hasInternetConnection = value;
                InternetConnectionChanged(_hasInternetConnection);
            }
               
        }
    }

    public Action<bool> InternetConnectionChanged = delegate { };

    public IEnumerator CheckInternetConnetcion()
    {
        while (Application.isPlaying)
        {
            using (var webClient = new UnityWebRequest(Constants.CheckInternetConnectionUrl) { method = UnityWebRequest.kHttpVerbGET })
            {
                yield return webClient.SendWebRequest();
                if (_firstCheck)
                    _firstCheck = false;
                HasInternetConnection = webClient.error == null;
            }
            yield return new WaitForSeconds(15);
        }
    }
}
