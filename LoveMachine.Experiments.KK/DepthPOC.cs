using UnityEngine;

namespace LoveMachine.Experiments
{
    public class DepthPOC : MonoBehaviour
    {
        public bool IsDeviceConnected { get; protected set; }

        public float Depth { get; protected set; }
    }
}