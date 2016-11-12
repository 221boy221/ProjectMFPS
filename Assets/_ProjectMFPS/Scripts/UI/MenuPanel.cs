using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuPanel : UIPanel {

    //todo: get rid of statics
    public static UnityAction OpenLogin;
    public static UnityAction OpenTraining;
    public static UnityAction OpenItemShop;

    [SerializeField] private GameObject _connect;
    [SerializeField] private GameObject _training;
    [SerializeField] private GameObject _itemShop;

    internal override void InitReferences() {
        _connect.GetComponent<Button>().onClick.AddListener(OpenLogin);
        _training.GetComponent<Button>().onClick.AddListener(OpenTraining);
        _itemShop.GetComponent<Button>().onClick.AddListener(OpenItemShop);
    }

    internal override void UpdateLayout() {
        ScreenPanel uiState = UIManager.UiState;

        if (uiState == ScreenPanel.MainMenu) {
            //off
            _training.SetActive(false);
            _itemShop.SetActive(false);
            //on
            _connect.SetActive(true);
        }
        else if (uiState == ScreenPanel.Lobby) {
            //off
            _connect.SetActive(false);
            //on
            _training.SetActive(true);
            _itemShop.SetActive(true);
        }
        else {
            //off
            _connect.SetActive(false);
            _training.SetActive(false);
            _itemShop.SetActive(false);
        }
    }

}
