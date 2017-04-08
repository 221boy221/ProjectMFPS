using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameHandler : MonoBehaviour
{
    #region Vars

    public static GameHandler Instance;
    public event UnityAction<GameObject[]> OnEnginesSpawned = delegate { };

    [SerializeField] private List<PlayerRoles> _playerRoles;
    [SerializeField] private List<Transform> _playerSpawnPoints;
    [SerializeField] private List<GameObject> _playerCharacters;
    [SerializeField] private List<GameObject> _engines;
    [SerializeField] private List<Transform> _engineSpawnPoints;
    [SerializeField] private GUIMenu _GUI;

    private bool allPlayersAreReady = false;
    private const string SCENE_MENU = "Boy";

    #endregion

    #region Constructor

    public GameHandler()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    #endregion

    #region Methods

    public void OnEnable()
    {
        //PhotonNetwork.player.CustomProperties[PlayerProperties.READY_STATE] = true;
        
        ExitGames.Client.Photon.Hashtable table = new ExitGames.Client.Photon.Hashtable
        {
            { PlayerProperties.READY_STATE, true },
            { PlayerProperties.ROLE, PhotonNetwork.player.CustomProperties[PlayerProperties.ROLE] }
        };
        PhotonNetwork.player.SetCustomProperties(table);

        if (PhotonNetwork.player.IsMasterClient)
        {
            StartCoroutine(WaitUntilAllPlayersLoaded());
        }
    }

    /// <summary>
    /// Waits for all the PhotonNetwork players to be ready before calling the GameSetup(). 
    /// Uses CustomProperties READY_STATE.
    /// </summary>
    private IEnumerator WaitUntilAllPlayersLoaded()
    {
        while (!allPlayersAreReady)
        {
            // Scan time
            yield return new WaitForSeconds(0.5f);
            CheckForReadyPlayers();
        }

        StartCoroutine(GameSetup());
        yield break;
    }

    /// <summary>
    /// Loops through all PhotonPlayers in PhotonNetwork.PlayerList to check if they are Ready or not.
    /// </summary>
    private void CheckForReadyPlayers()
    {
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            object isReady;
            if (player.CustomProperties.TryGetValue(PlayerProperties.READY_STATE, out isReady))
            {
                if (isReady == null || (bool)isReady == false)
                {
                    Debug.Log(player.NickName + " is false");
                    return;
                }
            }
        }

        allPlayersAreReady = true;
    }

    /// <summary>
    /// The master client sets up all the players and assigns them to the appropiate clients, as well as setup the networked level objects.
    /// </summary>
    public IEnumerator GameSetup()
    {
        if (PhotonNetwork.isMasterClient)
        {
            // Setup
            yield return StartCoroutine(SetupPlayers());
            yield return StartCoroutine(SetupEngines());

            // Call StartGame
            gameObject.GetComponent<PhotonView>().RPC("StartGame", PhotonTargets.All, _engines);
        }

        yield break;
    }

    /// <summary>
    /// Creates a player prefab for each connected player, assigns them, and gives them a random role and spawn position.
    /// </summary>
    private IEnumerator SetupPlayers()
    {
        // Player role assigning and spawning
        List<PlayerRoles> playerRoles = new List<PlayerRoles>(_playerRoles);
        List<Transform> playerSpawnPoints = new List<Transform>(_playerSpawnPoints);
        foreach (PhotonPlayer player in PhotonNetwork.playerList)
        {
            int randomPlayerRole = Random.Range(0, playerRoles.Count());
            int randomPlayerSpawnPoint = Random.Range(0, playerSpawnPoints.Count());

            // Instantiate prefab  with set role and spawnpoint
            GameObject instantiatedPlayerRole = PhotonNetwork.Instantiate(
                playerRoles[randomPlayerRole].ToString(),
                playerSpawnPoints[randomPlayerSpawnPoint].position,
                new Quaternion(0, 0, 0, 0),
                0);

            // Attach the prefab to the corresponding PhotonNetwork.Player
            instantiatedPlayerRole.GetComponent<PhotonView>().TransferOwnership(player);
            _playerCharacters.Add(instantiatedPlayerRole);

            // Save the role into the custom player properties of the PhotonNetwork.Player
            player.CustomProperties[PlayerProperties.ROLE] = playerRoles[randomPlayerRole];

            // Remove the used role and spawnpoint from the list
            playerRoles.RemoveAt(randomPlayerRole);
            playerSpawnPoints.RemoveAt(randomPlayerSpawnPoint);
        }

        yield break;
    }

    /// <summary>
    /// Instantiates all the networked engine prefabs and places them on a random spawn point.
    /// </summary>
    private IEnumerator SetupEngines()
    {
        // Engines
        List<Transform> engineSpawnPoints = new List<Transform>(_engineSpawnPoints);
        foreach (GameObject engine in _engines)
        {
            int randomEngineSpawnPoint = Random.Range(0, engineSpawnPoints.Count());

            // Instantiate the engine on the chosen spawnpoint
            PhotonNetwork.Instantiate(
                engine.name,
                engineSpawnPoints[randomEngineSpawnPoint].position,
                new Quaternion(0, 0, 0, 0),
                0);

            // Remove the used spawnpoint from the list
            engineSpawnPoints.RemoveAt(randomEngineSpawnPoint);
        }

        yield break;
    }

    /// <summary>
    /// This method gets called once the GameSetup is done and the game is ready to start.
    /// </summary>
    [PunRPC]
    private void StartGame(List<GameObject> engines)
    {
        Debug.Log("Start Game Function");

        // Fires the OnEnginesSpawned event of which the EnginePointers class listens to.
        if (OnEnginesSpawned != null)
        {
            OnEnginesSpawned(engines.ToArray());
        }
    }

    /// <summary>
    /// Gets called when a Player gets hit by a bullet. Fires the EndGame event.
    /// </summary>
    public void BulletHitPlayer(Player player)
    {
        gameObject.GetComponent<PhotonView>().RPC("EndGame", PhotonTargets.All, player.Role);
    }

    /// <summary>
    /// Gets fired when either all engines are destroyed, a character gets killed or a player leaves the game.
    /// Shows the GameOver screen with appropiate win/lose text for each seperate client.
    /// </summary>
    [PunRPC]
    private IEnumerator EndGame(PlayerRoles playerWhoDied)
    {
        PlayerRoles role;
        string text = "";

        if (PhotonNetwork.player.CustomProperties[PlayerProperties.ROLE] != null)
        {
            role = (PlayerRoles)PhotonNetwork.player.CustomProperties[PlayerProperties.ROLE];
            if (playerWhoDied == PlayerRoles.Engineer)
            {
                if (role == PlayerRoles.Saboteur)
                {
                    text = "You Win!";
                }
                else
                {
                    text = "Game Over";
                }
            }
            else if (playerWhoDied == PlayerRoles.Saboteur)
            {
                if (role == PlayerRoles.Saboteur)
                {
                    text = "Game Over";
                }
                else
                {
                    text = "You Win!";
                }
            }
        }
        else
        {
            Debug.LogError("Player [" + PhotonNetwork.player.NickName + "]'s role in PlayerProperties is Null");
            text = "Game Over";
        }
        
        yield return StartCoroutine(_GUI.EndGameScreen(text));
        yield return new WaitForSeconds(3f);

        if (PhotonNetwork.player.IsMasterClient)
        {
            NetworkManager.Instance.SetDefaultPlayerProperties(true);
        }

        PhotonNetwork.LoadLevel(SCENE_MENU);
        yield break;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    #endregion
}