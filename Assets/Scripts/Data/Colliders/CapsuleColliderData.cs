using System;
using UnityEngine;
namespace StateManchineTemplate
{
    [Serializable]
    public class CapsuleColliderData
    {
        [Tooltip("The Capsule Collider")]
        public CapsuleCollider Collider { get; private set; }

        [Tooltip("The Collider Center Point in Local Space")]
        public Vector3 ColliderCenterInLocalSpace { get; private set; }



        /// <summary>
        /// Initialize the Varaibles
        /// </summary>
        /// <param name="gameObject">The Object from which we will get the collider component </param>
        public void Initialize(GameObject gameObject)
        {
            if (Collider != null) return;

            Collider = gameObject.GetComponent<CapsuleCollider>();

            UpdateColliderData();
        }

        /// <summary>
        /// Updates the Initial Center Point of the Collider
        /// </summary>
        public void UpdateColliderData()
        {
            ColliderCenterInLocalSpace = Collider.center;
        }
    }
}