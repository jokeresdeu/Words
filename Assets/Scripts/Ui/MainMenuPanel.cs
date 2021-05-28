using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private Transform _lvlButtonsHolder;
    [SerializeField] private TextButton _lvlButtonPrefab;

    private List<TextButton> _lvlButtons = new List<TextButton>();

    public void Init(UIContainer container)
    {
        for(int i = 0; i< ServiceManager.Instanse.FireBaseServices.DataBase.MainWords.Count; i++)
        {
            Debug.LogError(i);
            TextButton textButton = Instantiate(_lvlButtonPrefab, _lvlButtonsHolder);
            string text = ServiceManager.Instanse.FireBaseServices.DataBase.MainWords[i].Replace("-", "");
            textButton.Text.text = ServiceManager.Instanse.FireBaseServices.DataBase.MainWords[i];
            int index = i;
            textButton.Button.onClick.AddListener(() => {
                Debug.LogError(ServiceManager.Instanse.FireBaseServices.DataBase.MainWords.Count);
                Debug.LogError(index);
                Debug.LogError(ServiceManager.Instanse.FireBaseServices.DataBase.MainWords[index]);
                container.OnLvlButtonClicked(ServiceManager.Instanse.FireBaseServices.DataBase.MainWords[index]);
            });
            _lvlButtons.Add(textButton);
        }
    }
}
