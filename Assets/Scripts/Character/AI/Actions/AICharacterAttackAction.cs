using UnityEngine;

namespace FT
{
    [CreateAssetMenu(menuName = "AI/Actions/Attack")]

    public class AICharacterAttackAction : ScriptableObject
    {
        [Header("Attack")]
        [SerializeField] private string attackAnimation;

        [Header("Combo Action")]
        public AICharacterAttackAction comboAction; // THE COMBO ACTION OF THIS ATTACK ACTION

        [Header("Action Values")]
        public int attackWeight = 50;
        // ATTACK TYPE
        [SerializeField] AttackType attackType;
        // ATTACK CAN BE REPEATED 
        public float actionRecoveryTime = 1.5f; // THE TIME BEFORE THE CHARACTER CAN MAKE ANOTHER ATTACK AFTER PERFORMING THIS ONE
        public float minimumAttackAngle = -35;
        public float maximumAttackAngle = 35;
        public float minimumAttackDistance = 0;
        public float maximumAttackDistance = 2;
        public void AttemptToPerformAction(AICharacterManager aiCharacter)
        {
            aiCharacter.characterAnimatorManager.PlayTargetActionAnimation(attackAnimation, true);
        }

    }
}