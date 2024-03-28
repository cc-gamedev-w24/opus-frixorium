using System.Threading.Tasks;
using DG.Tweening;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _currentTrialText;

    [SerializeField]
    private GameEvent _trialStartEvent;

    [SerializeField]
    private GameManager _gameManager;

    [SerializeField]
    private GameSettings _gameSettings;

    [SerializeField]
    private Image _roundTimerBarImage;

    [Header("Round Start Popup")]
    [SerializeField]
    private CanvasGroup _roundStartPopupCanvasGroup;
    private TextMeshProUGUI _roundStartPopupTitleText;
    private TextMeshProUGUI _roundStartPopupDescriptionText;

    [SerializeField]
    private float _fadeInDuration = 1f;
    [SerializeField]
    private float _holdDuration = 1f;
    [SerializeField]
    private float _fadeOutDuration = 1f;

    private DelegateGameEventListener _listener;
    
    void Awake()
    {
        _listener = new DelegateGameEventListener(_trialStartEvent, UpdateTextOnEvent);

        _roundStartPopupTitleText = _roundStartPopupCanvasGroup.transform.Find("Title").GetComponent<TextMeshProUGUI>();
        _roundStartPopupDescriptionText = _roundStartPopupCanvasGroup.transform.Find("Description").GetComponent<TextMeshProUGUI>();
    }

    private void OnDestroy()
    {
        _listener.Dispose();
    }

    private void UpdateTextOnEvent(object data)
    {
        if (data is not Trial trial)
        {
            Debug.LogError("Wrong data type passed to ui event handler!");
            return;
        }
        _currentTrialText.text = trial.TrialName;
        _roundStartPopupTitleText.text = trial.TrialName;
        _roundStartPopupDescriptionText.text = trial.Description;

        _roundStartPopupCanvasGroup.DOFade(1f, _fadeInDuration).OnComplete(() => {
            Task.Delay(1000).ContinueWith(_ => _roundStartPopupCanvasGroup.DOFade(0f, _fadeOutDuration));
        });
    }

    private void Update()
    {
        _roundTimerBarImage.fillAmount = 1.0f - _gameManager.ElapsedTime / _gameSettings.RoundTimeLimit;
    }
}
