using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MenuPanel : UIPanel {

    //todo: get rid of statics
    public static UnityAction OpenLogin = delegate { };
    public static UnityAction OpenTraining = delegate { };
    public static UnityAction OpenItemShop = delegate { };

    [SerializeField] private GameObject _connect;
    [SerializeField] private GameObject _training;
    [SerializeField] private GameObject _itemShop;

    internal override void InitReferences() {
        // Init callbacks
        _connect.GetComponent<Button>().onClick.AddListener(() => OpenLogin());
        _training.GetComponent<Button>().onClick.AddListener(() => OpenTraining());
        _itemShop.GetComponent<Button>().onClick.AddListener(() => OpenItemShop());
    }

    internal override void OnSwitchUIPanel(ScreenPanel panel) {
        Debug.Log("On Switch UI Panel");
        if (panel == ScreenPanel.MainMenu) {
            //off
            _training.SetActive(false);
            _itemShop.SetActive(false);
            //on
            _connect.SetActive(true);
        }
        else if (panel == ScreenPanel.Lobby) {
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
