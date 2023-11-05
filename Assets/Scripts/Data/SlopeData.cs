using System;
using UnityEngine;
namespace StateManchineTemplate
{
    [Serializable]
    public class SlopeData
    {
        [field: Tooltip("The step height the capsule can climb")]
        [field: SerializeField]
        [field: Range(0, 1)]
        public float StepHeightPercentage { get; private set; } = 0.25f;

        [field: Tooltip("The Distance to where the raycast can hit")]
        [field: SerializeField]
        [field: Range(0, 5)]
        public float FloatRayDistance { get; private set; } = 1f;


        [field: Tooltip("The step height the capsule can climb")]
        [field: SerializeField]
        [field: Range(0, 50)]
        public float StepReachForce { get; private set; } = 25f;

    }
}