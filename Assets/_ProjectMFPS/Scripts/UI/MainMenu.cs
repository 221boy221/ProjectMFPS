using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIPanel
{

    #region Vars
    [SerializeField] private Button _connectButton;
    [SerializeField] private Button _helpButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    [SerializeField] private LoginPanel _loginPanel;
    [SerializeField] private ServerList _serverList;

    #endregion

    #region Methods
    private void OnEnable()
    {
        _connectButton.onClick.AddListener(OnClickedConnect);
        _helpButton.onClick.AddListener(OnClickedHelp);
        _settingsButton.onClick.AddListener(OnClickedSettings);
        _quitButton.onClick.AddListener(OnClickedQuit);
    }

    private void OnDisable()
    {
        _connectButton.onClick.RemoveAllListeners();
        _helpButton.onClick.RemoveAllListeners();
        _settingsButton.onClick.RemoveAllListeners();
        _quitButton.onClick.RemoveAllListeners();

        _serverList.gameObject.SetActive(false);
    }


    #region Login Screen
    private void OnClickedConnect() {
        // Open login panel
        _loginPanel.OnClickedLogin += OnClickedLogin;
        _loginPanel.OnClose += OnClosedLogin;
        _loginPanel.gameObject.SetActive(true);
    }

    private void OnClosedLogin()
    {
        _loginPanel.OnClose -= OnClosedLogin;
        _loginPanel.OnClickedLogin -= OnClickedLogin;
    }

    private void OnClickedLogin(string username, string password)
    {
        if (NetworkManager.Instance.AttemptLogin(username, password) != 0)
        {
            Debug.Log("Error logging in");
            // Todo: trigger error message
        }
        else
        {
            Debug.Log("Sucessfully logged in");
            OnLoggedIn();
        }

        _loginPanel.gameObject.SetActive(false);
    }

    private void OnLoggedIn()
    {
        _serverList.gameObject.SetActive(true);
        // on login:
        ServerManager.Instance.OnLoggedIn();
    }
    #endregion


    private void OnClickedHelp() {
        // Open help panel
    }

    private void OnClickedSettings() {
        // Open settings panel
    }

    private void OnClickedQuit() {
        // Show popup with text -> Are you sure you want to Quit?  ->  Yes / No
        // Behaviour:
        Application.Quit();
    }
    
    #endregion

}
