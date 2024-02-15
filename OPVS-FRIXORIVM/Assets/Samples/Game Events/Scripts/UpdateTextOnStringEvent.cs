using TMPro;
using UnityEngine;

/// <summary>
///     Update TMP when given string event is raised
/// </summary>
public class UpdateTextOnStringEvent : GameEventListener
{
    [SerializeField]
    private TextMeshProUGUI _text;

    public override void OnEvent(object value)
    {
        _text.text = value as string;
    }
}
