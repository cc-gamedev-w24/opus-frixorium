using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIElementsController : MonoBehaviour
{
    [SerializeField] Canvas playerUI;
    [SerializeField]
    private Transform _headTransform;

    private Vector3 _offset;

    private void Start()
    {
        _offset = playerUI.transform.position - _headTransform.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerUI.transform.position = _offset + _headTransform.position;
        playerUI.transform.rotation = Camera.main.transform.rotation;
    }
}
