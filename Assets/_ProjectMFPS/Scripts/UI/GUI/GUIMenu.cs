using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GUIMenu : MonoBehaviour
{

    #region Vars

    public static GUIMenu Instance;

    [SerializeField]
    private ThumbStick _thumbStick;

    [SerializeField]
    private GameObject _interactionButton;

    [SerializeField]
    private GameObject _shootButton;

    [SerializeField] private CanvasGroup _blackOverlay;
    [SerializeField] private Text _blackOverlayText;
    [SerializeField] private Text _roleText;
    #endregion

    #region Methods

    private void Awake()
    {
        Instance = this;
        _blackOverlay.gameObject.SetActive(false);
        _blackOverlay.alpha = 0f;
    }

    public void SetRole(PlayerRoles role)
    {
        _shootButton.SetActive(role == PlayerRoles.Captain);
        _interactionButton.SetActive(role != PlayerRoles.Captain);
        _roleText.text = role.ToString();
    }

    private void ThumbStick_OnDirectionChanged(Vector2 direction)
    {
        InputHandler.Instance.SetDirection(direction);
    }

    public void JumpClick()
    {
        InputHandler.Instance.JumpClicked();
    }

    public void InteractDown()
    {
        InputHandler.Instance.InteractionDown();
    }

    public void InteractUp()
    {
        InputHandler.Instance.InteractionUp();
    }

    private void OnEnable()
    {
        _thumbStick.OnDirectionChanged += ThumbStick_OnDirectionChanged;
    }

    private void OnDisable()
    {
        _thumbStick.OnDirectionChanged -= ThumbStick_OnDirectionChanged;
    }
    
    public IEnumerator EndGameScreen(string text)
    {
        _blackOverlay.gameObject.SetActive(true);
        _blackOverlay.alpha = 0;
        _blackOverlayText.text = text;

        float time = 2f;

        while (_blackOverlay.alpha < 1)
        {
            _blackOverlay.alpha += Time.deltaTime / time;
            yield return null;
        }

        yield break;
    }

    #endregion

}