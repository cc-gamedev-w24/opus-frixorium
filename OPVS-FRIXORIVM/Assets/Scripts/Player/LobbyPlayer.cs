using System;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
///     Player controller while in lobby
/// </summary>
public class LobbyPlayer : PlayerController
{
    [SerializeField]
    private float _colorShiftSpeed = 0.25f;

    private float _shiftInput;
    
    private void OnReady()
    {
        PlayerData.IsReady = true;
    }

    private void OnUnready()
    {
        PlayerData.IsReady = false;
    }

    private void OnDropOut()
    {
        if (!PlayerData.IsReady)
            Destroy(transform.parent.gameObject);
    }

    private void OnShiftColor(InputValue value)
    {
        _shiftInput = value.Get<float>();
    }

    private void Update()
    {
        if (!PlayerData.IsReady && _shiftInput != 0f)
        {
            Color.RGBToHSV(PlayerData.Color, out var hue, out _, out _);
            PlayerData.Color = Color.HSVToRGB(hue + _shiftInput * _colorShiftSpeed * Time.deltaTime, 1f, 1f);
        }

    }

    protected override string ActionMap => "Lobby";
}
