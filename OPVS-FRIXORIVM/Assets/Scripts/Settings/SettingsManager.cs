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

        private void Start()
        {
            // Check PlayerPrefs and apply settings 
            // Graphics settings
            InitGraphicsSettings();
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
            
            var vSync = PlayerPrefs.GetInt("VSync");
            QualitySettings.vSyncCount = vSync == 1 ? 1 : 0;
            vSyncToggle.isOn = vSync == 1;
        }

        private void UpdatePlayerPrefs()
        {
            // Graphics Settings
            PlayerPrefs.SetInt("GraphicsQuality", graphicsQualityDropdown.value);
            PlayerPrefs.SetInt("VSync", vSyncToggle.isOn ? 1 : 0);
            
            // TODO Volume Settings & Input Settings
        }
    }
}
