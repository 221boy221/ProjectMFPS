using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInteracter : MonoBehaviour
{
    #region Vars

    public event UnityAction<InteractionTypes> OnInteractStart;
    public event UnityAction<InteractionTypes> OnInteractEnd;
    [SerializeField]
    private List<InteractionTypes> _enabledInteractions;
    private List<IPlayerInteractable> _closeInteractables;
    private IPlayerInteractable _currentInteractable;
    private int _interactionLayer;
    private PhotonView _photonView;

    #endregion

    #region Methods

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        _interactionLayer = LayerMask.NameToLayer(Layers.Interactable);
        _closeInteractables = new List<IPlayerInteractable>();
        InputHandler.Instance.OnInteractionDown += InteractionDown;
        InputHandler.Instance.OnInteractionUp += InteractionUp;
    }

    public void OnDestroy()
    {
        InputHandler.Instance.OnInteractionDown -= InteractionDown;
        InputHandler.Instance.OnInteractionUp -= InteractionUp;
    }

    private void InteractionDown()
    {
        Vector3 currentPos = _photonView.GetComponent<Transform>().position;
        _currentInteractable = null;
        float closest = float.MaxValue;
        foreach (IPlayerInteractable item in _closeInteractables)
        {
            Debug.Log(_enabledInteractions);
            if (_enabledInteractions.Contains(item.type))
            {
                Transform trans = item.gameObject.GetComponent<Transform>();
                float distance = Mathf.Sqrt(Mathf.Pow(trans.position.x - currentPos.x, 2) + Mathf.Pow(trans.position.y - currentPos.y, 2));
                if (distance < closest)
                {
                    closest = distance;
                    _currentInteractable = item;
                }
            }
        }
        if (_currentInteractable != null)
        {
            _currentInteractable.StartInteraction(this);
            if (_currentInteractable.gameObject.GetComponent<PhotonView>() != null)
            {
                _currentInteractable.gameObject.GetComponent<PhotonView>().RequestOwnership();
            }
        }
    }

    private void InteractionUp()
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.EndInteraction(this);
            _currentInteractable = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _interactionLayer)
        {
            IPlayerInteractable[] interactables = collision.gameObject.GetComponents<IPlayerInteractable>();
            if (interactables.Length > 0)
            {
                foreach (IPlayerInteractable interactable in interactables)
                {
                    _closeInteractables.Add(interactable);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _interactionLayer)
        {
            IPlayerInteractable[] interactables = collision.gameObject.GetComponents<IPlayerInteractable>();
            if (interactables.Length > 0)
            {
                foreach (IPlayerInteractable interactable in interactables)
                {
                    _closeInteractables.Remove(interactable);
                    if (interactable == _currentInteractable)
                    {
                        InteractionUp();
                    }
                }
            }
        }
    }
    

    #endregion
}