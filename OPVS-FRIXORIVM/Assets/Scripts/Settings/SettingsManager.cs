using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class SettingsManager : MonoBehaviour
    {
        [Header("Graphics Settings")] 
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown graphicsQualityDropdown;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Toggle vSyncToggle;

        [Header("Volume Settings")] 
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider voiceVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;

        private void Start()
        {
            // Initialize settings from PlayerPrefs
            InitGraphicsSettings();
            InitVolumeSettings();
        }

        public void OnSaveChangesClick()
        {
            ApplyGraphicsChanges();
            UpdatePlayerPrefs();
        }

        private void ApplyGraphicsChanges()
        {
            QualitySettings.vSyncCount = vSyncToggle == true ? 1 : 0;
            QualitySettings.SetQualityLevel(graphicsQualityDropdown.value, true);
            ApplyResolution();
        }

        private void ApplyResolution()
        {
            var selectedResolution = resolutionDropdown.options[resolutionDropdown.value].text;

            // Split the resolution string into width and height parts
            var resolutionParts = selectedResolution.Split('x');

            // Ensure that we have at least two parts and try to parse the width and height
            if (resolutionParts.Length >= 2 && int.TryParse(resolutionParts[0], out var width) && int.TryParse(resolutionParts[1].Split('@')[0], out var height))
            {
                // Apply the selected resolution
                Screen.SetResolution(width, height, fullscreenToggle.isOn);
            }
        }

        private void InitGraphicsSettings()
        {
            var graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", 0);
            QualitySettings.SetQualityLevel(graphicsQuality, true);
            graphicsQualityDropdown.value = graphicsQuality;
            
            var vSync = PlayerPrefs.GetInt("VSync", 1);
            QualitySettings.vSyncCount = vSync == 1 ? 1 : 0;
            vSyncToggle.isOn = vSync == 1;
        }

        private void InitVolumeSettings()
        {
            // Load volume settings from PlayerPrefs and set slider values
            // TODO: Implement volume changes when AudioManager is ready
            masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 100);
            sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 100);
            voiceVolumeSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 100);
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 100);
        }

        private void UpdatePlayerPrefs()
        {
            // Update Graphics Settings
            PlayerPrefs.SetInt("GraphicsQuality", graphicsQualityDropdown.value);
            PlayerPrefs.SetInt("VSync", vSyncToggle.isOn ? 1 : 0);
            
            // Update Volume Settings
            PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
            PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
            PlayerPrefs.SetFloat("VoiceVolume", voiceVolumeSlider.value);
            PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
            
            // TODO: Handle Input Settings
            
            PlayerPrefs.Save();
        }
    }
}
