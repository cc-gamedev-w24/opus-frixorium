using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class PlayerColorize: MonoBehaviour
{
    private void Start()
    {
        var playerData = GetComponentInParent<Player>().PlayerData;
        var meshRenderer = GetComponent<Renderer>();
        meshRenderer.material.color = playerData.Color;
    }
}
