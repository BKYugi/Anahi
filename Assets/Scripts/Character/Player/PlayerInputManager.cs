using UnityEngine;
using UnityEngine.SceneManagement;

namespace FT
{
    public class PlayerInputManager : MonoBehaviour
    {
        public GameObject playerPrefab;  // Arraste o prefab do player aqui no Inspector

        public static PlayerInputManager instance;
        public PlayerManager player;

        PlayerControls playerControls;

        [Header("PLAYER MOVEMENT INPUTS")]
        [SerializeField] Vector2 movementInput;
        public float verticalInput;
        public float horizontalInput;
        public float moveAmount;

        [Header("PLAYER ACTIONS INPUT")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;

        [Header("LOCK ON INPUT")]
        [SerializeField] bool lockOnInput;
        [SerializeField] bool lockOnLeftInput;
        [SerializeField] bool lockOnRightInput;
        private Coroutine lockOnCoroutine;


        [Header("CAMERA MOVEMENT INPUTS")]
        [SerializeField] Vector2 cameraInput;
        public float cameraVerticalInput;
        public float cameraHorizontalInput;

        [Header("TRIGGER INPUTS")]
        [SerializeField] bool RB_Input = false;
        [SerializeField] bool RT_Input = false;
        [SerializeField] bool hold_RT_Input = false;

        [Header("D-PAD INPUT")]
        [SerializeField] bool switchRightWeaponInput = false;
        [SerializeField] bool switchLeftWeaponInput = false;





        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }

        }

