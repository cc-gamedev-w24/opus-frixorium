using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagePanning : MonoBehaviour
{
    public float speed = 2f; // Adjust the speed as needed
    public float amplitude = 1f; // Adjust the amplitude of the motion

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        float newX = originalPosition.x + Mathf.Sin(Time.time * speed) * amplitude;
        float newY = originalPosition.y + Mathf.Cos(Time.time * speed * 0.5f) * amplitude;
        transform.position = new Vector3(newX, newY, originalPosition.z);
    }
}
