using UnityEngine;

namespace FT
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        PlayerManager player;

        public WeaponItem currentWeaponBeingUsed;

        [Header("FLAGS")]
        public bool canComboWithMainHandWeapon = false;
        //public bool canComboWithOffHandWeapon = false;

        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerforminAction)
        {
            // PERFORM THE ACTION

            weaponAction.AttemptToPerformAction(player, weaponPerforminAction);
        }

        public virtual void DrainStaminaBaseOnAttack()
        {
            float staminaDeducted = 0;

            if (currentWeaponBeingUsed == null) { return; }

            switch (currentAttackType)
            {
                case AttackType.LightAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                default:
                    break;
            }

            player.playerStatsManager._currentStamina -= staminaDeducted;
        }

        public override void SetTarget(CharacterManager newTarget)
        {
            base.SetTarget(newTarget);

            PlayerCamera.instance.SetLockCameraHeight();
        }

        
    }
}