        public void CreatePlayer()
        {
            // Checa se o player já existe na cena
            if (FindAnyObjectByType<CharacterManager>() == null)
            {
                // Instancia o player na posição inicial da cena
                Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            }
        }
        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            //IF WE ARE LOADING IN THE WORLD SCENE, ENABLE CHARACTER INPUT
            if (newScene.buildIndex == WorldSaveGameManager.instance.GetWorldSceneIndex())
            {
                instance.enabled = true;

                if (playerControls != null)
                {
                    playerControls.Enable();
                }
            }
            else
            {
                instance.enabled = false;

                if (playerControls != null)
                {
                    playerControls.Disable();
                }
            }
        }
        private void OnEnable()
        {
            // Verifica se playerControls é nulo e, se for, inicializa ele
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Mouse.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
                playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;
                playerControls.PlayerActions.SeekLeftLockOnTarget.performed += i => lockOnLeftInput = true;
                playerControls.PlayerActions.SeekRightLockOnTarget.performed += i => lockOnRightInput = true;

                // BUMPER
                playerControls.PlayerActions.RB.performed += i => RB_Input = true;

                // TRIGGERS
                playerControls.PlayerActions.HoldRT.performed += i => hold_RT_Input = true;
                playerControls.PlayerActions.HoldRT.canceled += i => hold_RT_Input = false;
                playerControls.PlayerActions.RT.performed += i => RT_Input = true;

                playerControls.PlayerActions.SwitchRightWeapon.performed += i => switchRightWeaponInput = true;
                playerControls.PlayerActions.SwitchLeftWeapon.performed += i => switchLeftWeaponInput = true;



                // HOLD TO ACTIVATE
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;



            }

            playerControls.Enable();
        }

        private void SwitchRightWeapon_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            throw new System.NotImplementedException();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if (focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            HandlePlayerMovementInput();
            HandCameraMovementInput();
            HandleDodgeInput();
            HandleSprintInput();
            HandleJumpInput();
            HandleLockOnInput();
            HandleLockOnSwitchTargetInput();
            HandleRBInput();
            HandleRTInput();
            HandleChargeRTInput();
            HandleSwitchRightWeaponInput();
            HandleSwitchLeftWeaponInput();
        }
        // MOVEMENT 
        private void HandlePlayerMovementInput()
        {
            verticalInput = movementInput.y;
            horizontalInput = movementInput.x;

            //ABS = ABSOLUTE = MODULO
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));

            //CLAMP VALUE TO 0 // 0.5 // 1 
            if (moveAmount <= 0.5 && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount >= 0.5 && moveAmount <= 1)
            {
                moveAmount = 1;
            }

            // PASS 0 IN THE HORIZONTAL CAUSE WANT NON STRAFING MOVEMENT
            // USE HORIZONTAL WHEN STRAFING OR LOCKED ON

            if (player == null)
                return;
            if(moveAmount != 0)
            {
                player.isMoving = true;
            }
            else
            {
                player.isMoving = false;
            }

            if (!player.isLockedOn || player.isSprinting)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.isSprinting);
            }
            else
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, player.isSprinting);

            }

        }

        private void HandCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }

        // ACTION
        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;

                // FUTURE NOTE: RETURN IF MENU OR UI IS OPEN

                // PERFORM A DODGE
                player.playerLocomotionManager.AttemptToPerformADodge();

            }
        }

        private void HandleSprintInput()
        {
            if (sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();
                player.isSprinting = true;

            }
            else
            {
                player.isSprinting = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;
                // IF WE HAVE A UI WINDOW OPEN, SIMPLY RETURN WITHOUT DOING ANYTHIN

                // ATTEMPT TO PERFORM JUMP
                player.playerLocomotionManager.AttemptToPerformAJump();
            }
        }

        private void HandleLockOnInput()
        {

            if (player.isLockedOn)
            {
                if (player.playerCombatManager.currentTarget == null)
                {
                    return;
                }
                if (player.playerCombatManager.currentTarget.characterStatsManager.isDead)
                {
                    player.isLockedOn = false;
                }

                // ATTEMPT TO FIND A NEW TARGET
                // THIS ASSURES THAT THE COROUTINE NEVER RUNS MULTIPLE TIMES
                if(lockOnCoroutine != null)
                {
                    StopCoroutine(lockOnCoroutine);
                }

                lockOnCoroutine = StartCoroutine(PlayerCamera.instance.WaitThenFindNewTarget());
            }

            if(lockOnInput && player.isLockedOn)
            {
                lockOnInput = false;
                PlayerCamera.instance.ClearLockOnTargets();
                player.isLockedOn = false;

                return;
                
            }

            if(lockOnInput && !player.isLockedOn)
            {
                lockOnInput = false;
                // IF WE ARE AIMING RETURN

                // ATTEMPT TO LOCK ON
                PlayerCamera.instance.HandleLocationLockOnTargets();
                // ARE WE ALREADY LOCK ON - RETURN/UNLOCK
                // IS OUR CURRENT TARGET DEAD - RETURN/UNLOCK

                // ENABLE LOCK ON
                if(PlayerCamera.instance.nearestLockOnTarget != null)
                {
                    // SET THE TARGET AS OUR CURRENT TARGET
                    player.playerCombatManager.SetTarget(PlayerCamera.instance.nearestLockOnTarget);
                    player.isLockedOn = true;
                }
            }



        }

        private void HandleLockOnSwitchTargetInput()
        {
            if (lockOnLeftInput)
            {
                lockOnLeftInput = false;

                if(player.isLockedOn)
                {
                    PlayerCamera.instance.HandleLocationLockOnTargets();

                    if(PlayerCamera.instance.leftLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.instance.leftLockOnTarget);
                    }
                }
            }

            if (lockOnRightInput)
            {
                lockOnRightInput = false;

                if (player.isLockedOn)
                {
                    PlayerCamera.instance.HandleLocationLockOnTargets();

                    if (PlayerCamera.instance.rightLockOnTarget != null)
                    {
                        player.playerCombatManager.SetTarget(PlayerCamera.instance.rightLockOnTarget);
                    }
                }
            }
        }

        private void HandleRBInput()
        {
            if (RB_Input)
            {
                RB_Input = false;

                // TO DO: IF WE HAVE A UI WINDOW OPEN, RETURN

                player.SetCharacterActionHand(true);

                // TO DO: IF WE ARE TWO HANDING THE WEAPON, USE TWO HANDED ACTION

                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RB_Action, player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        private void HandleRTInput()
        {
            if (RT_Input)
            {
                RT_Input = false;

                // TO DO: IF WE HAVE A UI WINDOW OPEN, RETURN

                player.SetCharacterActionHand(true);

                // TO DO: IF WE ARE TWO HANDING THE WEAPON, USE TWO HANDED ACTION

                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.oh_RT_Action, player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        private void HandleChargeRTInput()
        {
            // WE ONLY WANT TO CHECK FOR A CHANGE IF WE ARE IN AN ACTION THAT REQUIRES IT (ATTACCKING)
            if(player.isPerformingAction)
            {
                if(player.isUsingRightHand) 
                {
                    //player.isChargingAttack = Hold_RT_Input;
                    player.isChargingAttack = hold_RT_Input;
                }
            }
        }

        private void HandleSwitchRightWeaponInput()
        {
            if(switchRightWeaponInput)
            {
                switchRightWeaponInput = false;

                player.playerEquipmentManager.SwitchRightWeapon();
            }
        }

        private void HandleSwitchLeftWeaponInput()
        {
            if (switchLeftWeaponInput)
            {
                switchLeftWeaponInput = false;

                player.playerEquipmentManager.SwitchLeftWeapon();
            }
        }
    }
}