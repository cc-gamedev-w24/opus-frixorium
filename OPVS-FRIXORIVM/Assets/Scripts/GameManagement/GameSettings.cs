using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("General Settings")] 
    public int numberOfRounds = 3;
    public float roundTimeLimit = 60.0f;
    public bool audienceEnabled = false;

    public void UpdateGameSettings(int roundCount, int roundTimer, bool isAudienceEnabled)
    {
        numberOfRounds = roundCount;
        audienceEnabled = isAudienceEnabled;
        roundTimeLimit = roundTimer;
    }
}
