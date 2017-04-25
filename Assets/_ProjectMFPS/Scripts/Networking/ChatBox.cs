using ExitGames.Client.Photon.Chat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using System;

public class ChatBox : MonoBehaviour, IChatClientListener {

    public static ChatBox Instance;

    private ChatClient _chatclient;
    private string _appId = "";
    private string[] _availableChannels;
    private string _currentChannel;
    private string _messageText = "";

    void Start()
    {
        Instance = this;
        _appId = PhotonNetwork.PhotonServerSettings.ChatAppID;
    }

    void Update()
    {
        _chatclient.Service();
    }

    void OnEnable()
    {
        _chatclient = new ChatClient(this);
        _chatclient.ChatRegion = ServerManager.Instance.CurrentRegion.ToString();
        _chatclient.Connect(_appId, GameManager.Version, null);

        _chatclient.Subscribe(_availableChannels);
    }

    void OnDisable()
    {
        _chatclient.Disconnect();
    }


    public void SetMessage(string text)
    {
        _messageText = text;
    }

    public void SendMessage()
    {
        if (_currentChannel == "")
        {
            Debug.LogError("Current Channel is empty");
            return;
        }

        if (!_chatclient.CanChatInChannel(_currentChannel))
        {
            Debug.LogError("Not allowed to chat in current channel: " + _currentChannel);
            return;
        }

        string[] wordsOfMessage = _messageText.Split(' ');
        string command = wordsOfMessage[0];
        
        if (command == "/whisper")
        {
            Debug.Log("Whisper command");
            string targetUsername = wordsOfMessage[1];

            List<string> strippedMessage = new List<string>(wordsOfMessage.Length - 2);
            for (int i = 2; i < wordsOfMessage.Length - 1; i++)
            {
                strippedMessage.Add(wordsOfMessage[i]);
            }

            _chatclient.SendPrivateMessage(targetUsername, strippedMessage.ToArray());
        }

        _chatclient.PublishMessage(_currentChannel, _messageText);
    }

    public void JoinChannel(string channel)
    {
        _currentChannel = channel;
    }


    public void DebugReturn(DebugLevel level, string message)
    {
        //
    }

    public void OnChatStateChange(ChatState state)
    {
        //
    }

    public void OnConnected()
    {
        _chatclient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnDisconnected()
    {
        _chatclient.SetOnlineStatus(ChatUserStatus.Offline);
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string msgs = "";
        for (int i = 0; i < senders.Length; i++)
        {
            msgs = string.Format("{0}{1}={2}, ", msgs, senders[i], messages[i]);
        }
        Debug.Log("OnGetMessages: "+ channelName + " ("+ senders.Length + ") > "+ msgs);
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        ChatChannel ch = _chatclient.PrivateChannels[channelName];
        foreach (object msg in ch.Messages)
        {
            Debug.Log(msg);
        }
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        //
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        //
    }

    public void OnUnsubscribed(string[] channels)
    {
        //
    }


}
