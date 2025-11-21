using UnityEngine;

public abstract class TrapBase : MonoBehaviour
{
    private float WarmupTimeRemaining;

    // TODO: Change GameObject to a more specific type
    public abstract void Activate(TrapData TrapData, GameObject Other);

}
