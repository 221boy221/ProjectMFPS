using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    #region Vars

    [SerializeField]
    private Animator _animator;

    #endregion

    #region Methods

    public void Jump()
    {
        _animator.ResetTrigger("Land");
        _animator.SetTrigger("Jump");
    }

    public void Land()
    {
        _animator.SetTrigger("Land");
    }

    public void Shoot()
    {
        _animator.SetTrigger("Shoot");
    }

    #endregion

    #region Properties

    public bool IsMoving
    {
        get
        {
            return _animator.GetBool("IsMoving");
        }
        set
        {
            _animator.SetBool("IsMoving", value);
        }
    }

    #endregion
}