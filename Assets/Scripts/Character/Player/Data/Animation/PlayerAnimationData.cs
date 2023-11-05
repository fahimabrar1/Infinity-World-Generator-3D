using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerAnimationData
{

    [Header("State Group parameter names")]
    [SerializeField] private string isGroundedState = "isGrounded";
    [SerializeField] private string isMovingState = "isMoving";


    [Header("Movement parameter names")]
    [SerializeField] private string isWalkingState = "isWalking";
    [SerializeField] private string isRunningState = "isRunning";


    public int GroundedAnimationHash { get; private set; }
    public int IsMovingAnimationHash { get; private set; }
    public int IsWalkingAnimationHash { get; private set; }
    public int IsRunningAnimationHash { get; private set; }


    public void Initialize()
    {
        GroundedAnimationHash = Animator.StringToHash(isGroundedState);
        IsMovingAnimationHash = Animator.StringToHash(isMovingState);
        IsWalkingAnimationHash = Animator.StringToHash(isWalkingState);
        IsRunningAnimationHash = Animator.StringToHash(isRunningState);
    }
}
