using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private PlaySoundEventChannel _eventChannel;
    [SerializeField] private AudioSource _musicSource, _effectsSource, _voicesSource;
    [SerializeField] private List<EffectsClipGroup> _clips;
    [SerializeField] AudioMixer mixer;

    private Dictionary<string, EffectsClipGroup> _clipGroups;

    public const string MASTER_KEY = "MasterVolume";
    public const string MUSIC_KEY = "MusicVolume";
    public const string EFFECTS_KEY = "EffectsVolume";
    public const string VOICES_KEY = "VoiceVolume";

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _eventChannel.PlaySoundEvent += PlaySound;
        _clipGroups = new Dictionary<string, EffectsClipGroup>();
        foreach (EffectsClipGroup clipGroup in _clips)
        {
            _clipGroups.Add(clipGroup._name, clipGroup);
        }

        LoadVolume();
    }

    private void OnDestroy()
    {
        _eventChannel.PlaySoundEvent -= PlaySound;
    }

    private void LoadVolume()       //Volume saved in VolumeSettings.cs
    {
        float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float effectsVolume = PlayerPrefs.GetFloat(EFFECTS_KEY, 1f);
        float voicesVolume = PlayerPrefs.GetFloat(VOICES_KEY, 1f);

        mixer.SetFloat(VolumeSettings.MIXER_MASTER, Mathf.Log10(masterVolume) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_EFFECTS, Mathf.Log10(effectsVolume) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_VOICES, Mathf.Log10(voicesVolume) * 20);

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