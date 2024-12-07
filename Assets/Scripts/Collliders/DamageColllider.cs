using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace FT {
    public class DamageColllider : MonoBehaviour
    {
        [Header("Collider")]
        [SerializeField] protected Collider damageCollider;

        [Header("Damage")]
        public float physicalDamage = 0; // (IN THE FUTURE WILL BE SPLIT INTO "STANDARD", "STRIKE", "SLASH", "PIERCE")
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("Contact Point")]
        protected Vector3 contactPoint;

        [Header("Characters Damage")]
        protected List<CharacterStatsManager> charactersDamaged = new List<CharacterStatsManager>();

        protected virtual void Awake()
        {

        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            // CHECK THE OBJECT IS A CHARACTER
            CharacterStatsManager damageTarget = other.GetComponentInParent<CharacterStatsManager>();
            // IF YOU WANT TO SEARCH ON BOTH THE DAMAGE CHARACTERS COLLIDERS & THE CHARACTER CONTROLLER COLLIDER JUST CHECK FOR NULL HERE AND DO THE FOLLWING 

            /*if (damageTarget == null) 
            {
                damageTarget = other.GetComponent<CharacterStatsManager>();
            }*/

            if(damageTarget != null )
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // CHECK IF WE CAN DAMAGE THIS TARGET BASED ON FRIENDLY FIRE

                // CHECK IF TARGET IS BLOCKING

                // CHECK IF TARGET IS INVULNERABLE

                // DAMAGE

                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterStatsManager damageTarget)
        {
            // WE DONT WANT TO DAMAGE THE SAME TARGET MORE THAN ONCE IN A SINGLE ATTACK 
            // SO WE ADD THEM TO A LIST THAT CHECKS BEFORE APPLYINGDAMAGE
            if (charactersDamaged.Contains(damageTarget))
            {
                return;
            }
            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.instance.takeDamageEffect);
            damageEffect.physicalDamage = physicalDamage;
            damageEffect.magicDamage = magicDamage;
            damageEffect.fireDamage = fireDamage;
            damageEffect.lightningDamage = lightningDamage;
            damageEffect.holyDamage = holyDamage;

            damageTarget.characterEffectsManager.ProcessInstantEffects(damageEffect);


            charactersDamaged.Remove(damageTarget);
        }

        public virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            charactersDamaged.Clear(); // WE RESET THE CHARACTERS THAT HAVE BEEN HIT WHEN WE RESET THE COLLIDERS
        }
    }
}
