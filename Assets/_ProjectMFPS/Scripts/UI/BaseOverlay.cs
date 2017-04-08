using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BaseOverlay : MonoBehaviour
{

    public event UnityAction OnClose = delegate { };
    public event UnityAction OnMinimize = delegate { };

    [SerializeField] private Button close;
    [SerializeField] private Button minimize;

    private void OnEnable()
    {
        close.onClick.AddListener(OnClickedClose);
        minimize.onClick.AddListener(OnClickedMinimize);
    }

    private void OnClickedClose()
    {
        OnClose();
    }

    private void OnClickedMinimize()
    {
        OnMinimize();
    }

    private void OnDisable()
    {
        close.onClick.RemoveAllListeners();
        minimize.onClick.RemoveAllListeners();
    }

}
