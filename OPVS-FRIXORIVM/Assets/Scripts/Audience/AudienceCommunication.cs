using System;
using TMPro;
using WebSocketSharp;
using UnityEngine;

public class AudienceCommunication : MonoBehaviour
{
    [SerializeField] private TMP_Text gameCodeText;

    private static AudienceCommunication _instance;
    private WebSocket _socket;
    private string _gameCode;
    private int _playerCount;

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
        switch (e.Data)
        {
            case "new_connection":
                _playerCount++;
                break;
            case "client_disconnected":
                _playerCount--;
                break;
        }
        Debug.Log($"Player count: {_playerCount}");
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
        gameCodeText.text = $"Game Code: {_gameCode}";
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

    public void OnStartGame()
    {
        var gameData = new GameData
        {
            messageType = "voting",
            token = AuthenticationToken,
            gameCode = _gameCode
        };
        
        _socket.Send(JsonUtility.ToJson(gameData));
    }
}

public class GameData
{
    public string messageType;
    public string token;
    public string gameCode;
}
