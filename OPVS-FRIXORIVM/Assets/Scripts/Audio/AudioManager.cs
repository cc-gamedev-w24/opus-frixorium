using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private PlaySoundEventChannel _eventChannel;
    [SerializeField] private AudioSource _musicSource, _effectsSource;
    [SerializeField] private List<EffectsClipGroup> _clips;

    private Dictionary<string, EffectsClipGroup> _clipGroups;
    private void Awake()
    {
        _eventChannel.PlaySoundEvent += PlaySound;
        _clipGroups = new Dictionary<string, EffectsClipGroup>();
        foreach (EffectsClipGroup clipGroup in _clips)
        {
            _clipGroups.Add(clipGroup._name, clipGroup);
        }
    }

    private void OnDestroy()
    {
        _eventChannel.PlaySoundEvent -= PlaySound;
    }



    public void PlaySound(string clip)
    {
        if (_clipGroups.ContainsKey(clip))
        {
            int clipCount = _clipGroups[clip]._clips.Count;
            int clipIndex = UnityEngine.Random.Range(0, clipCount);
            _effectsSource.PlayOneShot(_clipGroups[clip]._clips[clipIndex]);
        }
    }

    public void PlayBackgroundMusic(AudioClip Music)
    {
        _musicSource.PlayOneShot(Music);
    }
    
}

[Serializable]
public class EffectsClipGroup
{
    public string _name;
    public List<AudioClip> _clips;
}