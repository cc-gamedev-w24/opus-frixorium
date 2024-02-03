using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;

    private void Awake()
    {
        Instantiate(_playerPrefab, transform);
    }
}
