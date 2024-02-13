using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HitboxManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("x: " + (other.gameObject.transform.position.x - transform.position.x) + ", z: " + (other.gameObject.transform.position.z - transform.position.z));

        //other.attachedRigidbody.AddForce((other.gameObject.transform.position.x - transform.position.x) * 1000, 0, (other.gameObject.transform.position.z - transform.position.z) * 1000);

        //Vector3 ab = (other.gameObject.transform.position - transform.position).normalized;]
        if (other.TryGetComponent<PlayerMovement>(out _))
        {
            Debug.Log("player");
            other.GetComponent<CharacterController>().Move(transform.parent.forward);
        }
        else
        {
            Debug.Log("not player");
        }
    }

    private void Update()
    {
        //Debug.Log("x: " + transform.position.x + ", z: " + transform.position.z);
    }
}
