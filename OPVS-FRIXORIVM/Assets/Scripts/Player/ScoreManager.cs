using Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.PlayerInputManager;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private GameEvent _scoreChangeEvent;

    private DelegateGameEventListener _listener;

    void Awake()
    {
        _listener = new DelegateGameEventListener(_scoreChangeEvent, UpdateTextOnEvent);
    }

    private void UpdateTextOnEvent(object data)
    {
        if (data is not int[] scores)
        {
            Debug.LogError("Wrong data type passed to ui event handler!: ");
            return;
        }
        _text.text = scores[0].ToString();
    }
}
