using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerOnscreenUI : MonoBehaviour
{
    public int PlayerId;

    [SerializeField]
    private GameObject _rootPanel;
    
    [SerializeField]
    private Image _iconImage;

    [SerializeField]
    private TextMeshProUGUI _scoreText;

    [SerializeField]
    private GameEvent _playerDataChangedEvent;

    private DelegateGameEventListener _listener;
    
    private void Awake()
    {
        _listener = new DelegateGameEventListener(_playerDataChangedEvent, OnPlayerDataChangedEvent, PlayerId);
        _rootPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        _listener.Dispose();
    }
    
    private void OnPlayerDataChangedEvent(object value)
    {
        if (value is not PlayerData playerData)
        {
            Debug.LogError("Provided event data is wrong type");
            return;
        }
        _rootPanel.SetActive(true);

        _iconImage.color = playerData.Color;
        _scoreText.text = $"{playerData.PlayerScore}";
    }
}
