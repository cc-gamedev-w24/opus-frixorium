using System.Collections;
using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine;

public class TrialTimerUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    [SerializeField]
    private GameEvent _timerChangeEvent;

    private DelegateGameEventListener _listener;

    void Awake()
    {
        _listener = new DelegateGameEventListener(_timerChangeEvent, UpdateTextOnEvent);
    }

    private void UpdateTextOnEvent(object data)
    {
        if (data is not int time)
        {
            Debug.LogError("Wrong data type passed to ui event handler!");
            return;
        }
        _text.text = time.ToString();
    }
}
