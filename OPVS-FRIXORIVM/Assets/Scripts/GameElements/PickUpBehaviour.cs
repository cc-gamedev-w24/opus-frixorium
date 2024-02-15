using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpBehaviour : MonoBehaviour
{
    public float inclineDegrees = 23.4f;
    public float rotationSpeed = 30f;
    public MonoBehaviour actionScript;
    private string scriptClassName;

    void Start()
    {
        transform.Rotate(0f, 0f, inclineDegrees);
        // Check if the otherScript is assigned before calling its method
        if (actionScript != null)
        {
            // Call the method of the otherScript
            scriptClassName = actionScript.GetType().Name;


        }
   
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (actionScript != null)
            {
                // Call a method from ScriptB when an event occurs in ScriptA
                actionScript.GetType().GetMethod("ItemPickedUp").Invoke(actionScript, null);
            }
            Destroy(gameObject);
        }
    }
}
