using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Engine : MonoBehaviour
{
    #region Vars

    public event UnityAction<Engine> OnDestroyed = delegate { };
    public event UnityAction<Engine> OnRepaired = delegate { };

    [SerializeField]
    private float _repairTime = 5f;
    [SerializeField]
    private float _destroyTime = 6f;

    [SerializeField]
    private GameObject _normalGraphic;
    [SerializeField]
    private GameObject _brokenGraphic;
    [SerializeField]
    private ProgressBar _progressBar;

    private bool _isBroken = false;
    [SerializeField]
    private bool _isInteracting = false;
    private IEnumerator _currentAction;

    #endregion

    #region Methods
    
    /// <summary>
    /// Streams the engine states to the players that are not an owner of the object itself.
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="info"></param>
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(_isBroken);
            stream.SendNext(_isInteracting);
        }
        else
        {
            bool broken = (bool)stream.ReceiveNext();
            SetBroken(broken);
            Debug.Log("Engine is " + broken);
            _isInteracting = (bool)stream.ReceiveNext();
        }
    }

    /// <summary>
    /// Calls either Destroy() or Repair() regarding to the Engine state received in the parameter. Also switches graphics.
    /// </summary>
    /// <param name="broken"></param>
    private void SetBroken(bool broken)
    {
        // Sets the Model
        _normalGraphic.SetActive(!broken);
        _brokenGraphic.SetActive(broken);

        // Sets the state
        if (broken)
        {
            Destroy();
        }
        else
        {
            Repair();
        }
    }

    /// <summary>
    /// Called when player starts repairing the engine. Starts the Repair Anim and prevents other players from interacting at the same time.
    /// </summary>
    public void BeginRepair()
    {
        if (!_isBroken || _isInteracting)
        {
            return;
        }

        _isInteracting = true;
        _currentAction = RepairAnim();
        StartCoroutine(_currentAction);
        // Do some anim
        Debug.Log("repairing");
    }

    /// <summary>
    /// Called when player stops repairing the engine. Stops the Repair Anim and allows other players to interacting with it again.
    /// </summary>
    public void EndRepair()
    {
        if (!_isBroken || !_isInteracting)
        {
            return;
        }

        _isInteracting = false;
        StopCoroutine(_currentAction);
        // End some anim
        Debug.Log("cancel repair");
    }

    /// <summary>
    /// Handles the repair time and animation of the engine, also calls the progress bar ui to update.
    /// </summary>
    private IEnumerator RepairAnim()
    {
        float time = 0f;

        while (time < _repairTime)
        {
            yield return new WaitForFixedUpdate();
            time += Time.deltaTime;
            _progressBar.SetProgress(time / _repairTime);
            Debug.Log("progress: " + (time / _repairTime).ToString());
        }

        Repair();
        yield break;
    }

    /// <summary>
    /// Repairs the engine.
    /// </summary>
    private void Repair()
    {
        if (!_isBroken)
        {
            return;
        }

        _isInteracting = false;
        _isBroken = false;
        SetBroken(false);
        OnRepaired(this);
        Debug.Log("repaired!");
    }

    /// <summary>
    /// Called when player starts destroying the engine. Starts the Destroy Anim and prevents other players from interacting at the same time.
    /// </summary>
    public void BeginDestroy()
    {
        if (_isBroken || _isInteracting)
        {
            return;
        }

        _isInteracting = true;
        _currentAction = DestroyAnim();
        StartCoroutine(_currentAction);
        // Do some anim
        Debug.Log("destroying");
    }

    /// <summary>
    /// Called when player stops destroying the engine. Stops the Destroy Anim and allows other players to interacting with it again.
    /// </summary>
    public void EndDestroy()
    {
        if (_isBroken || !_isInteracting)
        {
            return;
        }

        _isInteracting = false;
        StopCoroutine(_currentAction);
        // End some anim
        Debug.Log("cancel destroy");
    }

    /// <summary>
    /// Handles the destroy time and animation of the engine, also calls the progress bar ui to update.
    /// </summary>
    private IEnumerator DestroyAnim()
    {
        float time = 0f;
        while (time < _destroyTime)
        {
            yield return new WaitForFixedUpdate();
            time += Time.deltaTime;
            _progressBar.SetProgress(1f - time / _destroyTime);
        }

        Destroy();
    }

    /// <summary>
    /// Destroys the engine.
    /// </summary>
    private void Destroy()
    {
        if (_isBroken)
        {
            return;
        }

        _isInteracting = false;
        _isBroken = true;
        SetBroken(true);
        OnDestroyed(this);
        Debug.Log("destroyed");
    } 

    #endregion
}