using UnityEngine;
using UnityEngine.AI;

namespace FT
{
    public class AICharacterManager : CharacterManager
    {
        [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
        [HideInInspector] public AICharacterLocomotionManager aiCharacterLocomotionManager;
        [Header("Navmesh Agent")]
        public NavMeshAgent navMeshAgent;

        [Header("Current State")]
        [SerializeField] AIState currentState;

        [Header("States")]
        public IdleState idle;
        public PursueTargetState pursueTarget;
        // COMBAT STANCE
        // ATTACK STANCE

        protected override void Awake()
        {
            base.Awake();

            aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
            aiCharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();


            navMeshAgent = GetComponentInChildren<NavMeshAgent>();

            // USE A COPY OF THE SCRIPTABLE OBJECT, SO THE ORIGINALS ARE NOT MODIFIED
            idle = Instantiate(idle);
            pursueTarget = Instantiate(pursueTarget);

            currentState = idle;
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            ProcessStateMachine();
        }

        // OPTION 02
        private void ProcessStateMachine()
        {
            AIState nextState = currentState?.Tick(this);
            if(nextState != null)
            {
                currentState = nextState;
            }

            // THE POSITION/ROTATION SHOUD BE RESET ONLY AFTER THE STATE MACHINE
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

            if(aiCharacterCombatManager.currentTarget != null)
            {
                aiCharacterCombatManager.targetsDirection = aiCharacterCombatManager.currentTarget.transform.position - transform.position;
                aiCharacterCombatManager.viewableAngle = WorldUtilityManager.instance.GetAngleOfTarget(transform, aiCharacterCombatManager.targetsDirection);
                aiCharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position, aiCharacterCombatManager.currentTarget.transform.position);
            }

            if(navMeshAgent.enabled)
            {
                Vector3 agentDestination = navMeshAgent.destination;
                float remainingDistance = Vector3.Distance(agentDestination, transform.position);

                if(remainingDistance > navMeshAgent.stoppingDistance)
                {
                    isMoving = true;
                }
                else
                {
                    isMoving = false;
                }
            }
            else
            {
                isMoving = false;
            }

        }
    }

        /*// OPTION 01
        private void ProcessStateMachine()
        {
            AIState nextState = null;
            if(currentState != null)
            {
                nextState = currentState.Tick(this);
            }
            if(nextState != null)
            {
                currentState = nextState;
            }
        }*/
}
