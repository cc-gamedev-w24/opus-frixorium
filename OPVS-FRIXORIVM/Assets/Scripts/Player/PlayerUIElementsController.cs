using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIElementsController : MonoBehaviour
{
    [SerializeField] Canvas playerUI;

    // Update is called once per frame
    void Update()
    {
        playerUI.transform.rotation = Camera.main.transform.rotation;
    }
}
