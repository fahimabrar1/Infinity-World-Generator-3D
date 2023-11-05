using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateManchineTemplate
{
    public interface IState
    {
        /// <summary>
        /// Runs whenever a state becomes the Current State
        /// </summary>
        public void OnEnter();

        /// <summary>
        /// Runs whenever a state becomes the Previous State
        /// </summary>
        public void OnExit();

        /// <summary>
        /// Runs Logic that Handles User Input on the state
        /// </summary>
        public void HandleInput();

        /// <summary>
        /// Runs Logic that is not Physics based on the state
        /// </summary>
        public void Update();

        /// <summary>
        /// Runs Logic that is Physics based on the state
        /// </summary>
        public void FixedsUpdate();

        /// <summary>
        /// When the player enters the animation this funtion is triggered
        /// </summary>
        public void OnAnimationEnterEvent();

        /// <summary>
        /// When the player exits the animation this funtion is triggered
        /// </summary>
        public void OnAnimationExitEvent();

        /// <summary>
        /// Can be used to transition to differenct states after specific frames of the aniamtion
        /// </summary>
        public void OnAnimationTransitionEvent();
    }

}
