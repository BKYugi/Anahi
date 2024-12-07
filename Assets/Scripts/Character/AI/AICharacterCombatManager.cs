using UnityEngine;

namespace FT
{
    public class AICharacterCombatManager : CharacterCombatManager
    {
        [Header("Target Information")]
        public float distanceFromTarget;
        public float viewableAngle;
        public Vector3 targetsDirection;

        [Header("Detection")]
        [SerializeField] float detectionRadius = 15;
        public float minimumFOV = -35;
        public float maximumFOV = 35;

        public void FindATargetLineOfSight(AICharacterManager aiCharacter)
        {
            if (currentTarget != null)
            {
                return;
            }

            Collider[] colliders = Physics.OverlapSphere(aiCharacter.transform.position, detectionRadius, WorldUtilityManager.instance.GetCharacterLayers());

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();

                if (targetCharacter == null)
                    continue;
                
                if(targetCharacter == aiCharacter)
                    continue;
                

                if(targetCharacter.characterStatsManager.isDead)
                    continue;
                


                // CAN I ATTACK THIS CHARACTER? IS YES, MAKE MY TARGET
                if(WorldUtilityManager.instance.CanIDamageThisTarget(aiCharacter.characterGroup, targetCharacter.characterGroup))
                {
                    // IF A POTENTIAL TARGET IS FOUND, IT HAS TO BE INFRONT OF US
                    Vector3 targetsDirection = targetCharacter.transform.position - aiCharacter.transform.position;
                    float angleOfPotentialTarget = Vector3.Angle(targetsDirection, aiCharacter.transform.forward);

                    if(angleOfPotentialTarget > minimumFOV && angleOfPotentialTarget < maximumFOV)
                    {
                        // LASTLY WE CHECK FOR ENVIRO BLOCCKS 
                        if (Physics.Linecast(aiCharacter.characterCombatManager.lockOnTransform.position, 
                            targetCharacter.characterCombatManager.lockOnTransform.position,
                            WorldUtilityManager.instance.GetEnviroLayers()))
                        {
                            Debug.DrawLine(aiCharacter.characterCombatManager.lockOnTransform.position, targetCharacter.characterCombatManager.lockOnTransform.position);
                            Debug.Log("BLOCKED");
                            
                        }
                        else
                        {
                            targetsDirection = targetCharacter.transform.position - transform.position;
                            this.viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, targetsDirection);
                            aiCharacter.characterCombatManager.SetTarget(targetCharacter);
                            PivotTowardsTarget(aiCharacter);
                        }
                        
                    }
                }

            }

        }

        public void PivotTowardsTarget(AICharacterManager aiCharacter)
        {
            // PLAY A PIVOT ANIMATION DEPENDING ON VIEWABLE ANGLE OF TARGET
            if(aiCharacter.isPerformingAction)
            {
                return;
            }

            if(viewableAngle >= 20 && viewableAngle <= 60)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Right_Turn", true);
            }
            else if (viewableAngle >= -20 && viewableAngle >= 60)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Left_Turn", true);
            }
            else if (viewableAngle >= 61 && viewableAngle <= 110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Right_Turn", true);
            }
            else if (viewableAngle <= -61 && viewableAngle >= -110)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Left_Turn", true);
            }
            else if (viewableAngle >= 110 && viewableAngle <= 145)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Right_Turn", true);
            }
            else if (viewableAngle <= -110 && viewableAngle >= -145)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Left_Turn", true);
            }
            else if (viewableAngle >= 146 && viewableAngle <= 180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Right_Turn", true);
            }
            else if (viewableAngle <= -146 && viewableAngle >= -180)
            {
                aiCharacter.characterAnimatorManager.PlayTargetActionAnimation("Left_Turn", true);
            }
        }
    }
}