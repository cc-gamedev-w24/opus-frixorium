using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour
{
    public float inclineDegrees = 23.4f;
    public float rotationSpeed = 30f;

    void Start()
    {

        transform.Rotate(0f, 0f, inclineDegrees);

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotationAxis = Vector3.up;

        // Calculate the rotation amount based on time and speed
        float rotationAngle = rotationSpeed * Time.deltaTime;

        // Rotate the object around the specified axis
        transform.RotateAround(transform.position, rotationAxis, rotationAngle);
    }
}
