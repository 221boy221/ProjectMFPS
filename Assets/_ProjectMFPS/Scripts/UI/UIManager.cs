using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public enum ScreenPanel {
    LoadingGame,
    MainMenu,
    Lobby,
    Inventory,
    LoadingMatch
}

public class UIManager : MonoBehaviour {

    public static ScreenPanel UiState;
    public static UnityAction SwitchState;

    [SerializeField] private UIPanel _loadingGameScreen;
    [SerializeField] private UIPanel _mainMenu; // todo: make uipanelhandler for mainmenu that contains all sub-panels?
    [SerializeField] private UIPanel _lobbyScreen;
    [SerializeField] private UIPanel _loadingMatchScreen;
    [SerializeField] private UIPanel _inventoryScreen;


    void Awake() {
        // entry
        GameManager.Instance.InitializeGame += OnStartGame;
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
    private IEnumerator OpenPanel(ScreenPanel panel) {

        switch (panel) {
            case ScreenPanel.LoadingGame:
                yield return StartCoroutine(OpenLoadngGame());
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
        UiState = panel;
        SwitchState();
        yield break;
    }


    private IEnumerator OpenLoadngGame() {
        // -
        _loadingGameScreen.gameObject.SetActive(true);

        // Remove EventListeners
        GameManager.Instance.InitializeGame -= OnStartGame;
        // Add EventListeners
        GameManager.Instance.LoadedGame += OnLoadedGame;
        yield break;
    }

    private IEnumerator OpenMainMenu() {
        // -
        _mainMenu.gameObject.SetActive(true);

        // Remove EventListeners
        GameManager.Instance.LoadedGame -= OnLoadedGame;
        // Add EventListeners
        ServerManager.Instance.ConnectedToServer += OnConnectedToServer;
        yield break;
    }

    private IEnumerator OpenLobby() {
        // -
        _lobbyScreen.gameObject.SetActive(true);

        // Remove EventListeners
        ServerManager.Instance.ConnectedToServer -= OnConnectedToServer;
        // Add EventListeners
        // -
        yield break;
    }

    private IEnumerator OpenLoadingMatch() {
        // -
        _loadingMatchScreen.gameObject.SetActive(true);
        yield break;
    }

    private IEnumerator OpenInventory() {
        // -
        _inventoryScreen.gameObject.SetActive(true);
        yield break;
    }

}