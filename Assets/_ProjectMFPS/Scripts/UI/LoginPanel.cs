using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoginPanel : BaseOverlay
{
    internal event UnityAction<string, string> OnClickedLogin = delegate { };
	[SerializeField] private InputField _usernameInput;
	[SerializeField] private InputField _passwordInput;
	[SerializeField] private Button _loginButton;

    private string _username = "";
    private string _password = "";

    void OnEnable()
    {
        _usernameInput.onEndEdit.AddListener(SetUsername);
        _passwordInput.onEndEdit.AddListener(SetPassword);
        _loginButton.onClick.AddListener(Login);
    }

    private void SetUsername(string v)
    {
        _username = v;
    }

    private void SetPassword(string v)
    {
        _password = v;
    }

    private void Login()
    {
        OnClickedLogin(_username, _password);
    }

    void OnDisable()
    {
        _usernameInput.onEndEdit.RemoveAllListeners();
        _passwordInput.onEndEdit.RemoveAllListeners();
        _loginButton.onClick.RemoveAllListeners();
    }

}
