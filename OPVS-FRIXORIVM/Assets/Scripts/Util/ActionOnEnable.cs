/*
 * File: ActionOnEnable.cs
 * Author: Silas Bartha
 * Last Modified: 2024-02-23
 */

using UnityEngine;
using UnityEngine.Events;

/// <summary>
///     Invokes a UnityEvent when this object is enabled
/// </summary>
public class ActionOnEnable : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onEnable;

    private void OnEnable()
    {
        _onEnable.Invoke();
    }
}
