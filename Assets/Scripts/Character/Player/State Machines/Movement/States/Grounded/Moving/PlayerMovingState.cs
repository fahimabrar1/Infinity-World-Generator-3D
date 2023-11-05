using System.Collections;
using System.Collections.Generic;
using StateManchineTemplate;
using UnityEngine;

public class PlayerMovingState : PlayerGroundedState
{
    public PlayerMovingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
    {
    }

    #region IState Methods

    public override void OnEnter()
    {
        base.OnEnter();
        StartAniamtion(stateMachine.Player.PlayerAnimationData.IsMovingAnimationHash);
    }



    public override void OnExit()
    {
        base.OnExit();
        StopAniamtion(stateMachine.Player.PlayerAnimationData.IsMovingAnimationHash);
    }
    #endregion
}
