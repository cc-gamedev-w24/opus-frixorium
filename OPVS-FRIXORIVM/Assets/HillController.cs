using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HillController : MonoBehaviour
{

    private float[] players;
    // Start is called before the first frame update

    [SerializeField]
    private GameEvent _trialAddScoreEvent;

    void Start()
    {
        players = new float[4];
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<PlayerMovement>() != null)
        {
            var playerData = other.GetComponentInParent<Player>().PlayerData;

            if (players[playerData.PlayerNumber] >= 1.0f)
            {
                playerData.PlayerScore += 5;
                _trialAddScoreEvent.Invoke(GameEvent.GlobalChannel, playerData.PlayerNumber);
                players[playerData.PlayerNumber] = 0.0f;
            }
            else
            {
                players[playerData.PlayerNumber] += Time.deltaTime;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out _))
        {
            var playerData = other.GetComponentInParent<Player>().PlayerData;
            players[playerData.PlayerNumber] = 0.0f;
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
