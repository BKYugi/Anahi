using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FT
{
    public class PlayerManager : CharacterManager
    {
        [Header("Debug Menu")]
        [SerializeField] bool respawnCharacter = false;
        [SerializeField] bool switchRightWeapon = false;
        [SerializeField] bool switchLeftWeapon = false;

        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerStatsManager playerStatsManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;
        public string characterName;
        protected override void Awake()
        {
            base.Awake();
            // Garantindo que o player seja atribuído no início
            if (WorldSaveGameManager.instance != null)
            {
                WorldSaveGameManager.instance.player = this;
            }
            else
            {
                Debug.LogError("WorldSaveGameManager instance is null when assigning player.");
            }

            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            
            //WorldSaveGameManager.instance.player = this;
            PlayerInputManager.instance.player = this;
            //WorldSaveGameManager.instance.player = this;
            //currentStamina += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue();

            // CHARACTER STATS MANAGER
            //playerStatsManager.CurrentHealth += PlayerUIManager.instance.playerUIHudManager.SetNewHealthValue();
            //playerStatsManager.CurrentStamina += PlayerUIManager.instance.playerUIHudManager.SetNewStaminaValue();
            //playerStatsManager.CurrentStamina += playerStatsManager.ResetStaminaRegenTimer();



        }

        protected override void Start()
        {
            NotifyCamera();

        }
        protected override void Update()
        {
            base.Update();

            // HANDLE MOVEMENT
            playerLocomotionManager.HandleAllMovement();

            // REGEN STAMINA
            playerStatsManager.RegenerateStamina();

            DebugMenu();

            if (Input.GetKeyDown(KeyCode.K))
                DebugMenu();
        }
        protected override void LateUpdate()
        {
            base.LateUpdate();

            PlayerCamera.instance.HandleAllCameraActions();
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            PlayerUIManager.instance.playerUIPopUpManager.SendYouDiedPopUp();

            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        }

        public override void ReviveCharacter()
        {
            base.ReviveCharacter();

            playerStatsManager._currentHealth = playerStatsManager.maxHealth;
            playerStatsManager._currentStamina = playerStatsManager.maxStamina;
            // RESTORE MANA

            // PLAY REBIRTH EFFECTS
            playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
        }

        private void NotifyCamera()
        {
            if (PlayerCamera.instance != null)
            {
                PlayerCamera.instance.SetPlayerManager(this);
            }
        }

        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentCharacterData.characterName = characterName;
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;

            currentCharacterData.vitality = playerStatsManager.Vitality;
            currentCharacterData.endurance = playerStatsManager.Endurance;

            currentCharacterData.currentHealth = playerStatsManager.CurrentHealth;
            currentCharacterData.currentStamina = playerStatsManager.CurrentStamina;
        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            {
                characterName = currentCharacterData.characterName;
                Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
                transform.position = myPosition;

                playerStatsManager.Vitality = currentCharacterData.vitality;
                playerStatsManager.Endurance = currentCharacterData.endurance;

                playerStatsManager.maxHealth = playerStatsManager.CalculateHealthBasedOnVitalityLevel(currentCharacterData.vitality);
                playerStatsManager.maxStamina = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(currentCharacterData.endurance);
                playerStatsManager.CurrentHealth = currentCharacterData.currentHealth;
                playerStatsManager.CurrentStamina = currentCharacterData.currentStamina;
                PlayerUIManager.instance.playerUIHudManager.SetMaxStaminaValue(playerStatsManager.maxStamina);
                PlayerUIManager.instance.playerUIHudManager.SetMaxHealthValue(playerStatsManager.maxHealth);

            }


        }



        // DEBUG REMOVE LATER
        private void DebugMenu()
        {
            if (respawnCharacter)
            {
                respawnCharacter = false;
                ReviveCharacter();
            }

            if (switchRightWeapon)
            {
                switchRightWeapon = false;
                playerEquipmentManager.SwitchRightWeapon();
            }

            if (switchLeftWeapon)
            {
                Debug.Log("Switch Left Weapon Triggered!");
                switchLeftWeapon = false;
                playerEquipmentManager.SwitchLeftWeapon();
            }
        }
    }
}