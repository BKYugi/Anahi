using FT;
using UnityEngine;
using UnityEngine.AI;

namespace FT
{
    [CreateAssetMenu(menuName = "AI/States/Pursue Target")]

    public class PursueTargetState : AIState
    {

        public override AIState Tick(AICharacterManager aiCharacter)
        {
            // CHECK IF WE ARE PERFORMING AN ACTION (IF SO DO NOTHING UNTIL ACTION IS COMPLETE)
            if(aiCharacter.isPerformingAction)
            {
                return this;
            }

            // CHECK IF OUR TARGET IS NULL, IF WE DO NOT HAVE A TARGET, RETURN TO IDLE STATE
            if(aiCharacter.aiCharacterCombatManager.currentTarget == null)
            {
                return SwitchState(aiCharacter, aiCharacter.idle);
            }

            // MAKE SURE OUR NAVMESH AGENT IS ACTIVE, IF ITS NOT ENABLE IT
            if(!aiCharacter.navMeshAgent.enabled)
            {
                aiCharacter.navMeshAgent.enabled = true;
            }

            // IF OUR TARGET GOES OUTSIDE OF THE CHARACTERS FOV, PIVOT TO FACE THEM
            if(aiCharacter.aiCharacterCombatManager.viewableAngle < aiCharacter.aiCharacterCombatManager.minimumFOV 
                || aiCharacter.aiCharacterCombatManager.viewableAngle > aiCharacter.aiCharacterCombatManager.maximumFOV)
            {
                aiCharacter.aiCharacterCombatManager.PivotTowardsTarget(aiCharacter);
            }

            aiCharacter.aiCharacterLocomotionManager.RotateTowardsAgent(aiCharacter);

            // IF WE ARE WITHIN COMBAT RANGE OF A TARGET, SWITCH STATE TO COMBAT STANCE STATE 

            // IF THE TARGET IS NOT REACHABLE, AND THEY ARE FAR AWAY, RETURN HOME

            // PURSUE THE TARGET
            // OPTION 01 ( NOT USED ) 
            // aiCharacter.navMeshAgent.SetDestination(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position);

            // OPTION 02
            NavMeshPath path = new NavMeshPath();
            aiCharacter.navMeshAgent.CalculatePath(aiCharacter.aiCharacterCombatManager.currentTarget.transform.position, path);
            aiCharacter.navMeshAgent.SetPath(path);

            return this;
        }

    }
}
