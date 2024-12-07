using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace FT
{
    [CreateAssetMenu(menuName = "AI/States/Combat Stance")]

    public class CombatStanceState : AIState
    {

        // 1. SELECT AN ATTACK FOR THE ATTACK STATE, DEPENDING ON DISTANCE AND ANGLE OF TARGET IN RELATION TO CHARACTER
        // 2. PROCESS ANY COMBAT LOGIC HERE WHILST WAITING TO ATTACK (BLOCKING, STRAFING, DODING, ETC)
        // 3. IF TARGET MOVES OUT OF COMBAT RANGE, SWITCH TO PURSUE TARGET
        // 4. IF TARGET IS NO LONGER PRESENT, SWITCH TO IDLE STATE

        [Header("Attacks")]
        public List<AICharacterAttackAction> aiCharacterAttacks; // ALL POSSIBLE ATTACKS THE CHARACTER CAN DO
        protected List<AICharacterAttackAction> potentialAttacks; // A LIST THAT IS CREATED IN THIS STATE, ALL ATTACK POSSIBLE IN THIS SITUATION (BASED ON DISTANCE, ANGLE, ETC)
        private AICharacterAttackAction choosenAttack;
        private AICharacterAttackAction previousAttack;
        protected bool hasAttack = false;

        [Header("Combo")]
        [SerializeField] protected bool canPerformCombo = false; // IF AI CAN PERFORM A COMBO AFTER FIRST ATTACK
        [SerializeField] protected int chanceToPerformCombo = 25; // THE CHANCE IN % FOR CHARACTER TO PERFORM A COMBO
        [SerializeField] bool hasRolledForChance = false; // IF WE HAVE ALREADY ROLLED TO THE CHANCE THIS STATE

        [Header("Engagement Distance")]
        [SerializeField] protected float maximumEngagementDistance = 5; // THE DISTANCE WE HAVE TO BE AWAY FROM THE TARGET WE ENTER THE PURSUE TARGET STATE

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if(aiCharacter.isPerformingAction)
                return this;
            
            if (!aiCharacter.navMeshAgent.enabled)
                aiCharacter.navMeshAgent.enabled = true;

            // IF YOU WANT THE AI CHARACTER TO FACE AND TURN TOWARDS ITS TARGET WHEN ITS OUTSIDE ITS FOV INCLUDE THIS

            if (!aiCharacter.isMoving)
            {
                if (aiCharacter.aiCharacterCombatManager.viewableAngle < -30 || aiCharacter.aiCharacterCombatManager.viewableAngle > 30)
                    aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }
            // ROTATE TO FACE OUR TARGET

            // IF OUR TARGET IS NO LONGE RPRESENT, GO TO IDLE
            if (aiCharacter.aiCharacterCombatManager.currentTarget == null)
                return SwitchState(aiCharacter, aiCharacter.idle);

            if (!hasAttack)
            {
                GetNewAttack(aiCharacter);
            }
            else
            {
                // CHECK RECOVERY TIMER
                // PASS ATTACK TO ATTACK STATE
                // ROLL FOR COMBO CHANCE
                // SWITCH STATE
            }

            if (aiCharacter.aiCharacterCombatManager.distanceFromTarget > maximumEngagementDistance)
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);

            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;

        }

        protected virtual void GetNewAttack(AICharacterManager aICharacter)
        {
            potentialAttacks = new List<AICharacterAttackAction>();

            foreach (var potentialAttack in potentialAttacks)
            {
                // IF WE ARE TOO CLOSE FOR THIS, NEXT
                if (potentialAttack.minimumAttackDistance > aICharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;
                // IF WE ARE TOO FAR FOR THIS, NEXT
                if (potentialAttack.maximumAttackDistance < aICharacter.aiCharacterCombatManager.distanceFromTarget)
                    continue;
                // IF THE TARGET IS OUTSIDE THE VIEW, NEXT
                if (potentialAttack.minimumAttackDistance > aICharacter.aiCharacterCombatManager.viewableAngle)
                    continue;
                // IF THE TARGET IS OUTSIDE THE VIEW, NEXT
                if (potentialAttack.maximumAttackDistance < aICharacter.aiCharacterCombatManager.viewableAngle)
                    continue;

                potentialAttacks.Add(potentialAttack);

            }

            if (potentialAttacks.Count <= 0)
                return;

            var totalWeight = 0;

            foreach (var attack in potentialAttacks)
            {
                totalWeight += attack.attackWeight;
            }

            var randomWeightValue = Random.Range(1, totalWeight + 1);
            var processedWeight = 0;
            foreach (var attack in potentialAttacks)
            {
                processedWeight += attack.attackWeight;
                if (randomWeightValue <= processedWeight)
                {
                    // THIS IS OUR ATTACK
                    choosenAttack = attack;
                    previousAttack = choosenAttack;
                    hasAttack = true;
                }
            }
            // 1. SORT TROUGH ALL POSSIBLE ATTACKS
            // 2. REMOVE ATTACKS THAT  CANT BE USED IN THIS SITUATION (ANGLE OR DISTANCE)
            // 3. PLACE REMAINING ATTACKS IN A LIST
            // 4. PICK ONE OF THE REMAINING ATTACKS RANDOMBLY, BASE ON WEIGHT
            // 5. SELECT THIS ATTACK AND PASS TO ATTACK STATE
        }

        protected virtual bool RollForOutcomeChance(int outcomeChance)
        {
            bool outcomeWillBePerformed = false;

            int randomPercentage = Random.Range(0, 100);

            if (randomPercentage < outcomeChance)
            {
                outcomeWillBePerformed = true;
            }

            return outcomeWillBePerformed;
        }

        protected override void ResetStateFlags(AICharacterManager aICharacter)
        {
            base.ResetStateFlags(aICharacter);
            hasRolledForChance = false;
            hasAttack = false;
        }

    }
}
