using UnityEngine;
namespace FT
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;

        [Header("Ground Check & Jumping")]
        [SerializeField] protected float gravityForce = - 5.55f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 1;
        [SerializeField] protected Vector3 yVelocity; // THE FORCE AT WHICH OUR CHARACTER IS PULLED UP OR DOWN (Jumping or falling)
        [SerializeField] protected float groundedYVelocity = -20; // THE FORCE AT WHICH OUR CHARACTER IS STICKING TO THE GORUND WHISLT THEY ARE GROUNDED
        [SerializeField] protected float fallStartYVelocity = -5; // THE FORCE AT WHICH OUR CHARACTER BEGINS TO FALL WHEN THEY BECOME UNGROUNDED (RISES AS THEY FALL LONGER)
        protected bool fallingVelocityHasBeenSet = false;
        protected float InAirTimer = 0;

        [Header("Flags")]
        public bool isRolling = false;

        protected virtual void Awake( ) 
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();

            if(character.isGrounded)
            {
                // IF WE ARE NOT ATTEMPTING TO JUMP OR MOVE UPWARD 
                if (yVelocity.y < 0)
                {
                    InAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                // IF WE ARE NOT JUMPING AND OUR FALLING VELOCITY HAS NOT BEEN SET
                if (!character.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }
                InAirTimer += Time.deltaTime;
                character.animator.SetFloat("InAirTimer", InAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;
            }

            // THERE SHOULD ALWAYS BE SOME FORCE APPLIED TO THE Y VELOCITY
            character.characterController.Move(yVelocity * Time.deltaTime);

        }

        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }

        protected void OnDrawGizmosSelected()
        {
            //Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
    }
}
