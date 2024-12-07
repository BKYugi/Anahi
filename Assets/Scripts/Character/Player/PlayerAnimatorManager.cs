using UnityEngine;

namespace FT
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }
        private void OnAnimatorMove()
        {
            if (player.applyRootMotion)
            {
                Vector3 velocity = player.animator.deltaPosition;
                player.characterController.Move(velocity);
                player.transform.rotation *= player.animator.deltaRotation;
            }
        }

        public override void EnableCanDoCombo()
        {
            if (player.isUsingRightHand)
            {
                player.playerCombatManager.canComboWithMainHandWeapon = true;
            }
            else
            {
                // ENABLE OFF HAND COMBO
            }
        }

        public override void DisableCanDoCombo()
        {
            if (player.isUsingRightHand)
            {
                player.playerCombatManager.canComboWithMainHandWeapon = false;
                // canComboWithOffHandWeapon = false;

            }

        }
    }
}
