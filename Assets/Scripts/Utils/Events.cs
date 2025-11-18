using System;
using UnityEngine;

public class Events : MonoBehaviour
{
    public static class Gameplay
    {
        public static Action<bool> ToggleTrapPlacementEvent;
        public static void BroadcastToggleTrapPlacementEvent(bool NewState)
        {
            ToggleTrapPlacementEvent?.Invoke(NewState);
        }
    }
}
