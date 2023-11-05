using System;
using UnityEditor.EditorTools;
using UnityEngine;
namespace StateManchineTemplate
{
    [Serializable]
    public class DefaultColliderData
    {
        [field: Header("Default Values of the Capsule Collider")]
        [field: Tooltip("The height of the Capsule Collider")]
        [field: SerializeField]
        public float Height { get; private set; } = 1.8f;

        [field: Tooltip("The Center Y of the capsule Collider ")]
        [field: SerializeField]
        public float CenterY { get; private set; } = 0.9f;

        [field: Tooltip("The radius of the Capsule Collider")]
        [field: SerializeField]
        public float Radius { get; private set; } = 0.2f;
    }
}