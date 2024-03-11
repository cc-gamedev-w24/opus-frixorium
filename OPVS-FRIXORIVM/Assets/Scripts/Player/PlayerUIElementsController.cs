using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerUIElementsController : MonoBehaviour
{
    [SerializeField] Canvas playerUI;
    [SerializeField]
    private Transform _trackedTransform;

    private Vector3 _offset;

    private void Start()
    {
        _offset = playerUI.transform.position - _trackedTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        playerUI.transform.position = _offset + _trackedTransform.position;
        playerUI.transform.rotation = Camera.main.transform.rotation;
    }
}
