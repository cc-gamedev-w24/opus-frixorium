using Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.PlayerInputManager;

public class HealthManager : MonoBehaviour
{
    // Start is called before the first frame update

    private DelegateGameEventListener _changeListener;
    private int _playerNumber;
    [SerializeField]
    private GameEvent _playerDataChangeEvent;
    [SerializeField]
    private Image _healthFill;

    private Vector3 fillSize;

    void Start()
    {
        _playerNumber = GetComponentInParent<Player>().PlayerData.PlayerNumber;
        fillSize = new Vector3(1.0f, 1.0f, 1.0f);
    }

    private void OnPlayerDataChangedEvent(object value)
    {
        if (value is not PlayerData playerData)
        {
            Debug.LogError("Provided event data is wrong type");
            return;
        }
        fillSize.x = playerData.PlayerHP / 100.0f;
        // Update UI
        _healthFill.transform.localScale = fillSize;
    }
    void Awake()
    {
        _changeListener = new DelegateGameEventListener(_playerDataChangeEvent, OnPlayerDataChangedEvent, _playerNumber);
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
