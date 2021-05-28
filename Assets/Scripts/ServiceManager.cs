using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServiceManager: MonoBehaviour
{
    private int _currentDataVersion = 1;

    public event Action<List<string>> DataForStartLvlPrepared = delegate(List<string> list) { };

    public event Action DataForGameLvlPrepared = delegate { };

    public bool ServicesReady { get; private set; }

    public FireBaseServicesLocator FireBaseServices { get; private set; }
    public ConnectionController ConnectionController { get; private set; }
    public LvlDataController LvlDataController { get; private set; }

    public static ServiceManager Instanse;
    private void Awake()
    {
        if (Instanse == null)
            Instanse = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        ConnectionController = new ConnectionController();

        StartCoroutine(ConnectionController.CheckInternetConnetcion());
    }

    private void Start()
    {
        InitCompomemts();
    }

    private async Task InitCompomemts()
    {
        FireBaseServices = new FireBaseServicesLocator(this);
        await FireBaseServices.InitServices();

        LvlDataController = new LvlDataController(this);
        Debug.LogError("WTF");
        Debug.LogError(DataForStartLvlPrepared);

        DataForStartLvlPrepared?.Invoke(FireBaseServices.DataBase.MainWords);
    }

    public async Task PrepareLvlData(string lvlWord)
    {
        Debug.LogError("Preparing lvl Data");
        await LvlDataController.LoadLvl(lvlWord);
        //Hide load panel
        DataForGameLvlPrepared?.Invoke();
        StartCoroutine(SendDataToFireBase());
    }

    private IEnumerator SendDataToFireBase()
    {
        while(true)
        {
            yield return new WaitUntil(() => LvlDataController.SaveData().IsCompleted);
            yield return new WaitForSeconds(10f);
        }
    }

    private void OnMainMenuLoaded()
    {
        StopCoroutine(SendDataToFireBase()); 
    }
}

