using UnityEngine;

public interface IPlayerInteractable
{
    GameObject gameObject { get; }
    bool isInteractable { get; }
    InteractionTypes type { get; }

    void StartInteraction(PlayerInteracter sender);
    void EndInteraction(PlayerInteracter sender);
}