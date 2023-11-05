using System;
using UnityEngine;
namespace StateManchineTemplate
{

    [CreateAssetMenu(fileName = "PlayerSO", menuName = "Custom SO/Character/PlayerSO", order = 0)]
    [Serializable]
    public class PlayerSO : ScriptableObject
    {
        [field: SerializeField]
        public PlayerGroundedData GroundedData { get; private set; }

    }
}