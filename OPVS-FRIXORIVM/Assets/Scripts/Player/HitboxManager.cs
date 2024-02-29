using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class HitboxManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out _))
        {
            if (!other.GetComponentInParent<Player>().PlayerData.PlayerBlocked)
            {
                other.GetComponent<CharacterController>().Move(transform.parent.forward);
                other.GetComponentInParent<Player>().PlayerData.PlayerHP -= GetComponentInParent<PlayerMovement>().EquippedWeapon.GetComponent<WeaponData>().Damage;

                other.GetComponentInParent<Player>().PlayerData.PlayerHit = true;
                Debug.Log("Hitbox: " + GetComponentInParent<PlayerMovement>().EquippedWeapon.GetComponent<WeaponData>().Damage);
            }
            else
            {
                GetComponentInParent<Player>().PlayerData.PlayerStamina = 0;
            }
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
