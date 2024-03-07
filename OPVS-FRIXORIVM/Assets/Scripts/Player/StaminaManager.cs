using Events;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StaminaManager : MonoBehaviour
{
    // Start is called before the first frame update

    private DelegateGameEventListener _changeListener;
    private int _playerNumber;
    [SerializeField]
    private GameEvent _playerDataChangeEvent;
    [SerializeField]
    private Image _staminaFill;

    private Vector3 fillSize;

    private void OnPlayerDataChangedEvent(object value)
    {
        if (value is not PlayerData playerData)
        {
            Debug.LogError("Provided event data is wrong type");
            return;
        }

        fillSize.x = playerData.PlayerStamina / 100.0f;
        // Update UI
        _staminaFill.transform.localScale = fillSize;
        Debug.Log("new size: " + _staminaFill.transform.localScale);
    }
    void Awake()
    {
        _playerNumber = GetComponentInParent<Player>().PlayerData.PlayerNumber;
        _changeListener = new DelegateGameEventListener(_playerDataChangeEvent, OnPlayerDataChangedEvent, _playerNumber);
        fillSize = new Vector3(1.0f, 1.0f, 1.0f);
        Debug.Log("player: " + GetComponentInParent<Player>().PlayerData.PlayerNumber);
    }

    private void OnDestroy()
    {
        _changeListener.Dispose();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
