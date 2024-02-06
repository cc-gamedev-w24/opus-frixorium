using UnityEngine;

namespace Settings
{
    public class SettingsUIMenuManager : MonoBehaviour
    {
        [Header("Menus")] 
        [SerializeField] private GameObject graphicsMenu;
        [SerializeField] private GameObject volumeMenu;
        [SerializeField] private GameObject controlsMenu;
        [SerializeField] private GameObject previousMenu;
    
        private GameObject _currentMenu;

        private void Awake()
        {
            // Default current menu to graphics
            _currentMenu = graphicsMenu;
        }
    
        private void DisableMenus()
        {
            graphicsMenu.SetActive(false);
            volumeMenu.SetActive(false);
            controlsMenu.SetActive(false);
        }

        public void ToggleMenu(GameObject menu)
        {
            // is current menu the same as the requested menu?
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
