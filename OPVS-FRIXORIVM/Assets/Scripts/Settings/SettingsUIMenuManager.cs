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
        [SerializeField] private GameObject previousMenu;

        [Header("Graphics Settings")] 
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private Toggle fullscreenToggle;
    
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

        public void OnBackClick()
        {
            previousMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
