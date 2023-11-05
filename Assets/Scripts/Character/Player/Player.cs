using System.Collections;
using System.Collections.Generic;
using StateManchineTemplate;
using UnityEngine;
namespace StateManchineTemplate
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public PlayerSO Data { get; private set; }

        [field: Header("Collder")]
        [field: SerializeField] public CapsuleColliderUtility ColliderUtility { get; private set; }
        [field: SerializeField] public PlayerLayerData LayerData { get; private set; }

        [field: Header("Animations")]
        public Animator Animator { get; private set; }
        [field: SerializeField] public PlayerAnimationData PlayerAnimationData { get; private set; }


        [Tooltip("State Machine of the player containing all the states")]
        private PlayerMovementStateMachine PlayerMovementStateMachine;

        [Tooltip("Player Input is genetared from the new Input System")]
        public PlayerInput Input { get; private set; }


        [Tooltip("The Rigidbody of the player")]
        public Rigidbody Rigidbody { get; private set; }

        [Tooltip("The main camera transform")]
        public Transform mainCameraTransform { get; set; }


        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Input = GetComponent<PlayerInput>();
            Animator = GetComponentInChildren<Animator>();
            PlayerMovementStateMachine = new(this);
            mainCameraTransform = Camera.main.transform;
            ColliderUtility.Initialize(gameObject);
            ColliderUtility.CalculateCapsuleColliderDimensions();
            PlayerAnimationData.Initialize();
        }



        /// <summary>
        /// Called when the script is loaded or a value is changed in the
        /// inspector (Called in the editor only).
        /// </summary>
        void OnValidate()
        {
            ColliderUtility.Initialize(gameObject);
            ColliderUtility.CalculateCapsuleColliderDimensions();

        }

        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {
            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.IdlingState);
        }


        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            PlayerMovementStateMachine.HandleInput();
            PlayerMovementStateMachine.Update();
        }


        /// <summary>
        /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
        /// </summary>
        void FixedUpdate()
        {
            PlayerMovementStateMachine.FixedsUpdate();
        }
    }


}