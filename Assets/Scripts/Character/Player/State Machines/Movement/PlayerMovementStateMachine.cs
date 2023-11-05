using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace StateManchineTemplate
{
    public class PlayerMovementStateMachine : StateMachine
    {
        public Player Player { get; }
        public PlayerStateReusableData ReusableData { get; }
        public PlayerIdlingState IdlingState { get; }
        public PlayerWalkingState WalkingState { get; }
        public PlayerRunningState RunningState { get; }



        public PlayerMovementStateMachine(Player player)
        {
            Player = player;
            ReusableData = new();
            IdlingState = new(this);
            WalkingState = new(this);
            RunningState = new(this);
        }
    }

}
