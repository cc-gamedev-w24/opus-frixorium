using UnityEngine;

public class TriggerPredicate: MonoBehaviour, IPredicate
{
    private bool _triggered;

    private void LateUpdate()
    {
        _triggered = false;
    }

    public bool Evaluate() => _triggered;

    public void Trigger()
    {
        _triggered = true;
    }
}
