using UnityEngine;

namespace FT
{
    public class AIState : ScriptableObject
    {
        
        public virtual AIState Tick(AICharacterManager aiCharacter)
        {
            // DO SOME LOGIC TO FIND THE PLAYER

            // IF WE HAVE FOUND THE PLAYER, RETURN THE PURSUE TARGET SATE INSTEAD

            // IF WE HAVE NOT FOUND THE PLAYER, CONTINUE TO RETURN THE IDLE STATE
            return this; 
        }

        protected virtual AIState SwitchState(AICharacterManager aICharacter, AIState newState)
        {
            ResetStateFlags(aICharacter);
            return newState;
        }

        protected virtual void ResetStateFlags(AICharacterManager aICharacter)
        {
            // RESET ANY FALGS HERE SO WHEN YOU RETURN TO THE STATE, THEY ARE BLANK ONCE AGAIN
        }

    }
}
