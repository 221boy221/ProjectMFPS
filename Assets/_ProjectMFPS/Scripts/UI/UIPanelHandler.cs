using UnityEngine;
using System.Collections;

public class UIPanelHandler : MonoBehaviour {

	[SerializeField] private UIPanel[] _uiPanels;

    private bool _isActive = false;

    internal void SetActive(bool b)
    {
        if (_isActive == b) return;

        if (b)
        {
            _isActive = true;
            for (int i = _uiPanels.Length -1; i >= 0; i--)
            {
                // ? hoeft niet?
            }
        }
    }

    public bool Active
    {
        get { return _isActive; }
        set { _isActive = value; }
    }


}
