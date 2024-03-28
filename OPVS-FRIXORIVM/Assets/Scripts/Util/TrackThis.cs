/*
 * Author: Silas Bartha
 * Last Modified: 2024-03-17
 */

using Cinemachine;
using UnityEngine;

public class TrackThis : MonoBehaviour
{
    private bool _tracked;
    private void Update()
    {
        if (_tracked) return;
        var targetGroup = Camera.main.GetComponentInChildren<CinemachineTargetGroup>();
        if(targetGroup == null) return;
        targetGroup.AddMember(transform, 1, 5);
        _tracked = true;
    }
}
