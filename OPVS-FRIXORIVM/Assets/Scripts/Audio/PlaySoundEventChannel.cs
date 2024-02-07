using UnityEngine;
[CreateAssetMenu(menuName = "EventChannels/Play Sound Event Channel")]

public class PlaySoundEventChannel : ScriptableObject
{
    public delegate void OnPlaySound(string clip);

    public event OnPlaySound PlaySoundEvent;

    public void RaisePlaySoundEvent(string clip)
    {
        if (PlaySoundEvent != null)
        {
            PlaySoundEvent.Invoke(clip);
        }
    }

}
