using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateManchineTemplate
{

    public class PlayerRunningState : PlayerMovingState
    {
        public PlayerRunningState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        #region IStates Methods

        public override void OnEnter()
        {
            base.OnEnter();
            StartAniamtion(stateMachine.Player.PlayerAnimationData.IsRunningAnimationHash);

            stateMachine.ReusableData.MovementSpeedModifier = GroundedData.RunData.SpeedModifier;
        }





        public override void OnExit()
        {
            base.OnExit();
            StopAniamtion(stateMachine.Player.PlayerAnimationData.IsRunningAnimationHash);
        }


        #endregion

        #region Input Methods

        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            stateMachine.ChangeState(stateMachine.WalkingState);
        }



        #endregion
    }
}
