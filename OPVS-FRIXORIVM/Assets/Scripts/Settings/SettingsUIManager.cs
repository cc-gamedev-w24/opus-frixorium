using UnityEngine;
using UnityEngine.Events;

namespace Settings
{
    /// <summary>
    ///     UI handling for switching between settings menus
    /// </summary>
    public class SettingsUIManager : MonoBehaviour
    {
        /// <summary>
        ///     Available settings menus
        /// </summary>
        [Header("Menus")] 
        [SerializeField] private GameObject graphicsMenu;
        [SerializeField] private GameObject volumeMenu;
        [SerializeField] private GameObject controlsMenu;
        [SerializeField] private GameObject mainMenu;
    
        /// <summary>
        ///    Currently selected menu
        /// </summary>
        private GameObject _currentMenu;
        
        /// <summary>
        ///     Event called when save changes button is clicked
        /// </summary>
        public UnityEvent onSaveChangesClicked;

        private void Awake()
        {
            // Default current menu to graphics
            ToggleMenu(graphicsMenu);
        }
        
        /// <summary>
        ///     Triggered when a new menu is selected
        /// </summary>
        private void DisableMenus()
        {
            graphicsMenu.SetActive(false);
            volumeMenu.SetActive(false);
            controlsMenu.SetActive(false);
        }
        
        /// <summary>
        ///     Switches to the requested menu. Triggered on button click
        /// </summary>
        /// <param name="menu"> Requested menu </param>
        public void ToggleMenu(GameObject menu)
        {
            // is current menu the same as the requested menu?
            if (_currentMenu == menu) 
                return;
            
            // Disable all active menus and enable requested menu
            DisableMenus();
            menu.SetActive(true);
            _currentMenu = menu;
        }
        
        /// <summary>
        ///     Disables settings menu and returns to previous screen
        /// </summary>
        public void OnBackClick()
        {
            gameObject.SetActive(false);
            mainMenu.SetActive(true);
        }

        /// <summary>
        ///     Triggered when save changes button is clicked. Applies setting changes
        ///     See SettingsManager.cs
        /// </summary>
        public void SaveChangesButtonClicked()
        {
            onSaveChangesClicked?.Invoke();
        }
    }
}
