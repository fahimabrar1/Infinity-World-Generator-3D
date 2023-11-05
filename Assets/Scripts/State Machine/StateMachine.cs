using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateManchineTemplate
{
    public abstract class StateMachine
    {
        /// <summary>
        /// The Current State
        /// </summary>
        protected IState currentState;


        /// <summary>
        ///     The State changes or initializes from one to another
        /// </summary>
        /// <param name="newState">new state to change to</param>
        public void ChangeState(IState newState)
        {
            currentState?.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }

        /// <summary>
        ///     Updates the Handle Input of the current state
        /// </summary>
        public void HandleInput()
        {
            currentState?.HandleInput();
        }

        /// <summary>
        ///     Runs the Non Physics Logic of the current state
        /// </summary>
        public void Update()
        {
            currentState?.Update();
        }


        /// <summary>
        ///     Runs the Physics Logic of the current state
        /// </summary>
        public void FixedsUpdate()
        {
            currentState?.FixedsUpdate();
        }

        /// <summary>
        /// When the player enters the animation this funtion is triggered
        /// </summary>
        public virtual void OnAnimationEnterEvent()
        {
            currentState?.OnAnimationEnterEvent();
        }

        /// <summary>
        /// When the player exits the animation this funtion is triggered
        /// </summary>
        public virtual void OnAnimationExitEvent()
        {
            currentState?.OnAnimationExitEvent();
        }

        /// <summary>
        /// Can be used to transition to differenct states after specific frames of the aniamtion
        /// </summary>
        public virtual void OnAnimationTransitionEvent()
        {
            currentState?.OnAnimationTransitionEvent();
        }
    }

}
