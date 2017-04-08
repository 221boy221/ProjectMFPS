using UnityEngine;

public class DoorButton : MonoBehaviour, IPlayerInteractable
{
    #region Vars

    [SerializeField]
    private Door _door;

    #endregion

    #region Methods

    public void StartInteraction(PlayerInteracter sender)
    {
        return;
    }

    /// <summary>
    /// Transfers the ownership of the door object to the sender and calls Close() on the referenced door.
    /// </summary>
    /// <param name="sender"></param>
    public void EndInteraction(PlayerInteracter sender)
    {
        _door.gameObject.GetComponent<PhotonView>().RequestOwnership();
        _door.Close();
    }

    #endregion

    #region Properties

    public bool isInteractable
    {
        get { return true; }
    }

    public InteractionTypes type
    {
        get { return InteractionTypes.Doors; }
    }
    
    #endregion
}