using UnityEngine;

public abstract class UIPanel : MonoBehaviour {

    void Start()
    {
        UIManager.SwitchUIPanel += OnSwitchUIPanel;
        
        // todo: replace with UIManager calling inits of all panels
        InitReferences();
    }

    //void OnEnable() { UIManager.SwitchUIPanel += OnSwitchUIPanel; }
    void OnDisable() { UIManager.SwitchUIPanel -= OnSwitchUIPanel; }
    void OnDestroy() { UIManager.SwitchUIPanel -= OnSwitchUIPanel; }

    // EventHandler
    internal abstract void OnSwitchUIPanel(ScreenPanel panel);

    // Override me
    internal virtual void InitReferences() { }
    internal virtual void Destroy() { }

}
