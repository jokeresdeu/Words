using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIContainer : MonoBehaviour
{
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private ServiceManager _serviceManager;
    [SerializeField] private GameLvlPanel _gameLvlPanel;
    [SerializeField] private MainMenuPanel _mainMenuPanel;
    [SerializeField] private LoadScreen _loadScreen;
    [SerializeField] private TopGroupButtons _topGropButtons;

    public GameLvlPanel GameLvlPanel => _gameLvlPanel;
    public MainMenuPanel MainMenuPanel => _mainMenuPanel;
    public LoadScreen LoadScreen => _loadScreen;
    public TopGroupButtons TopGroupButtons => _topGropButtons;

    private void Awake()
    {
        _loadScreen.gameObject.SetActive(true);
        Debug.LogError("Subscribed");
        Debug.LogError(_serviceManager);
        _serviceManager.DataForStartLvlPrepared += OnStartLvlPrepared;
    }

    private void OnStartLvlPrepared(List<string> mainWords)
    {

        _mainMenuPanel.gameObject.SetActive(true);

        _mainMenuPanel.Init(this);

        _serviceManager.DataForStartLvlPrepared -= OnStartLvlPrepared;

        _loadScreen.gameObject.SetActive(false);
    }

    public void OnLvlButtonClicked(string lvlWord)
    {
        _loadScreen.gameObject.SetActive(true);
        _mainMenuPanel.gameObject.SetActive(false);
        _serviceManager.DataForGameLvlPrepared += OnGameLvlPrepared;
        _serviceManager.PrepareLvlData(lvlWord);

    }

    private void OnGameLvlPrepared()
    {
        _gameLvlPanel.gameObject.SetActive(true);
        _gameLvlPanel.InitPlayScene();
        _loadScreen.gameObject.SetActive(false);
        _serviceManager.DataForGameLvlPrepared -= OnGameLvlPrepared;
    }


}
