using UnityEngine;
using System.Collections;

internal enum PopupType
{
    None = 0,
    Default,
    MultipleChoice,
    TripleChoice,
    Login
}

internal enum PopupSeverity
{
    None = 0,
    Information,
    Question,
    Warning,
    Error
}

public class PopupFactory : MonoBehaviour {

    public static PopupFactory Instance;
    [SerializeField] private GameObject _popupPrefab;
    [SerializeField] private RectTransform _alwaysOnTopCanvasObj;
    [SerializeField] private Sprite[] _severityLevels;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    internal BasePopup CreatePopup(PopupType type)
    {
        GameObject popupObj = Instantiate(_popupPrefab, Vector3.zero, Quaternion.identity, _alwaysOnTopCanvasObj) as GameObject;
        BasePopup popup = popupObj.GetComponent<BasePopup>();

        popup.AmountOfButtons = (int)type;
        popup.HasSeverity = false;
        popup.Setup();

        return popup;
    }

    internal BasePopup CreatePopup(PopupType type, PopupSeverity severity)
    {
        GameObject popupObj = Instantiate(_popupPrefab, Vector3.zero, Quaternion.identity, _alwaysOnTopCanvasObj) as GameObject;
        BasePopup popup = popupObj.GetComponent<BasePopup>();

        popup.AmountOfButtons = (int)type;

        if ((int)severity > 0)
        {
            popup.Severity = _severityLevels[(int)severity];
        }

        popup.Setup();

        return popup;
    }
}
