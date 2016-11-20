using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;
using System;

public class BasePopup : MonoBehaviour {

    // header
    [SerializeField] private Text _headerText;
    [SerializeField] private Button _minus;
    [SerializeField] private Button _close;
    // body
    [SerializeField] private Image _severity;
    [SerializeField] private Text _bodyText;
    [SerializeField] private Button[] _buttons;
    // Info
    private bool _hasSeverity = true;
    
    internal void Setup()
    {
        // Layout
        if (!_hasSeverity)
        {
            _bodyText.transform.parent.gameObject.AddComponent<HorizontalLayoutGroup>();
        }

        // Click Events
        _minus.onClick.AddListener(() => OnClickedMinus());
        _close.onClick.AddListener(() => OnClickedClose());

        // Check buttons
        if (_buttons == null)
        {
            Debug.LogError("Array of buttons is null; Can't properly add listeners to buttons");
            Debug.Break();
        }

        for (int i = _buttons.Length -1; i >= 0; i--)
        {
            SetButtonAction(i, () => OnClickedClose());
        }
    }

    private void OnClickedClose()
    {
        this.gameObject.SetActive(false);
    }

    private void OnClickedMinus()
    {
        //
    }

    public void SetButtonAction(int buttonIndex, UnityAction action)
    {
        if (buttonIndex > _buttons.Length - 1)
        {
            Debug.LogError("ButtonIndex out of range! Returning...");
            return;
        }

        _buttons[buttonIndex].onClick.AddListener(action);
    }

    private void SetAmountOfButtons(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            _buttons[i].gameObject.SetActive(true);
        }
    }
    

    // -- Getters and Setters -- //
    public int AmountOfButtons
    {
        get { return _buttons.Length; }
        set { SetAmountOfButtons(value); }
    }

    public string HeaderText
    {
        set { _headerText.text = value; }
    }

    public string BodyText
    {
        set { _bodyText.text = value; }
    }

    public Sprite Severity
    {
        set
        {
            _severity.gameObject.SetActive(true);
            _severity.sprite = value;
        }
    }

    public bool HasSeverity
    {
        set { _hasSeverity = value; }
    }
}
