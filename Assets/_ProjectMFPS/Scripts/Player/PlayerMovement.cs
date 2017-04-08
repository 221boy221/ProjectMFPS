using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Vars

    [SerializeField]
    private PlayerAnimator _animator;
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _climbSpeed = 5f;
    [SerializeField]
    private float _jumpForce = 5f;
    [SerializeField]
    private Rigidbody2D _rigid;

    private bool _isClimbing = false;
    private int _groundTouching = 0;
    private Vector2 _targetDirection;
    private int _groundLayer;
    private int _ladderLayer;

    #endregion

    #region Methods

    public void Start()
    {
        _targetDirection = Vector2.zero;
        _groundLayer = LayerMask.NameToLayer(Layers.Ground);
        _ladderLayer = LayerMask.NameToLayer(Layers.Ladder);
        InputHandler.Instance.OnDirectionChanged += OnDirectionChanged;
        InputHandler.Instance.OnJumpClicked += Jump;
    }

    private void FixedUpdate()
    {
        Vector2 velo = _rigid.velocity;
        if (_targetDirection != null)
        {
            velo.x = _targetDirection.x * _speed;
            if (_isClimbing)
            {
                velo.y = _targetDirection.y * _climbSpeed;
            }
            _rigid.velocity = velo;
        }

        bool moving = velo.x != 0f;
        if (moving)
        {
            float direction = velo.x / Mathf.Abs(velo.x);
            if (direction != transform.localScale.x)
            {
                transform.localScale = new Vector3(direction, 1f, 1f);
            }
        }
        _animator.IsMoving = moving;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _groundLayer)
        {
            if (_groundTouching == 0)
            {
                _animator.Land();
            }
            _groundTouching++;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == _groundLayer)
        {
            _groundTouching--;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!_isClimbing && collision.gameObject.layer == _ladderLayer && _rigid.velocity.y < 0f)
        {
            isClimbing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == _ladderLayer)
        {
            isClimbing = false;
        }
    }

    private void OnDirectionChanged(Vector2 direction)
    {
        _targetDirection = direction;
    }

    public void Jump()
    {
        if (_isClimbing || _groundTouching < 1)
        {
            return;
        }
        Vector2 velo = _rigid.velocity;
        velo.y = _jumpForce;
        _rigid.velocity = velo;
        _animator.Jump();
    }

    #endregion

    #region Properties

    private bool isClimbing
    {
        set
        {
            _rigid.gravityScale = value ? 0f : 1f;
            _isClimbing = value;
            if (_isClimbing && _groundTouching == 0)
            {
                _animator.Land();
            }
        }
    }

    #endregion
}