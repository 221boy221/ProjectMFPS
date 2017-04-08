using UnityEngine;

public class RepairEngine : MonoBehaviour, IPlayerInteractable
{
    #region Vars

    [SerializeField]
    private Engine _engine;

    private PlayerInteracter _currentPlayer;

    #endregion

    #region Methods

    /// <summary>
    /// Sets the current interacting player to the sender parameter and calls the BeginRepair() on the Engine.
    /// </summary>
    public void StartInteraction(PlayerInteracter sender)
    {
        if (_currentPlayer == null)
        {
            _currentPlayer = sender;
            _engine.BeginRepair();
        }
    }

    /// <summary>
    /// Sets the current interacting player to null and calls the EndRepair() on the Engine.
    /// </summary>
    public void EndInteraction(PlayerInteracter sender)
    {
        if (_currentPlayer == sender)
        {
            _currentPlayer = null;
            _engine.EndRepair();
        }
    }

    #endregion

    #region Properties

    public bool isInteractable
    {
        get { return true; }
    }

    public InteractionTypes type
    {
        get { return InteractionTypes.RepairEngine; }
    }
    
    #endregion
}