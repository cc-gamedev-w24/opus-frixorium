using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameState _gameState;
    
    void Start()
    {
        _gameState.StartGame();
    }

    void Update()
    {
        _gameState.Update();
    }
}
