using UnityEngine;

public class UIPanel : MonoBehaviour {

    void Awake () {
        UIManager.SwitchState += OnUiSwitchState;
    }

    void Start() {
        // todo: replace with UIManager calling inits of all panels
        InitReferences();
    }

    void OnDestroy() {

    }

    // EventHandler
    private void OnUiSwitchState() {
        UpdateLayout();
    }

    // Override me
    internal virtual void InitReferences() { }
    internal virtual void UpdateLayout() { }
    internal virtual void Destroy() { }

}
