using System;
using UnityEngine;

public class Speen : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    void Update()
    {
        _transform.Rotate(Vector3.up, _speed * Time.deltaTime);
    }
}
