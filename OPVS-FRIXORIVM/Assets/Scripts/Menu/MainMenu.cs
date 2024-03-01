using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")] 
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject mainMenu;

    private const string PathToLobbyScene = "Assets/Scenes/LocalLobby.unity";
    
    public void StartGame()
    {
        SceneManager.LoadScene(PathToLobbyScene, LoadSceneMode.Single);
    }

    public void ShowSettings()
    {
        settingsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
}
