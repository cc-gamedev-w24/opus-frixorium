using System;
using UnityEngine;
using WebSocketSharp;

namespace Audience
{
    public class AudienceServerManager : MonoBehaviour
    {
        [SerializeField] private Trial[] trials;
        
        public event Action<int> OnPlayerCountChangedEvent;
        public event Action<string> OnGameCodeGeneratedEvent;
    
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
                    _audienceCount++;
                    OnPlayerCountChangedEvent?.Invoke(_audienceCount);
                    break;
                case "client_disconnected":
                    _audienceCount--;
                    OnPlayerCountChangedEvent?.Invoke(_audienceCount);
                    break;
                case "vote_result":
                    Debug.Log(messageData.winningTrial);
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

        public void SendTrialDataToServer()
        {
            var trialData = new Message
            {
                messageType = "voting",
                token = AuthenticationToken,
                gameCode = _gameCode,
                trialNames = new string[trials.Length]
            };

            for (var i = 0; i < trials.Length; i++)
            {
                trialData.trialNames[i] = trials[i].name;
            }
        
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
}
