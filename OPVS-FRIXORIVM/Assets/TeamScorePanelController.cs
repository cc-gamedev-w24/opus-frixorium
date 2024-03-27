using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class TeamScorePanelController : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] TextMeshProUGUI[] teamScorePanels;

    public void SetTeamScore(int team, string score)
    {
        teamScorePanels[team - 1].text = score;
    }
}
