using UnityEngine;
using UnityEngine.Events;

public class UIPanel : MonoBehaviour
{
    #region Vars
    internal event UnityAction<UIPanelTypes> OpenUIPanelEvent = delegate { };

    protected RectTransform rectTransform;
    [SerializeField] internal UIPanelTypes panelType;
    #endregion

    #region Methods
    // Use this for initialization
    void Awake ()
    {
        rectTransform = this.GetComponent<RectTransform>();
	}
	
    /// <summary>
    /// Fires the OpenUIPanelEvent to which the UIManager listens in order to switch UI Panels.
    /// </summary>
    /// <param name="panel"></param>
    protected void OpenUIPanel(UIPanelTypes panel) 
    {
        OpenUIPanelEvent(panel);
    }

    public virtual void Toggle(bool value)
    {
        SetListeners(value);
    }

    protected virtual void SetListeners(bool on)
    {

    }

    #endregion
}
