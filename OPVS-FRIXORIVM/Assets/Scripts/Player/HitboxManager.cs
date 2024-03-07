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
            var otherPlayerData = other.GetComponentInParent<Player>().PlayerData;
            if (!otherPlayerData.PlayerBlocked)
            {
                var otherController = other.GetComponent<CharacterController>();
                var forward = transform.forward;
                otherController.Move(forward);
                otherController.SimpleMove(forward);
                
                otherPlayerData.PlayerHP -= GetComponentInParent<PlayerMovement>().EquippedWeapon.GetComponent<WeaponData>().Damage;

                otherPlayerData.PlayerHit = true;
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
