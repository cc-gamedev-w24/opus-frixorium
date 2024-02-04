using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class SettingsUIMenuManager : MonoBehaviour
    {
        [Header("Menus")] 
        [SerializeField] private GameObject graphicsMenu;
        [SerializeField] private GameObject volumeMenu;
        [SerializeField] private GameObject controlsMenu;

        [Header("Graphics Settings")] 
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Toggle vSyncToggle;
    
        private GameObject _currentMenu;

        private void Awake()
        {
            // Fill resolution dropdown
            var options = Screen.resolutions.Select(resolution => resolution.ToString()).ToList();
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = FindCurrentResolution();
        
            // Check fullscreen
            fullscreenToggle.isOn = Screen.fullScreen;
        
            // Check vsync
            vSyncToggle.isOn = QualitySettings.vSyncCount != 0;
        
            // Default current menu to graphics
            _currentMenu = graphicsMenu;
        }

        private int FindCurrentResolution()
        {
            var currentResolution = Screen.currentResolution;

            for (var i = 0; i < resolutionDropdown.options.Count; i++)
            {
                if(resolutionDropdown.options[i].text != currentResolution.ToString()) 
                    continue;
                return i;
            }
            return -1;
        }
    
        private void DisableMenus()
        {
                graphicsMenu.SetActive(false);
                volumeMenu.SetActive(false);
                controlsMenu.SetActive(false);
        }

        public void ToggleMenu(GameObject menu)
        {
            if (_currentMenu == menu) 
                return;
        
            DisableMenus();
            menu.SetActive(true);
            _currentMenu = menu;
        }
    }
}
