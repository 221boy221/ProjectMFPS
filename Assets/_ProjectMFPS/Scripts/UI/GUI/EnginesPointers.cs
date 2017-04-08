using System.Collections.Generic;
using UnityEngine;

public class EnginesPointers : MonoBehaviour
{
    #region Vars

    [SerializeField]
    private GameObject _pointerPrefab;

    [SerializeField]
    private RectTransform _origin;

    [SerializeField]
    private float _maxDistance = 280f;
    
    private Engine[] _engines;
    private Dictionary<Engine, RectTransform> _pointers;

    #endregion

    #region Methods

    private void Awake()
    {
        GameHandler.Instance.OnEnginesSpawned += Instance_OnEnginesSpawned;
        _pointers = new Dictionary<Engine, RectTransform>();
    }

    private void Instance_OnEnginesSpawned(GameObject[] objects)
    {
        Debug.Log("On Spawned");
        GameHandler.Instance.OnEnginesSpawned -= Instance_OnEnginesSpawned;
        List<Engine> engines = new List<Engine>();
        foreach (GameObject gameObj in objects)
        {
            Engine engine = gameObj.GetComponent<Engine>();
            if (engine != null)
            {
                engines.Add(engine);
                engine.OnDestroyed += EngineDestroyed;
                engine.OnRepaired += EngineRepaired;
            }
        }
        _engines = engines.ToArray();
    }

    private void EngineDestroyed(Engine engine)
    {
        Debug.Log("On Destroyed");
        GameObject pointer = Instantiate<GameObject>(_pointerPrefab);
        RectTransform transform = pointer.GetComponent<RectTransform>();
        transform.SetParent(_origin);
        transform.localPosition = Vector3.zero;
        _pointers.Add(engine, transform);
    }

    private void EngineRepaired(Engine engine)
    {
        RectTransform pointer = _pointers[engine];
        Destroy(pointer.gameObject);
        _pointers.Remove(engine);
    }

    private void Update()
    {
        foreach (KeyValuePair<Engine, RectTransform> item in _pointers)
        {
            Vector2 pos = Camera.main.WorldToScreenPoint(item.Key.transform.position) - _origin.position;
            item.Value.gameObject.SetActive(pos.magnitude > _maxDistance * 1.5f);
            if (item.Value.gameObject.activeSelf)
            {
                float angle = Mathf.Atan2(pos.y, pos.x) * (180f / Mathf.PI);
                item.Value.eulerAngles = new Vector3(0f, 0f, angle);
                item.Value.localPosition = pos.normalized * _maxDistance;
            }
        }
    }

    #endregion
}