using UnityEngine;

public class BearTrap : TrapBase
{
    public override void Activate(TrapData TrapData, GameObject Other)
    {
        if (!Other.CompareTag("Player"))
        {
            Debug.Log("Bear trap triggering!");
            Destroy(Other);
        }
    }

    void Start()
    {
        // 
    }

    void Update()
    {

    }
}
