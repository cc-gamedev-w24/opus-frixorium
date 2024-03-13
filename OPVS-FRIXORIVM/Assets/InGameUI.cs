using System.Collections;
using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private GameEvent _trialStartEvent;

    private DelegateGameEventListener _listener;
    
    void Awake()
    {
        _listener = new DelegateGameEventListener(_trialStartEvent, UpdateTextOnEvent);
    }

    private void UpdateTextOnEvent(object data)
    {
        if (data is not Trial trial)
        {
            Debug.LogError("Wrong data type passed to ui event handler!");
            return;
        }
        _text.text = trial.name;
    }
}
