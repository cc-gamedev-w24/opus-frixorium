using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private PlaySoundEventChannel _eventChannel;
    [SerializeField] private string _yaySound;
    [SerializeField] private string _gruntSound;

    public void OnYayClick()
    {
        _eventChannel.RaisePlaySoundEvent(_yaySound);
    }

    public void OnGruntClick()
    {
        _eventChannel.RaisePlaySoundEvent(_gruntSound);
    }
}
