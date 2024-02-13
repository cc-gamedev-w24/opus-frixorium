using System.Collections;
using System.Globalization;
using UnityEngine;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{

   [SerializeField] private TMP_Text countdownText;
   [SerializeField] private GameObject countdownPanel;
   
   private float _countdownTimer = 3.0f;

   public void Awake()
   {
      countdownPanel.SetActive(false);
   }
   
   public void OnStartClick()
   {
      countdownPanel.SetActive(true);
      StartCoroutine(StartCountdown());
   }

   private IEnumerator StartCountdown()
   {
      while (_countdownTimer > 0)
      {
         yield return new WaitForSeconds(1);
         _countdownTimer--;
         countdownText.text = _countdownTimer.ToString(CultureInfo.InvariantCulture);
      }
   }
}
