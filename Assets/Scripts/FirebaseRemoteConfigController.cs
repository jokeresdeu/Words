using Firebase;
using Firebase.RemoteConfig;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;
using Firebase.InstanceId;
using TMPro;
using UnityEngine.UI;
public class FirebaseRemoteConfigController
{
    [SerializeField] TMP_Text _text;
    [SerializeField] Button _button;
    private bool _isInitialized;
    private bool _configsReceived;
    Dictionary<string, object> _defaults = new Dictionary<string, object>();

    public event Action ConfigsReceived = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        if (_isInitialized)
            return;

        _isInitialized = true;
        _defaults.Add("Font", "TimesNewRoman");
        _defaults.Add("ThemeColor", "Black");
        _defaults.Add("ButtonOrded", "Default");
        _defaults.Add("WelkomeText", "Hello World");
        FirebaseRemoteConfig remoteConfig = FirebaseRemoteConfig.GetInstance(FirebaseApp.DefaultInstance);

        remoteConfig.SetConfigSettingsAsync(new ConfigSettings() { IsDeveloperMode = true });
        remoteConfig.SetDefaultsAsync(_defaults);

       // Firebase.Installations.FirebaseInstallations.DefaultInstance.GetTokenAsync().ContinueWith(
       //  task => {
       //if (!(task.IsCanceled || task.IsFaulted) && task.IsCompleted)
       //{
       //    UnityEngine.Debug.Log(System.String.Format("Installations token {0}", task.Result));
       //}
       // });

        FirebaseInstanceId.DefaultInstance.GetTokenAsync().ContinueWith(
            task =>
            {
                if (!(task.IsCanceled || task.IsFaulted) && task.IsCompleted)
                {

                        Debug.Log(String.Format("Instance ID Token {0}", task.Result));
                }
            });

        LoadConfig();
        _button.onClick.AddListener(GetConfig);
    }

    private void LoadConfig()
    {
       Task fetchTask = FirebaseRemoteConfig.GetInstance(FirebaseApp.DefaultInstance).FetchAsync(TimeSpan.FromSeconds(0f));

        fetchTask.ContinueWith(task =>
        {
            if(!(task.IsCanceled || task.IsFaulted) && task.IsCompleted)
            {
                _configsReceived = true;
                ConfigsReceived();
                Debug.LogError("Here");
                FirebaseRemoteConfig.GetInstance(FirebaseApp.DefaultInstance).FetchAndActivateAsync();
            }
        });
    }

    public bool TryGetConfig(string key, out ConfigValue value)
    {
        value = FirebaseRemoteConfig.GetInstance(FirebaseApp.DefaultInstance).GetValue(key);
        return _configsReceived;
    }

    private void GetConfig()
    {
        TryGetConfig("Font", out ConfigValue value);
        _text.text = value.StringValue;
    }
}
