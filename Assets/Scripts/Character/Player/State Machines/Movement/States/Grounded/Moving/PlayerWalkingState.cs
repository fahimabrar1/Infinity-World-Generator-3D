using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace StateManchineTemplate
{
    public class PlayerWalkingState : PlayerMovingState
    {
        public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }


        #region IStates Methods



        public override void OnEnter()
        {
            base.OnEnter();
            StartAniamtion(stateMachine.Player.PlayerAnimationData.IsWalkingAnimationHash);
            stateMachine.ReusableData.MovementSpeedModifier = GroundedData.WalkData.SpeedModifier;
        }



        public override void OnExit()
        {
            base.OnExit();
            StopAniamtion(stateMachine.Player.PlayerAnimationData.IsWalkingAnimationHash);
        }


        #endregion

        #region Input Methods


        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            stateMachine.ChangeState(stateMachine.RunningState);
        }



        #endregion

    }
}
