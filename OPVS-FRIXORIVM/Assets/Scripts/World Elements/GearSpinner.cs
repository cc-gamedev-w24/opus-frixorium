using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;

public class GearSpinner : MonoBehaviour
{
    public float spinSpeed = 50f; // Adjust this value to change the spinning speed
    public string direction = "up";
    private Vector3 axis;
    // Update is called once per frame

    private void Start()
    {
        switch (direction)
        {
            case "up":
                axis = Vector3.up;
                break;
            case "down":
                axis = Vector3.down;
                break;
            case "left":
                axis = Vector3.left;
                break;
            case "right":
                axis = Vector3.right;
                break;
            case "back":
                axis = Vector3.back;
                break;
            default:
                // If direction is not recognized, default to spinning around the up axis
                axis = Vector3.up;
                break;
        }

    }
    void Update()
    {
        // Spin the object around the x-axis
        transform.Rotate(axis, spinSpeed * Time.deltaTime);
    }
}
