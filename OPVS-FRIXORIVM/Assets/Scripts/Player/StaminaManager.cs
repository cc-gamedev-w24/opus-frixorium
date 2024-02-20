using Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaminaManager : MonoBehaviour
{
    // Start is called before the first frame update

    private DelegateGameEventListener _changeListener;
    [SerializeField]
    private int _playerNumber;
    [SerializeField]
    private GameEvent _playerDataChangeEvent;
    [SerializeField]
    private TextMeshProUGUI _staminaText;



    private void OnPlayerDataChangedEvent(object value)
    {
        if (value is not PlayerData playerData)
        {
            Debug.LogError("Provided event data is wrong type");
            return;
        }

        // Update UI
        _staminaText.text = playerData.PlayerStamina.ToString();
    }
    void Awake()
    {
        _changeListener = new DelegateGameEventListener(_playerDataChangeEvent, OnPlayerDataChangedEvent, _playerNumber);
    }

    private void OnDestroy()
    {
        _changeListener.Dispose();
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
