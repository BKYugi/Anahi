using UnityEngine;

namespace FT
{
    public class WeaponItem : Item
    {
        // ANIMATOR CONTROLLER OVERRIDE (CHANGE ATTACK ANIMATIONS BASED ON WEAPON YOU ARE USING)

        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirements")]
        public int strengthREQ = 0;
        public int dexREQ = 0;
        public int intREQ = 0;
        public int faithREQ = 0;

        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;
        public int magicDamage = 0;
        public int fireDamage = 0;
        public int lightningDamage = 0;
        public int holyDamage = 0;

        // WEAPON GUARD ABSORPTIONS (BLOCKING POWER)

        [Header("Weapon Base Poise Damage")]
        public float poiseDamage = 10;
        // OFFENSIVE POISE BONUS WHEN ATTACKING 

        [Header("Attack Modifiers")]
        // WEAPON MODIFIERS
        // LIGHT ATTACK MODIFIER
        public float light_Attack_01_Modifier = 1;
        public float light_Attack_02_Modifier = 1.2f;
        public float heavy_Attack_01_Modifier = 1.5f;
        public float heavy_Attack_02_Modifier = 1.8f;
        public float charged_Attack_01_Modifier = 2;
        public float charged_Attack_02_Modifier = 2.5f;

        // HEAVY ATTACK MODIFIER
        // CRITICAL DAMAGE MODIFIER

        [Header("Stamina Costs")]
        public int baseStaminaCost = 20;
        // RUNNING ATTACK STAMINA COST MODIFIER
        // LIGHT ATTACK STAMINA COST MODIFIER
        public float lightAttackStaminaCostMultiplier = 1;
        // HEAVY ATTACK STAMINA COST MODIFIER

        // ITEM BASED ACTIONS (RB, RT, LB, LT)
        [Header("Actions")]
        public WeaponItemAction oh_RB_Action; // ONE HANDED RIGHT BUMPER
        public WeaponItemAction oh_RT_Action; // ONE HANDED RIGHT TRIGGER


        // ASH OF AR

        // BLOCKING SOUNDS

    }
}
