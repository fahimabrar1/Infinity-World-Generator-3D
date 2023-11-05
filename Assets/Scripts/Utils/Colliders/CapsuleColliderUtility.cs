using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
namespace StateManchineTemplate
{
    [Serializable]
    public class CapsuleColliderUtility
    {
        public CapsuleColliderData CapsuleColliderData { get; private set; }
        [field: SerializeField] public DefaultColliderData DefaultColliderData { get; private set; }
        [field: SerializeField] public SlopeData SlopeData { get; private set; }



        public void Initialize(GameObject gameObject)
        {
            if (CapsuleColliderData != null) return;
            CapsuleColliderData = new();
            CapsuleColliderData.Initialize(gameObject);
        }

        /// <summary>
        /// Calculates the Capsule Radius, Height and Center
        /// </summary>
        public void CalculateCapsuleColliderDimensions()
        {
            SetCapsuleColliderRadius(DefaultColliderData.Radius);
            SetCapsuleColliderHeight(DefaultColliderData.Height * (1f - SlopeData.StepHeightPercentage));
            RecalcualteCapsuleCOlliderCenter();
            float halfColliderHeight = CapsuleColliderData.Collider.height / 2f;
            if (halfColliderHeight < CapsuleColliderData.Collider.radius)
            {
                SetCapsuleColliderRadius(halfColliderHeight);
            }

            CapsuleColliderData.UpdateColliderData();
        }

        /// <summary>
        /// Sets the Radius Data to collider
        /// </summary>
        /// <param name="radius">The Radius Value</param>
        private void SetCapsuleColliderRadius(float radius)
        {
            CapsuleColliderData.Collider.radius = radius;
        }

        /// <summary>
        /// Sets the Capsule Height Data to collider
        /// </summary>
        /// <param name="height">The Height Value</param>
        private void SetCapsuleColliderHeight(float height)
        {
            CapsuleColliderData.Collider.height = height;
        }

        /// <summary>
        /// 
        /// </summary>
        private void RecalcualteCapsuleCOlliderCenter()
        {
            float colliderHeightDifference = DefaultColliderData.Height - CapsuleColliderData.Collider.height;
            Vector3 newColliderCenter = new(0, DefaultColliderData.CenterY + (colliderHeightDifference / 2), 0f);
            CapsuleColliderData.Collider.center = newColliderCenter;
        }



    }
}