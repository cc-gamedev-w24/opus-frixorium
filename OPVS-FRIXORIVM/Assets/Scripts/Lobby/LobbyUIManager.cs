using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace Lobby
{
    public class LobbyUIManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private GameObject countdownPanel;
        [SerializeField] private float countdownTimer = 3.0f;

        private bool _cancelled;
        private Coroutine _countdownCoroutine;

        public void Awake()
        {
            countdownPanel.SetActive(false);
            _cancelled = false;
        }

        public void OnStartClick()
        {
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
        }

        public void OnCancelClick()
        {
            _cancelled = true;
            countdownPanel.SetActive(false); 
        }
    }
}