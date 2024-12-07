using System.Net.Sockets;
using UnityEngine;
using UnityEngine.Rendering;

namespace FT
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;

        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float moveAmount;

        [Header("Movement Settings")]
        private Vector3 moveDirection;
        private Vector3 TargetRotationDirection;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runningSpeed = 5;
        [SerializeField] float sprintingSpeed = 6;
        [SerializeField] float rotationSpeed = 15;
        [SerializeField] int sprintingStaminaCost = 2;

        [Header("Jump")]
        [SerializeField] float jumpHeight = 4;
        [SerializeField] float jumpStaminaCost = 15;
        private Vector3 jumpDirection;
        [SerializeField] float jumpForwardSpeed = 5;
        [SerializeField] float freeFallSpeed = 2;

        [Header("Dodge")]
        private Vector3 rollDirection;
        [SerializeField] float dodgeStaminaCost = 25;


        //[Header("Sprinting")]
        //public bool isSprinting;


        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void HandleAllMovement()
        {

            // GROUNDED MOVEMENT
            HandleGroundedMovement();
            HandleRotation();
            // AERIAL MOVEMENT
            HandleJumpingMovement();
            HandleFreeFallMovement();
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;
            //CLAMP THE MOVEMENTS
        }

        private void HandleGroundedMovement()
        {
            if (!player.canMove)
            {
                return;
            }
            GetMovementValues();

            //MOVE DIRECTION BASED ON CAMERA FACING PERSPECTIVE
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection + PlayerCamera.instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (player.isSprinting)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);

            }
            else
            {
                if (PlayerInputManager.instance.moveAmount > 0.5f)
                {
                    // MOVE AT RUNNING SPEED
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5f)
                {
                    // MOVE AT WALK SPEED
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);

                }
            }

   
        }

        private void HandleJumpingMovement()
        {
            if (player.isJumping)
            {
                player.characterController.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }

        private void HandleFreeFallMovement()
        {
            if(!player.isGrounded)
            {
                Vector3 freeFallDirection;
                freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                freeFallDirection = freeFallDirection = PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
            }
        }

        private void HandleRotation()
        {
            if(player.characterStatsManager.isDead)
            {
                return;
            }

            if(!player.canRotate)
            {
                return;
            }

            if(player.isLockedOn)
            {
                if(player.isSprinting || player.playerLocomotionManager.isRolling)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
                    targetDirection += PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if(targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
                else
                {
                    if(player.playerCombatManager.currentTarget == null)
                    {
                        return;
                    }
                    Vector3 targetDirection;
                    targetDirection = player.playerCombatManager.currentTarget.transform.position - transform.position;
                    targetDirection.y = 0;
                    targetDirection.Normalize();

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation,targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;

                }
            }
            else
            {
                TargetRotationDirection = Vector3.zero;
                TargetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward * verticalMovement;
                TargetRotationDirection = TargetRotationDirection + PlayerCamera.instance.cameraObject.transform.right * horizontalMovement;
                TargetRotationDirection.Normalize();
                TargetRotationDirection.y = 0;

                if (TargetRotationDirection == Vector3.zero)
                {
                    TargetRotationDirection = transform.forward;
                }

                Quaternion newRotation = Quaternion.LookRotation(TargetRotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = targetRotation;
            }

            
        }

        public void HandleSprinting()
        {
            if(player.isPerformingAction)
            {
                // SET SPRINT TO FALSE
                player.isSprinting = false;
            }
            // IF OUT OF STAMINE, SET TO FALSE
            if(player.playerStatsManager.CurrentStamina <= 0)
            {
                player.isSprinting = false;
                return;
            }

            // IF WE ARE MOVING SET SPRINT TO TRUE
            if (moveAmount >= 0.5)
            {
                player.isSprinting = true;
            }
            // IF WE ARE STATIONARY SET SPRINT TO FALSE
            else
            {
                player.isSprinting = false;
            }

            if (player.isSprinting)
            {
                player.playerStatsManager.CurrentStamina -= sprintingStaminaCost * Time.deltaTime;
            }
        }

        public void AttemptToPerformADodge()
        {

            if (player.isPerformingAction)
            { 
                return; 
            }

            if(player.playerStatsManager.CurrentStamina <= 0)
            {
                return;
            }
            // IF WE ARE MOVING, WE ROLL, ELSE, WE JUST BACKSTEP
            if(PlayerInputManager.instance.moveAmount > 0)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;

                rollDirection.Normalize();
                rollDirection.y = 0;

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                // PERFORM A ROLL ANIMATION
                player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward", true, true);
                player.playerLocomotionManager.isRolling = true;
            }
            else
            {
                // PERFORM A BACKSTEP ANIMATION
                player.playerAnimatorManager.PlayTargetActionAnimation("Backstep", true, true);

            }
            player.playerStatsManager.CurrentStamina -= dodgeStaminaCost;

        }

        public void AttemptToPerformAJump()
        {
            // IF WE ARE PERFORMING AN ACTION, WE DO NOT WANT TO ALLOW TO JUMP (WILL CHANGE WHENC COMBAT)
            if (player.isPerformingAction)
            {
                return;
            }

            // IF WE ARE OUT OF STAMINA, WE DO NOT WISH TO ALLOW A JUMP
            if (player.playerStatsManager.CurrentStamina <= 0)
            {
                return;
            }

            // IF WE ARE ALREADY JUMPING, WE CANNOT JUMP
            if (player.isJumping)
                return;

            // IF WE ARE NOT GROUNDED WE DO NOT WANT TO ALLOW A JUMP
            if (!player.isGrounded)
                return;

            // IF WE ARE TWO HANDING OUR WEAPON, PLAY THE TWO HANDED JUMP ANIMATION, OTHERWISE PLAY THE 1 HANDED ANIM
            player.playerAnimatorManager.PlayTargetActionAnimation("Main_Jump_01", false);

            player.isJumping = true;

            player.playerStatsManager.CurrentStamina -= jumpStaminaCost;

            jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            jumpDirection += PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.horizontalInput;

            jumpDirection.y = 0;

            if(jumpDirection != Vector3.zero)
            {
                // MOVEMENTS JUMP DIRECTIONS FORCE
                // SPRINT
                if (player.isSprinting)
                {
                    jumpDirection *= 1;
                }
                // RUN
                else if (PlayerInputManager.instance.moveAmount < 0.5)
                {
                    jumpDirection *= 0.5f;
                }
                // WALK
                else if (PlayerInputManager.instance.moveAmount <= 0.5)
                {
                    jumpDirection *= 0.25f;
                }
            }
            
        }

        public void ApplyJumpingVelocity()
        {
            // APPLY AN UPWARD VELOCITY DEPENDING ON FORCES IN OUR GAME
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
    }
}