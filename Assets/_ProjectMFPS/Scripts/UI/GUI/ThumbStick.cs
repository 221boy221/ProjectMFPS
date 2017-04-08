using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ThumbStick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    #region Vars

    public event UnityAction<Vector2> OnDirectionChanged = delegate { };

    [SerializeField]
    private RectTransform _origin;

    [SerializeField]
    private float _maxRange = 20f;

    [SerializeField]
    private float _moveBackSpeed = 10f;

    private RectTransform _trans;
    private IEnumerator _moveAnim;

    #endregion

    #region Methods

    private void Awake()
    {
        _trans = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData data)
    {
        if (_moveAnim != null)
        {
            StopCoroutine(_moveAnim);
        }
    }

    public void OnDrag(PointerEventData data)
    {
        // Calculate relative position
        Vector3 pos = data.position - _origin.anchoredPosition;
        
        // Limit
        if (pos.magnitude > _maxRange)
        {
            pos = pos.normalized * _maxRange;
        }

        _trans.anchoredPosition = pos;
        DirectionChanged(pos / _maxRange);
    }
    
    public void OnEndDrag(PointerEventData data)
    {
        _moveAnim = MoveBack();
        StartCoroutine(_moveAnim);
    }

    private IEnumerator MoveBack()
    {
        Vector2 pos = _trans.anchoredPosition;
        while (pos.magnitude > 0.1f)
        {
            yield return new WaitForFixedUpdate();
            pos = pos.normalized * Mathf.Max(0f, pos.magnitude - _moveBackSpeed * Time.fixedDeltaTime);
            _trans.anchoredPosition = pos;
            DirectionChanged(pos / _maxRange);
        }

        _moveAnim = null;
    }

    private void DirectionChanged(Vector2 direction)
    {
        if (OnDirectionChanged != null)
        {
            OnDirectionChanged(direction);
        }
    }
    
    #endregion
}