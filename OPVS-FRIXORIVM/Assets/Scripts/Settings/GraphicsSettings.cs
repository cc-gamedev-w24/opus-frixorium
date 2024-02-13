using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    /// <summary>
    ///     Settings and PlayerPref handling
    /// </summary>
    public class GraphicsSettings : MonoBehaviour
    {
        [Header("Graphics Settings")] 
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private TMP_Dropdown graphicsQualityDropdown;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Toggle vSyncToggle;

        [Header("Managers")]
        [SerializeField] private SettingsUIManager menuManager;
        
        private Resolution[] _availableResolutions;
        
        private void OnEnable()
        {
            menuManager.onSaveChangesClicked.AddListener(ApplyChanges);
        }

        private void OnDisable()
        {
            menuManager.onSaveChangesClicked.RemoveListener(ApplyChanges);
        }

        private void Awake()
        {
            // Initialize settings from PlayerPrefs
            InitGraphicsSettings();
        }

        private void ApplyChanges()
        {
            ApplyGraphicsChanges();
            UpdatePlayerPrefs();
        }
        
        private void ApplyGraphicsChanges()
        {
            QualitySettings.vSyncCount = vSyncToggle.isOn ? 1 : 0;
            QualitySettings.SetQualityLevel(graphicsQualityDropdown.value, true);
            ApplyResolution();
        }
        
        /// <summary>
        ///     Applies the new resolution
        /// </summary>
        private void ApplyResolution()
        {
            // Is new resolution valid?
            if (resolutionDropdown.value >= 0 && resolutionDropdown.value < _availableResolutions.Length)
            {
                var resolution = _availableResolutions[resolutionDropdown.value];
                Screen.SetResolution(resolution.width, resolution.height, fullscreenToggle.isOn);
            }
        }
        
        /// <summary>
        ///     Initializes graphics settings and UI from PlayerPrefs
        /// </summary>
        private void InitGraphicsSettings()
        {
            // Init graphics quality
            var graphicsQuality = PlayerPrefs.GetInt("GraphicsQuality", 0);
            QualitySettings.SetQualityLevel(graphicsQuality, true);
            graphicsQualityDropdown.value = graphicsQuality;
            
            // Init VSync
            var vSync = PlayerPrefs.GetInt("VSync", 1);
            QualitySettings.vSyncCount = vSync;
            vSyncToggle.isOn = vSync == 1;
            
            // Init resolution
            _availableResolutions = Screen.resolutions;
            LoadResolutionsToDropdown();
            resolutionDropdown.value = FindCurrentResolutionIndex(Screen.currentResolution);
            
            // Init fullscreen
            fullscreenToggle.isOn = Screen.fullScreen;
        }
        
        /// <summary>
        ///     Initializes supported resolutions to dropdown
        /// </summary>
        private void LoadResolutionsToDropdown()
        {
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(_availableResolutions.Select(resolution => resolution.ToString()).ToList());
        }
        
        /// <summary>
        ///     Finds the current resolution index for dropdown initialization
        /// </summary>
        /// <param name="currentResolution"></param>
        /// <returns> Index of the current resolution </returns>
        private int FindCurrentResolutionIndex(Resolution currentResolution)
        {
            for (var i = 0; i < _availableResolutions.Length; i++)
            {
                // Is current index equal to the current resolution?
                if (_availableResolutions[i].Equals(currentResolution))
                {
                    return i;
                }
            }
            return -1;
        }
        
        /// <summary>
        ///     Saves new settings to PlayerPrefs for persistence
        /// </summary>
        private void UpdatePlayerPrefs()
        {
            // Update Graphics Settings
            PlayerPrefs.SetInt("GraphicsQuality", graphicsQualityDropdown.value);
            PlayerPrefs.SetInt("VSync", vSyncToggle.isOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
