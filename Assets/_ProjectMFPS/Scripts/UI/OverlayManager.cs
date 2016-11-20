using UnityEngine;
    
public class OverlayManager : MonoBehaviour {

    public static OverlayManager Instance;

    // todo: replacewith overlayData that contains OnEnable and OnDisable that handles the overlay's components etc (and bool for OnDemand loading/destroying)
    //[SerializeField] private GameObject[] _overlays;
    [SerializeField] private BasePopup _loginPopup;
    [SerializeField] private GameObject _serverBrowser;    


    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        //
        UIManager.SwitchUIPanel += OnUiSwitchState;
    }

    private void OnUiSwitchState(ScreenPanel panel) {
        Debug.Log("On Switch UI Panel");
        // Add EventListener
        if (panel == ScreenPanel.MainMenu)
        {
            MenuPanel.OpenLogin += OpenLoginScreen; // Todo: dont forget to remove
        }
    }

    internal void OpenLoginScreen() {
        if (_loginPopup == null) _loginPopup = PopupFactory.Instance.CreatePopup(PopupType.MultipleChoice);
        if (_loginPopup.gameObject.activeSelf == false) _loginPopup.gameObject.SetActive(true);
        
        _loginPopup.SetButtonAction(1, () => OnLoggedIn()); // TODO: Replace this with an actual login popup that has its own prefab and is connected to some LoginVerification
    }
    
    private void OnLoggedIn()
    {
        OpenServerBrowser();
        ServerManager.Instance.OnLoggedIn();
    }
    
    internal void OpenServerBrowser() {
        _serverBrowser.SetActive(true);
    }

}
