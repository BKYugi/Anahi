using UnityEngine;

namespace FT
{
    public class AICharacterLocomotionManager : CharacterLocomotionManager
    {
        
        public void RotateTowardsAgent(AICharacterManager aiCharacter)
        {
            if (aiCharacter.isMoving)
            {
                aiCharacter.transform.rotation = aiCharacter.navMeshAgent.transform.rotation;
            }
        }

    }
}
