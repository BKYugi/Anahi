using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace FT
{
    public class MeleeWeaponDamageCollider : DamageColllider
    {

        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage; // WHEN CALCULATING DAMAGE, THIS IS USED TO CHECK FOR ATTACKERS DAMAGE MODIFIERS, EFFECTS, ETC

        [Header("Weapon Attack Modifiers")]
        public float light_Attack_01_Modifier;
        public float light_Attack_02_Modifier;
        public float heavy_Attack_01_Modifier;
        public float heavy_Attack_02_Modifier;
        public float charged_Attack_01_Modifier;
        public float charged_Attack_02_Modifier;

        protected override void Awake()
        {
            base.Awake();

            if(damageCollider == null)
            {
                damageCollider = GetComponent<Collider>();
            }

            damageCollider.enabled = false; // WEAPON COLLIDERS ARE DISABLED AT START, ONLY ENABLED WHEN ANIMATIONS ALLOW
        }

        protected override void OnTriggerEnter(Collider other)
        {
            // CHECK THE OBJECT IS A CHARACTER
            CharacterStatsManager damageTarget = other.GetComponentInParent<CharacterStatsManager>();

            // DONT CAUSE DAMAGE TO ITSELF
            if(damageTarget == characterCausingDamage.characterStatsManager)
            {
                return;
            }
            // IF YOU WANT TO SEARCH ON BOTH THE DAMAGE CHARACTERS COLLIDERS & THE CHARACTER CONTROLLER COLLIDER JUST CHECK FOR NULL HERE AND DO THE FOLLWING 

            /*if (damageTarget == null) 
            {
                damageTarget = other.GetComponent<CharacterStatsManager>();
            }*/

            if (damageTarget != null)
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // CHECK IF WE CAN DAMAGE THIS TARGET BASED ON FRIENDLY FIRE

                // CHECK IF TARGET IS BLOCKING

                // CHECK IF TARGET IS INVULNERABLE

                // DAMAGE
                DamageTarget(damageTarget);
            }
        }

        protected override void DamageTarget(CharacterStatsManager damageTarget)
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
            damageEffect.angleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

            switch(characterCausingDamage.characterCombatManager.currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApplyAttackDamageModifiers(light_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.LightAttack02:
                    ApplyAttackDamageModifiers(light_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack01:
                    ApplyAttackDamageModifiers(heavy_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack02:
                    ApplyAttackDamageModifiers(heavy_Attack_02_Modifier, damageEffect);
                    break;
                case AttackType.ChargedAttack01:
                    ApplyAttackDamageModifiers(charged_Attack_01_Modifier, damageEffect);
                    break;
                case AttackType.ChargedAttack02:
                    ApplyAttackDamageModifiers(charged_Attack_02_Modifier, damageEffect);
                    break;
                default: 
                    break;
            }

            //damageTarget.characterEffectsManager.ProcessInstantEffects(damageEffect);
            damageTarget.character.ProcessCharacterDamage(
                damageTarget.character.playerManager,
                characterCausingDamage.playerManager,
                damageEffect.physicalDamage,
                damageEffect.magicDamage,
                damageEffect.fireDamage,
                damageEffect.holyDamage,
                damageEffect.poiseDamage,
                damageEffect.lightningDamage,
                damageEffect.angleHitFrom,
                damageEffect.contactPoint.x,
                damageEffect.contactPoint.y,
                damageEffect.contactPoint.z
                );
            
        }

        private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
        {
            damage.physicalDamage *= modifier;
            damage.magicDamage *= modifier;
            damage.fireDamage *= modifier;
            damage.holyDamage *= modifier;
            damage.poiseDamage *= modifier;
            damage.lightningDamage *= modifier;

            // IF ATTACK IS A FULLY CHARGED HEAVY, MULTIPLY BY FULL CHARGE MODIFIER AFTER NORMAL MODIFIER HAVE BEEN CALCULATED
        }

    }
}
