using System.Runtime.InteropServices;
using UnityEngine;

namespace FT
{
    [CreateAssetMenu(menuName = "AI/States/Idle")]
    public class IdleState : AIState
    {

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            if(aiCharacter.characterCombatManager.currentTarget != null)
            {
                // RETURN THE PURSUE TARGET STATE (CHANGE THE SATET TO THE PURSUE TARGET STATE)
                return SwitchState(aiCharacter, aiCharacter.pursueTarget);
            }
            else
            {
                // RETURN THIS STATE, TO CONTINUALLY SEARCH FOR A TARGET ( KEEP THE STATE HERE, UNTIL A TARGET IS FOUND)
                aiCharacter.aiCharacterCombatManager.FindATargetLineOfSight(aiCharacter);
                return this;
            }

        }


    }
}
