using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WebSocketSharp;

    
[CreateAssetMenu(menuName = "Single-Instance SOs/Audience Manager")]
public class AudienceServerManager : ScriptableObject
{
    public event Action<int> OnPlayerCountChangedEvent;
    public event Action<string> OnGameCodeGeneratedEvent;

    public event Action<string> OnTrialSelectedEvent;

    public bool Enabled { get; private set; }

    private WebSocket _socket;
    private string _gameCode;
    public int AudienceCount { get; private set; }

    private const string ServerURL = "ws://localhost:8080/";
    private const string AuthenticationToken = "BTS02OQVKJ";

    private void OnEnable() => Enable();

    public void Enable()
    {
        GenerateGameCode();
        InitializeSocket();
        SendAuthenticationData();
        Enabled = true;
    }

    private void OnDisable() => Disable();
    public void Disable()
    {
        CloseSocket();
        Enabled = false;
    }

    private void InitializeSocket()
    {
        _socket = new WebSocket(ServerURL);
        _socket.OnMessage += OnSocketMessage;
        _socket.Connect();
    }

    private void CloseSocket()
    {
        if (_socket != null && _socket.IsAlive)
        {
            _socket.Close();
        }
    }

    private void OnSocketMessage(object sender, MessageEventArgs e)
    {
        var messageData = JsonUtility.FromJson<Message>(e.Data);
        switch (messageData.messageType)
        {
            case "new_connection":
                AudienceCount++;
                OnPlayerCountChangedEvent?.Invoke(AudienceCount);
                break;
            case "client_disconnected":
                AudienceCount--;
                OnPlayerCountChangedEvent?.Invoke(AudienceCount);
                break;
            case "vote_result":
                Debug.Log(messageData.winningTrial);
                OnTrialSelectedEvent?.Invoke(messageData.winningTrial);
                break;
        }
    }

    private void GenerateGameCode()
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var stringChars = new char[8];
        var random = new System.Random();

        for (var i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        _gameCode = new string(stringChars);
        OnGameCodeGeneratedEvent?.Invoke(_gameCode);
    }

    private void SendAuthenticationData()
    {
        var initData = new Message
        {
            messageType = "authentication",
            token = AuthenticationToken,
            gameCode = _gameCode
        };
    
        _socket.Send(JsonUtility.ToJson(initData));
    }

    public void SendTrialDataToServer(IEnumerable<Trial> trials)
    {
        var trialData = new Message
        {
            messageType = "voting",
            token = AuthenticationToken,
            gameCode = _gameCode,
            trialNames = trials.Select(trial => trial.TrialName).ToArray()
        };

        _socket.Send(JsonUtility.ToJson(trialData));
    }

    [Serializable]
    public class Message
    {
        public string messageType;
        public string token;
        public string gameCode;
        public string[] trialNames;
        public string winningTrial;
    }
}
