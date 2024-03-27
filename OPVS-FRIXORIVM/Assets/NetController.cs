using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetController : MonoBehaviour
{
    private int _teamNumber;
    public int TeamNumber
    {
        get { return _teamNumber; }
        set { _teamNumber = value; }
    }

    public Color NetColor
    {
        get { return GetComponent<Renderer>().material.color; }
        set { GetComponent<Renderer>().material.color = value; }
    }

    [SerializeField]
    private GameEvent _soccerTrialAddScoreEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<BallController>(out _))
        {
            _soccerTrialAddScoreEvent.Invoke(GameEvent.GlobalChannel, _teamNumber);
            other.GetComponent<BallController>().RespawnBall();
        }
    }
}
