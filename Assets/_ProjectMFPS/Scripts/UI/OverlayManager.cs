using UnityEngine;
    
public class OverlayManager : MonoBehaviour {

    public static OverlayManager Instance;

    // todo: replacewith overlayData that contains OnEnable and OnDisable that handles the overlay's components etc (and bool for OnDemand loading/destroying)
    //[SerializeField] private GameObject[] _overlays;
    [SerializeField] private GameObject _loginPopup;
    [SerializeField] private GameObject _serverBrowser;    


    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        //
        UIManager.SwitchState += OnUiSwitchState;
    }

    private void OnUiSwitchState() {
        // Add EventListener
        MenuPanel.OpenLogin += OpenLoginScreen;
    }

    internal void OpenLoginScreen() {
        _loginPopup.SetActive(true);
        // todo: add onClick event from login popup to call Connect
    }
    
    internal void OpenServerBrowser() {
        _serverBrowser.SetActive(true);
    }

}
