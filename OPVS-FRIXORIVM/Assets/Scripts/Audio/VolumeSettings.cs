using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider effectsSlider;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider voicesSlider;

    public const string MIXER_MASTER = "MasterVolume";
    public const string MIXER_EFFECTS = "EffectsVolume";
    public const string MIXER_MUSIC = "MusicVolume";
    public const string MIXER_VOICES = "VoicesVolume";

    private void Awake()
    {
        effectsSlider.onValueChanged.AddListener(SetEffectsVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        voicesSlider.onValueChanged.AddListener(SetVoicesVolume);
    }

    private void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat(AudioManager.MASTER_KEY, 1f);
        musicSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f);
        effectsSlider.value = PlayerPrefs.GetFloat(AudioManager.EFFECTS_KEY, 1f);
        voicesSlider.value = PlayerPrefs.GetFloat(AudioManager.VOICES_KEY, 1f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MASTER_KEY, masterSlider.value);
        PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(AudioManager.EFFECTS_KEY, effectsSlider.value);
        PlayerPrefs.SetFloat(AudioManager.VOICES_KEY, voicesSlider.value);
    }

    void SetEffectsVolume(float value)
    {
        mixer.SetFloat(MIXER_EFFECTS, Mathf.Log10(value) * 20);
    }

    void SetMusicVolume(float value)
    {
        mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }

    void SetVoicesVolume(float value)
    {
        mixer.SetFloat(MIXER_VOICES, Mathf.Log10(value) * 20);
    }

    void SetMasterVolume(float value)
    {
        mixer.SetFloat(MIXER_MASTER, Mathf.Log10(value) * 20);
    }
}
