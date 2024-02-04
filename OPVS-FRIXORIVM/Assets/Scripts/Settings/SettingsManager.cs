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
        
        public void OnSaveChangesClick()
        {
            Screen.fullScreen = fullscreenToggle.isOn;
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
                Screen.SetResolution(width, height, Screen.fullScreen);
            }
        }
    }
}
