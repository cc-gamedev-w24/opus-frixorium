using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameSettings gameSettings;
    
    private void Awake()
    {
        Debug.Log($"Audience Enabled: {gameSettings.audienceEnabled}\nRound Count: {gameSettings.numberOfRounds} Round Timer: {gameSettings.roundTimeLimit}");
    }
}
