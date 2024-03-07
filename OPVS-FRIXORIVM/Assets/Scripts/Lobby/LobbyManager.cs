using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lobby
{
    public class LobbyUIManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private GameObject countdownPanel;
        [SerializeField] private GameObject playersNotReadyPanel;
        [SerializeField] private TMP_Dropdown roundTimerDropdown;
        [SerializeField] private TMP_Dropdown numberOfRoundsDropdown;
        [SerializeField] private Toggle audienceEnabledToggle;

        [Header("Countdown Settings")]
        [SerializeField] private float countdownTimer = 3.0f;

        [Header("References")]
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private GameSettings gameSettings;

        [SerializeField]
        private GameEvent _lobbyEnteredEvent;
        
        private bool _cancelled;
        private Coroutine _countdownCoroutine;
        private const string GameScenePath = "Assets/Scenes/GameScene.unity";

        private void Awake()
        {
            countdownPanel.SetActive(false);
            _cancelled = false;
            _lobbyEnteredEvent.Invoke();
        }

        public void OnStartClick()
        {
            if (!playerManager.AllPlayersReady())
            {
                playersNotReadyPanel.SetActive(true);
                return;
            }
            
            countdownPanel.SetActive(true);
            _cancelled = false; 
        
            if (_countdownCoroutine != null)
                StopCoroutine(_countdownCoroutine);
            
            _countdownCoroutine = StartCoroutine(StartCountdown());
        }

        private IEnumerator StartCountdown()
        {
            var countdown = countdownTimer;
            while (countdown > 0 && !_cancelled)
            {
                countdownText.text = countdown.ToString(CultureInfo.InvariantCulture);
                yield return new WaitForSeconds(1);
                countdown--;
            }

            countdownPanel.SetActive(false); 
            _cancelled = false;
            UpdateGameSettings();
            SceneManager.LoadScene(GameScenePath, LoadSceneMode.Single);
        }

        private void UpdateGameSettings()
        {
            var roundCount = int.Parse(numberOfRoundsDropdown.options[numberOfRoundsDropdown.value].text);
            var audienceEnabled = audienceEnabledToggle.isOn;
            var roundTimeLimit = int.Parse(roundTimerDropdown.options[roundTimerDropdown.value].text);
            
            gameSettings.UpdateGameSettings(roundCount, roundTimeLimit, audienceEnabled);
        }

        public void OnCancelClick(GameObject menu)
        {
            _cancelled = true;
            menu.SetActive(false); 
        }
    }
}