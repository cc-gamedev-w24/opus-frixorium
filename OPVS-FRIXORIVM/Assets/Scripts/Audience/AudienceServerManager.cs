using System;
using TMPro;
using WebSocketSharp;
using UnityEngine;

public class AudienceServerManager : MonoBehaviour
{
    
    public delegate void PlayerCountChangedEventHandler(int newPlayerCount);
    public event PlayerCountChangedEventHandler OnPlayerCountChangedEvent;
    public delegate void GameCodeGeneratedEventHandler(string newLobbyCode);
    public event GameCodeGeneratedEventHandler OnGameCodeGeneratedEvent;
    
    private static AudienceServerManager _instance;
    private WebSocket _socket;
    private string _gameCode;
    private int _audienceCount;

    private const string ServerURL = "ws://localhost:8080/";
    private const string AuthenticationToken = "BTS02OQVKJ";

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    private void Start()
    {
        GenerateGameCode();
        InitializeSocket();
        SendAuthenticationData();
    }

    private void OnDestroy()
    {
        CloseSocket();
    }

    private void InitializeSocket()
    {
        _socket = new WebSocket(ServerURL);
        _socket.OnOpen += OnSocketOpen;
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

    private void OnSocketOpen(object sender, EventArgs e)
    {
        Debug.Log("Connected to server");
    }

    private void OnSocketMessage(object sender, MessageEventArgs e)
    {
        if (e.Data == "new_connection")
        {
            _audienceCount += 1;
            OnPlayerCountChangedEvent?.Invoke(_audienceCount);
        } 
        else if (e.Data == "client_disconnected")
        {
            _audienceCount -= 1;
            OnPlayerCountChangedEvent?.Invoke(_audienceCount);
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
        var initData = new GameData
        {
            messageType = "authentication",
            token = AuthenticationToken,
            gameCode = _gameCode
        };
        
        _socket.Send(JsonUtility.ToJson(initData));
    }
}

public class GameData
{
    public string messageType;
    public string token;
    public string gameCode;
}
