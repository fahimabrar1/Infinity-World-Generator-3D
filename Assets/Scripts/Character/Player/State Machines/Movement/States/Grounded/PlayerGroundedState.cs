using System;
using System.Collections;
using System.Collections.Generic;
using StateManchineTemplate;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundedState : PlayerMovementState
{

    private SlopeData SlopeData;
    public PlayerGroundedState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
        SlopeData = stateMachine.Player.ColliderUtility.SlopeData;
    }


    #region IState Methods

    public override void OnEnter()
    {
        base.OnEnter();
        StartAniamtion(stateMachine.Player.PlayerAnimationData.GroundedAnimationHash);
    }



    public override void OnExit()
    {
        base.OnExit();
        StopAniamtion(stateMachine.Player.PlayerAnimationData.GroundedAnimationHash);
    }



    public override void FixedsUpdate()
    {
        base.FixedsUpdate();
        Float();
    }


    #endregion




    #region Main Methods


    protected virtual void OnMove()
    {
        if (stateMachine.ReusableData.ShouldWalk)
        {
            stateMachine.ChangeState(stateMachine.WalkingState);
            return;
        }

        stateMachine.ChangeState(stateMachine.RunningState);
    }

    private void Float()
    {
        Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;
        Ray downwardsRayFromCapsuleCollider = new(capsuleColliderCenterInWorldSpace, Vector3.down);
        if (Physics.Raycast(downwardsRayFromCapsuleCollider, out RaycastHit hit, SlopeData.FloatRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
        {
            float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCollider.direction);
            float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

            if (slopeSpeedModifier == 0) return;

            float distanceToFloatingPoint = stateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y * stateMachine.Player.transform.localScale.y - hit.distance;

            if (distanceToFloatingPoint == 0) return;

            float amountToLife = distanceToFloatingPoint * SlopeData.StepReachForce - GetPlayerVerticalVelocity().y;
            Debug.Log("Distance :" + distanceToFloatingPoint);
            Debug.Log("Lift :" + amountToLife);
            Vector3 liftForce = new(0, amountToLife, 0);
            stateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
        }
    }

    private float SetSlopeSpeedModifierOnAngle(float angle)
    {
        float slopeSpeedModifier = GroundedData.SlopeSpeedAngles.Evaluate(angle);
        stateMachine.ReusableData.MovementOnSlopeSpeedModifier = slopeSpeedModifier;
        return slopeSpeedModifier;
    }

    #endregion
    #region Input Methods

    protected override void AddInputActionCallbacks()
    {
        base.AddInputActionCallbacks();
        stateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;

    }



    protected override void RemoveInputActionCallbacks()
    {
        stateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;

    }


    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.IdlingState);
    }




    #endregion

}
