using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.PlayerLoop;

namespace StateManchineTemplate
{
    public class PlayerMovementState : IState
    {
        // Ref of the player state machine
        protected PlayerMovementStateMachine stateMachine;

        // Player speed
        protected PlayerGroundedData GroundedData;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="playerMovementStateMachine">The State machine</param>
        public PlayerMovementState(PlayerMovementStateMachine playerMovementStateMachine)
        {
            stateMachine = playerMovementStateMachine;
            GroundedData = stateMachine.Player.Data.GroundedData;
            InitializeData();
        }

        private void InitializeData()
        {
            stateMachine.ReusableData.TimeToReachTargetRotation = GroundedData.BaseRotationData.TargetRotationReachTime;
        }

        #region  IState Methids



        /// <summary>
        /// Handles the player input data
        /// </summary>
        public virtual void HandleInput()
        {
            ReadPlayerInput();
        }

        /// <summary>
        ///  On Enter This State
        /// </summary>
        public virtual void OnEnter()
        {

            Debug.Log($"State:{GetType().Name}");
            AddInputActionCallbacks();
        }


        /// <summary>
        ///  On Exit This State
        /// </summary>
        public virtual void OnExit()
        {
            RemoveInputActionCallbacks();
        }


        /// <summary>
        /// Fixed Update For Physics Based calculations
        /// </summary>
        public virtual void FixedsUpdate()
        {
            Move();
        }


        /// <summary>
        /// Update For Non-Physics Based calculations
        /// </summary>
        public virtual void Update()
        {
        }

        /// <summary>
        /// When the player enters the animation this funtion is triggered
        /// </summary>
        public virtual void OnAnimationEnterEvent()
        {
        }

        /// <summary>
        /// When the player exits the animation this funtion is triggered
        /// </summary>
        public virtual void OnAnimationExitEvent()
        {
        }

        /// <summary>
        /// Can be used to transition to differenct states after specific frames of the aniamtion
        /// </summary>
        public virtual void OnAnimationTransitionEvent()
        {
        }

        #endregion

        #region  Main Funttions

        /// <summary>
        /// Starts the animation hash
        /// </summary>
        protected void StartAniamtion(int animationHash)
        {
            stateMachine.Player.Animator.SetBool(animationHash, true);
        }

        /// <summary>
        /// Stops the animation hash
        /// </summary>
        protected void StopAniamtion(int animationHash)
        {
            stateMachine.Player.Animator.SetBool(animationHash, false);
        }

        /// <summary>
        /// Reads Player Input
        /// </summary>
        private void ReadPlayerInput()
        {
            stateMachine.ReusableData.MovementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
        }


        /// <summary>
        /// Moves the player to player input direction 
        /// * It also Rotates the Player to direction of the player input
        /// </summary>
        public void Move()
        {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero || stateMachine.ReusableData.MovementSpeedModifier == 0)
                return;

            // gets the move direction of the player
            Vector3 movementDirection = GetMovementDirection();

            // Gets the rotation in Yangle of the player
            float targetRotationYAngle = Rotate(movementDirection);
            Vector3 targetRotationDirection = Quaternion.Euler(0, targetRotationYAngle, 0) * Vector3.forward;

            //gets the move speed of the player
            float movementSpeed = GetMovementSpeed();

            //calculates the horizontal velocity of the player
            Vector3 playerCurrentHorizontalVelocity = GetHorizontalVelocity();

            // adds force to the direction
            //* VelocityChange is Time Independent and Mass Indepenet, which helps to perform action instantly.
            stateMachine.Player.Rigidbody.AddForce(targetRotationDirection * movementSpeed - playerCurrentHorizontalVelocity, ForceMode.VelocityChange);
        }



        #endregion


        #region Reusable Methods

        /// <summary>
        /// Gets the player movement direction from 2D vector to 3D Vector
        /// </summary>
        /// <returns>The Move Direction in Vec3t</returns>
        protected Vector3 GetMovementDirection()
        {
            return new Vector3(stateMachine.ReusableData.MovementInput.x, 0, stateMachine.ReusableData.MovementInput.y);
        }

        /// <summary>
        ///  The Movement Speed with the speed modifier
        /// </summary>
        /// <returns>Final Speed</returns>
        protected float GetMovementSpeed()
        {
            return GroundedData.BaseSpeed * stateMachine.ReusableData.MovementSpeedModifier * stateMachine.ReusableData.MovementOnSlopeSpeedModifier;
        }


        /// <summary>
        /// Eleminates the Vertical Velocity
        /// </summary>
        /// <returns>Thhe Player Velocity</returns>
        protected Vector3 GetHorizontalVelocity()
        {
            Vector3 playerVlocity = stateMachine.Player.Rigidbody.velocity;
            playerVlocity.y = 0;
            return playerVlocity;
        }

        /// <summary>
        ///  Gets the player vertical velocity  
        ///                  
        /// </summary>
        /// <returns>The Player Vertical Velocity</returns>
        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new(0, stateMachine.Player.Rigidbody.velocity.y, 0);
        }



        /// <summary>
        /// Rotates the Player along with the camera angle
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private float Rotate(Vector3 direction)
        {
            // as UNITY DEGREE CIRCLE is not same to NORMAL DEGREE CIRCLE, we get the correct directions just by simply swaping the value in Atan2(y,x)
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // if degree is negative, we make it positive
            if (directionAngle < 0f) directionAngle += 360f;

            // Updates with the gets the camera y angle
            directionAngle += stateMachine.Player.mainCameraTransform.eulerAngles.y;

            // if degree is more then 360, we make it less then 360
            if (directionAngle > 360) directionAngle -= 360;

            // updates the current Y angle with the main camera Y angle if not same
            if (directionAngle != stateMachine.ReusableData.CurrentTargetRotation.y) UpdateTargetRotationData(directionAngle);

            // Rotates along the camera
            RotateTowardsTargetRotation();
            return directionAngle;
        }


        /// <summary>
        /// Updates the camera current target rotation variable for smooth damping
        /// </summary>
        /// <param name="targetRotation">the direction angle</param>
        private void UpdateTargetRotationData(float targetRotation)
        {
            stateMachine.ReusableData.CurrentTargetRotation.y = targetRotation;
            stateMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f;
        }



        /// <summary>
        /// rotates the character with the player input
        /// </summary>
        private void RotateTowardsTargetRotation()
        {
            float currentYAngle = stateMachine.Player.Rigidbody.rotation.eulerAngles.y;

            if (currentYAngle == stateMachine.ReusableData.CurrentTargetRotation.y) return;

            float smoothYAngle = Mathf.SmoothDampAngle(currentYAngle, stateMachine.ReusableData.CurrentTargetRotation.y, ref stateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y, stateMachine.ReusableData.TimeToReachTargetRotation.y - stateMachine.ReusableData.DampedTargetRotationPassedTime.y);
            stateMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(0, smoothYAngle, 0);
            stateMachine.Player.Rigidbody.MoveRotation(targetRotation);
        }



        protected void ResetVelocity()
        {
            stateMachine.Player.Rigidbody.velocity = Vector3.zero;
        }




        #endregion


        #region Input Methods


        // Todo: The walk and running control should be revised


        protected virtual void AddInputActionCallbacks()
        {
            stateMachine.Player.Input.PlayerActions.WalkToggle.started += OnWalkToggleStarted;
        }


        protected virtual void RemoveInputActionCallbacks()
        {
            stateMachine.Player.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;
        }


        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            stateMachine.ReusableData.ShouldWalk = !stateMachine.ReusableData.ShouldWalk;
        }


        #endregion
    }
}
