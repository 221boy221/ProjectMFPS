using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public enum ScreenPanel {
    None = 0,
    LoadingGame,
    MainMenu,
    Lobby,
    Inventory,
    LoadingMatch
}

public class UIManager : MonoBehaviour {
    
    public static UnityAction<ScreenPanel> SwitchUIPanel = delegate { };
    
    [SerializeField] private GameObject _loadingGameScreen;
    [SerializeField] private ScreenPanel _currentScreenPanel;

    void Awake() {
        // entry
        GameManager.InitializeGame += OnStartGame;
    }

    // -------------------- //
    // -- Event Handlers -- //
    // -------------------- //
    private void OnStartGame()          { StartCoroutine(OpenPanel(ScreenPanel.LoadingGame)); }
    private void OnLoadedGame()         { StartCoroutine(OpenPanel(ScreenPanel.MainMenu)); }
    private void OnConnectedToServer()  { StartCoroutine(OpenPanel(ScreenPanel.Lobby)); }
    private void OnJoiningMatch()       { StartCoroutine(OpenPanel(ScreenPanel.LoadingMatch)); }
    private void OnOpenInventory()      { StartCoroutine(OpenPanel(ScreenPanel.Inventory)); }


    // ------------------------------- //
    // -- Enabling/Disabling panels -- //
    // ------------------------------- //
    private IEnumerator OpenPanel(ScreenPanel panel)
    {
        Debug.Log("OpenPanel:" + panel);

        switch (panel) {
            case ScreenPanel.LoadingGame:
                yield return StartCoroutine(OpenLoadingGame());
                break;
            case ScreenPanel.MainMenu:
                yield return StartCoroutine(OpenMainMenu());
                break;
            case ScreenPanel.Lobby:
                yield return StartCoroutine(OpenLobby());
                break;
            case ScreenPanel.LoadingMatch:
                yield return StartCoroutine(OpenLoadingMatch());
                break;
            case ScreenPanel.Inventory:
                yield return StartCoroutine(OpenInventory());
                break;
            default:
                break;
        }
        // On Done Animating
        //todo: Event OnDoneAnimatingTowards(panel);
        _currentScreenPanel = panel;
        SwitchUIPanel(panel);
        yield break;
    }


    private IEnumerator OpenLoadingGame() {
        // Enable
        _loadingGameScreen.gameObject.SetActive(true);

        // Remove EventListeners
        GameManager.InitializeGame -= OnStartGame;
        // Add EventListeners
        GameManager.LoadedGame += OnLoadedGame;
        yield break;
    }

    private IEnumerator OpenMainMenu() {
        // Disable
        _loadingGameScreen.gameObject.SetActive(false);
        // Enable
        //_mainMenu.gameObject.SetActive(true);

        // Remove EventListeners
        GameManager.LoadedGame -= OnLoadedGame;
        // Add EventListeners
        ServerManager.Instance.ConnectedToServer += OnConnectedToServer;
        yield break;
    }

    private IEnumerator OpenLobby() {
        // Disable
        //_mainMenu.gameObject.SetActive(false);
        // Enable
        //_lobbyScreen.gameObject.SetActive(true);

        // Remove EventListeners
        ServerManager.Instance.ConnectedToServer -= OnConnectedToServer;
        // Add EventListeners
        // -
        yield break;
    }

    private IEnumerator OpenLoadingMatch() {
        // -
        //_loadingMatchScreen.gameObject.SetActive(true);
        yield break;
    }

    private IEnumerator OpenInventory() {
        // -
        //_inventoryScreen.gameObject.SetActive(true);
        yield break;
    }

}