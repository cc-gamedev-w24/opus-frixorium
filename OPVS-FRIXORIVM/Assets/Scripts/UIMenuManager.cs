using UnityEngine;

public class UIMenuManager : MonoBehaviour
{
    [Header("Menus")] 
    [SerializeField] private GameObject graphicsMenu;
    [SerializeField] private GameObject volumeMenu;
    [SerializeField] private GameObject controlsMenu;

    private GameObject _currentMenu;

    private void Awake()
    {
        _currentMenu = graphicsMenu;
    }
    
    private void DisableMenus()
    {
        graphicsMenu.SetActive(false);
        volumeMenu.SetActive(false);
        controlsMenu.SetActive(false);
    }

    public void OnGraphicsClick()
    {
        if (_currentMenu == graphicsMenu) return;
        
        DisableMenus();
        graphicsMenu.SetActive(true);
        _currentMenu = graphicsMenu;
    } 
    
    public void OnVolumeClick()
    {
        if (_currentMenu == volumeMenu) return;
        
        DisableMenus();
        volumeMenu.SetActive(true);
        _currentMenu = volumeMenu;
    } 
    
    public void OnControlsClick()
    {
        if (_currentMenu == controlsMenu) return;
        
        DisableMenus();
        controlsMenu.SetActive(true);
        _currentMenu = controlsMenu;
    } 
}
