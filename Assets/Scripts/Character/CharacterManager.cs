using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace FT
{
    public class CharacterManager : MonoBehaviour
    {

        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;
        [HideInInspector] public CharacterStatsManager characterStatsManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        [HideInInspector] public PlayerManager playerManager;
        [HideInInspector] public CharacterCombatManager characterCombatManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterSoundFXManager characterSoundFXManager; 

        [Header("Equipment")]
        public int _currentWeaponBeingUsedID;
        public int _currentRightHandWeaponID;
        public int _currentLeftHandWeaponID;

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;
        public bool isGrounded = true;
        public bool isJumping = false;
        public bool isUsingRightHand = false;
        public bool isUsingLeftHand = false;
        public bool isMoving = false;

        [Header("Character Group")]
        public CharacterGroup characterGroup;

        [Header("Actions")]
        public bool isSprinting;

        private bool _isLockedOn;
        public bool isLockedOn
        {
            get => _isLockedOn;
            set
            {
                if (_isLockedOn != value)
                {
                    bool oldValue = _isLockedOn;
                    _isLockedOn = value;
                    OnIsLockedOnchanged(oldValue, _isLockedOn); // Lógica adicional caso necessário
                }
            }
        }
        private bool _isChargingAttack;
        public bool isChargingAttack
        {
            get => _isChargingAttack;
            set
            {
                if (_isChargingAttack != value)
                {
                    bool oldValue = _isChargingAttack;
                    _isChargingAttack = value;
                    OnIsChargingAttackChanged(oldValue, _isChargingAttack); // Lógica adicional caso necessário
                }
            }
        }
        public int CurrentRightHandWeaponID
        {
            get => _currentRightHandWeaponID;
            set
            {
                if (_currentRightHandWeaponID != value)
                {
                    int oldID = _currentRightHandWeaponID;
                    _currentRightHandWeaponID = value;
                    OnCurrentRightHandWeaponIDChange(oldID, value);
                }
            }
        }

        public int CurrentLeftHandWeaponID
        {
            get => _currentLeftHandWeaponID;
            set
            {
                if (_currentLeftHandWeaponID != value)
                {
                    int oldID = _currentLeftHandWeaponID;
                    _currentLeftHandWeaponID = value;
                    OnCurrentLeftHandWeaponIDChange(oldID, value);
                }
            }
        }

        public int CurrentWeaponBeingUsedID
        {
            get => _currentWeaponBeingUsedID;
            set
            {
                if (_currentWeaponBeingUsedID != value)
                {
                    int oldID = _currentWeaponBeingUsedID;
                    _currentWeaponBeingUsedID = value;
                    OnCurrentWeaponBeingUsedIDChange(oldID, value);
                }
            }
        }

        public void SetCharacterActionHand(bool rightHandedAction)
        {
            if(rightHandedAction)
            {
                isUsingRightHand = true;
                isUsingLeftHand = false;
            }
            else
            {
                isUsingRightHand = false;
                isUsingLeftHand = true;
            }
        }

        //[Header("Stats")]
        //public int endurance = 1;
        //public int currentStamina = 0;
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);

            characterController = GetComponent<CharacterController>();
            animator = GetComponentInChildren<Animator>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();
            characterStatsManager = GetComponentInChildren<CharacterStatsManager>();
            characterAnimatorManager = GetComponentInChildren<CharacterAnimatorManager>();
            playerManager = GetComponent<PlayerManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
        }
        
        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }

        protected virtual void Update()
        {
            if(animator  != null) 
            {
                animator.SetBool("isGrounded", isGrounded);
            }
            if(isMoving)
            {
                animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
        }

        protected virtual void FixedUpdate()
        {
            
        }

        protected virtual void LateUpdate()
        {

        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            characterStatsManager.CurrentHealth = 0;
            characterStatsManager.isDead = true;

            // RESET ANY FLAGS HERE THAT NEED TO BE RESET

            // IF WE ARE NOT GROUNDED, PLAY AND AERIAL DEATH ANIMATION

            if (!manuallySelectDeathAnimation)
            {
                characterAnimatorManager.PlayTargetActionAnimation("Dead_01", true);
            }

            // PLAY SOME DEATH SFX

            yield return new WaitForSeconds(5);
            if (characterStatsManager.isDead)
                yield break; // Cancela a execução se já estiver morto.
            // AWARD PLAYERS WITH RUNES

            // DISABLE CHARACTER
        }

        public virtual void ReviveCharacter()
        {

        }

        public void OnCurrentRightHandWeaponIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
            playerManager.playerInventoryManager.currentRightHandWeapon = newWeapon;
            playerManager.playerEquipmentManager.LoadRightWeapon();

            PlayerUIManager.instance.playerUIHudManager.SetRightWeaponQuickSlotIcon(newID);
        }
        public void OnCurrentLeftHandWeaponIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
            playerManager.playerInventoryManager.currentLeftHandWeapon = newWeapon;
            playerManager.playerEquipmentManager.LoadLeftWeapon();

            PlayerUIManager.instance.playerUIHudManager.SetLeftWeaponQuickSlotIcon(newID);

        }
        public void OnCurrentWeaponBeingUsedIDChange(int oldID, int newID)
        {
            WeaponItem newWeapon = Instantiate(WorldItemDatabase.instance.GetWeaponByID(newID));
            CurrentWeaponBeingUsedID = newWeapon.itemID;
        }

        private void PerformWeaponBasedAction(int actionID, int weaponID)
        {
            WeaponItemAction weaponAction = WorldActionManager.instance.GetWeaponItemActionByID(actionID);

            if (weaponAction != null)
            {
                weaponAction.AttemptToPerformAction(playerManager, WorldItemDatabase.instance.GetWeaponByID(weaponID));
            }
            else
            {
                Debug.Log("ACTION IS NULL, CANNOT BE PERFORMED");
            }
        }

        public void OnIsLockedOnchanged(bool old, bool isLockedOn)
        {
            if(!isLockedOn)
            {
                characterCombatManager.currentTarget = null;
            }
        }

        public void OnIsChargingAttackChanged(bool oldStatus, bool newStatus)
        {
            animator.SetBool("isChargingAttack", isChargingAttack);
        }

        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider = GetComponent<Collider>();
            Collider[] damageableCharacterColliders = GetComponentsInChildren<Collider>();

            List<Collider> ignoreCollider = new List<Collider>();

            // ADD ALL OF OUR DAMAGEABLE CHARACTER COLLIDER TO THE LIST HTAT WILL BE IGNORED
            foreach( var collider in damageableCharacterColliders)
            {
                ignoreCollider.Add(collider);
            }

            // ADD OUR CHARACTER CONTROLLER COLLIDER TO THE LIST HTAT WILL BE USED TO IGNORE COLLISIONS
            ignoreCollider.Add(characterControllerCollider);

            // GOES THROUGH EVERY COLLIDER IN THE LIST AND IGNORE COLLISIONS WITH EACH OTHER 
            foreach ( var collider in ignoreCollider)
            {
                foreach (var otherCollider in ignoreCollider)
                {
                    Physics.IgnoreCollision(collider, otherCollider, true);
                }
            }
        }

        public void ProcessCharacterDamage(
            PlayerManager damagedCharacterID, 
            PlayerManager characterCausingDamageID, 
            float physicalDamage,
            float magicDamage,
            float fireDamage,
            float holyDamage,
            float poiseDamage,
            float lightningDamage,
            float angleHitFrom,
            float contactPointX,
            float contactPointY,
            float contactPointZ
            )
        {
            CharacterManager damagedCharacter = GetComponent<CharacterManager>();
            CharacterManager characterCausingDamage = GetComponent<CharacterManager>();

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);

            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.holyDamage = holyDamage;
            damageEffect.poiseDamage = poiseDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.angleHitFrom = angleHitFrom;
            damageEffect.contactPoint = new Vector3(contactPointX, contactPointY, contactPointZ);
            damageEffect.characterCausingDamage = characterCausingDamage;

            damagedCharacter.characterEffectsManager.ProcessInstantEffects( damageEffect );

        }
    }
}