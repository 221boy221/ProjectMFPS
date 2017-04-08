using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    #region Vars

    public static InputHandler Instance;

    public event UnityAction<Vector2> OnDirectionChanged;
    public event UnityAction OnJumpClicked;
    public event UnityAction OnInteractionDown;
    public event UnityAction OnInteractionUp;

    [SerializeField]
    private bool _allowDebugKeys = true;

    #endregion

    #region Constructor

    public InputHandler()
    {
        Instance = this;
        OnDirectionChanged = delegate { };
        OnJumpClicked = delegate { };
        OnInteractionDown = delegate { };
        OnInteractionUp = delegate { };
    }

    #endregion

    #region Methods

//#if UNITY_EDITOR // Development code
    Vector2 lastDir = Vector2.zero;
    private void Update()
    {
        if (!_allowDebugKeys)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpClicked();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            InteractionDown();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            InteractionUp();
        }
        Vector2 newDir = Vector2.zero;
        if (Input.GetKey(KeyCode.D))
        {
            newDir.x += 1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            newDir.x -= 1;
        }
        if (Input.GetKey(KeyCode.W))
        {
            newDir.y += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            newDir.y -= 1;
        }
        if (newDir != lastDir)
        {
            lastDir = newDir;
            SetDirection(newDir);
        }
    }
    //#endif

    /// <summary>
    /// Fires the OnDirectionChanged event with the given direction which gets applied by the PlayerMovement.
    /// </summary>
    public void SetDirection(Vector2 direction)
    {
        if (OnDirectionChanged != null)
        {
            OnDirectionChanged(direction);
        }
    }

    /// <summary>
    /// Fires the OnJumpClicked event which gets applied by the PlayerMovement.
    /// </summary>
    public void JumpClicked()
    {
        if (OnJumpClicked != null)
        {
            OnJumpClicked();
        }
    }

    /// <summary>
    /// Fires the OnInteractionDown event which gets applied by the PlayerInteracter and PlayerShoot.
    /// </summary>
    public void InteractionDown()
    {
        if (OnInteractionDown != null)
        {
            OnInteractionDown();
        }
    }

    /// <summary>
    /// Fires the OnInteractionUp event which gets applied by the PlayerInteracter and PlayerShoot.
    /// </summary>
    public void InteractionUp()
    {
        if (OnInteractionUp != null)
        {
            OnInteractionUp();
        }
    }

    #endregion
}
