using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OnScrollPickUp : MonoBehaviour
{
    public string playerTag = "Player";

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player

        if (other.CompareTag(playerTag))
        {
            Destroy(gameObject);
        }

    }

}
