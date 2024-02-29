using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : MonoBehaviour
{
    public enum WeaponType
    {
        Fists,
        Bladed,
        Blunt,
        Polearm,
        Ranged
    }

    
    
    
    
    

    /// <summary>
    ///     Device class of this player, e.g. "Keyboard" or "Gamepad"
    /// </summary>
    /// 
    [SerializeField] int _damage;
    public int Damage
    {
        get => _damage;
    }

    /// <summary>
    ///     Device class of this player, e.g. "Keyboard" or "Gamepad"
    /// </summary>
    /// 
    [SerializeField] int _staminaCost;
    public int StaminaCost
    {
        get => _staminaCost;
    }

    /// <summary>
    ///     Device class of this player, e.g. "Keyboard" or "Gamepad"
    /// </summary>
    /// 
    [SerializeField] float _cooldown;
    public float Cooldown
    {
        get => _cooldown;
    }

    /// <summary>
    ///     Device class of this player, e.g. "Keyboard" or "Gamepad"
    /// </summary>
    /// 
    [SerializeField] float _useTime;
    public float UseTime
    {
        get => _useTime;
    }

    /// <summary>
    ///     Device class of this player, e.g. "Keyboard" or "Gamepad"
    /// </summary>
    /// 
    [SerializeField] Vector3 _range;
    public Vector3 Range
    {
        get => _range;
    }

    /// <summary>
    ///     Device class of this player, e.g. "Keyboard" or "Gamepad"
    /// </summary>
    /// 
    [SerializeField] WeaponType _weaponType;
    public WeaponType Type
    {
        get => _weaponType;
    }
}