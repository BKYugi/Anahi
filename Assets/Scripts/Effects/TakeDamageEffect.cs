using UnityEngine;
using UnityEngine.TextCore.Text;

namespace FT
{

    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/ Take Damage")]

    public class TakeDamageEffect : InstantCharacterEffect
    {

        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage; // If the damage is caused by another character attack it will be stored here

        [Header("Damage")]
        public float physicalDamage = 0; // (IN THE FUTURE WILL BE SPLIT INTO "STANDARD", "STRIKE", "SLASH", "PIERCE")
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;

        [Header("finalDamageDealth")]
        public int finalDamageDealt = 0; // THE DAMAGE THE CHARACTER TAKES AFTER ALL CALCULATIONS HAVE BEEN MADE

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false; // IF A CHARACTER POISE IS BROKEN, THEY WILL BE STUNNED AND PLAY DAMAGE ANIMATION
        // (TO DO)
        // BUILDS UPS
        // BUILD UP EFFECT AMOUNTS 

        [Header("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("SoundFX")]
        public bool willPLayDamageSFX = true;
        public AudioClip elementalDamageSoundFX; // USED ON TOP OF REGULAR SFX IF THERE IS ELEMENTAL DAMAGE PRESNET

        [Header("Durection Damage Taken From")]
        public float angleHitFrom; // USED TO DETERMINE THAT DAMAGE ANIMATION TO PLAY (MOVE BACKWARDS, TO THE LEFT, TO THE RIFHT, Etc)
        public Vector3 contactPoint; // USE TO DETERMINATE WHERE THE BLOOD INSTANTIATE

        public override void ProcessEffect(CharacterStatsManager character)
        {
            base.ProcessEffect(character);
            // IF THE CHARACTER IS DEAD, NO ADDITIONAL DAMAGE EFFECTS SHOULD BE PROCESSED
            if (character.isDead)
            {
                return;
            }

            // CHECK FOR "INVULNERABILITY"

            // CALCULATE DAMAGE
            CalculateDamage(character);
            // CHECK WITH DIRECTION DAMAGE CAME FROM
            // PLAY A DAMAGE ANIMATION
            PlayDirectionalBasedDamageAnimation(character.character);

            // CHECK FOR BUILD UPS (POISON, BLEED ETC)

            // PLAY SOUND FX
            PlayDamageSFX(character.character);
            // PLAY DAMAGE VFX ( BLOOD )
            PlayDamageVFX(character.character);
            // IF CHARACTER IS AI, CHECK FOR NEW TARGET IF CHARACTER CAUSING DAMAGE IS PRESENT 

        }

        private void CalculateDamage(CharacterStatsManager character)
        {
            if (characterCausingDamage != null)
            {
                // CHECK FOR DAMAGE MODIFIERS AND MODIFY BASE DAMAGE (PHYSICAL DAMAGE BUFF, ELEMENTAL DAMAGE BUFF)
                // physical *= physicalModifier (ex)
            }

            // CHECK CHARACTER FOR FLAT DEFENSES AND SUBTRACT THEM FROM THE DAMAGE 

            // CHECK CHARACTER FOR ARMOR ABSORPTION, AND SUBTRACT THE PERCENTAGE FROM THE DAMAGE

            // ADD ALL DAMAGE TYPES TOGETHER, AND APPLY THE FINAL DAMAGE
            finalDamageDealt = Mathf.RoundToInt(physicalDamage + fireDamage + magicDamage + lightningDamage + holyDamage);

            if (finalDamageDealt <= 0)
            {
                finalDamageDealt = 1;
            }
            Debug.Log("Final damage " + finalDamageDealt);
            character.CurrentHealth -= finalDamageDealt;

            // CALCULATE POISE DAMAGE TO DETERMINE IF THE CHARACTER WILL BE STUNNED
        }

        private void PlayDamageVFX(CharacterManager character)
        {
            if (character == null)
            {
                Debug.LogError("Character is null in PlayDamageVFX");
                return;
            }
            if (contactPoint == null)
            {
                Debug.LogError("ContactPoint is not set for PlayDamageVFX");
                return;
            }
            if (character.characterEffectsManager == null)
            {
                Debug.LogError("CharacterEffectsManager is null for character: " + character.name);
                return;
            }
            // IF WE HAVE FIRE DAMAGE, PLAY FIRE PARTICLES, MAGIC FOR MAGIC ETC
            character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);

        }

        private void PlayDamageSFX(CharacterManager character)
        {
            AudioClip physicalDamageSFX = WorldSoundFXManager.instance.ChooseRandomSFXFromArray(WorldSoundFXManager.instance.physicalDamageSFX);

            character.characterSoundFXManager.PlaySoundSFX(physicalDamageSFX);
            // IF FIRE DAMAGE IS GREATER THAN 0, PLAY BURN SFX, MAGIC FOR MAGIC ETC
        }

        private void PlayDirectionalBasedDamageAnimation(CharacterManager character)
        {
            // CALCULATE IF POISE IS BROKEN
            poiseIsBroken = true;
            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                // PLAY FRONT ANIMATION 
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                // PLAY FRONT ANIMATION
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.forward_Medium_Damage);
            }
            if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                // PLAY BACK ANIMATION
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.backward_Medium_Damage);
            }
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
            {
                // PLAY LEFT ANIMATION
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.left_Medium_Damage);
            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
            {
                // PLAY RIGHT ANIMATION
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.right_Medium_Damage);
            }

            // IF POISE IS BROKEN, PLAY A STAGGERING DAMAGE ANIMATION
            if (poiseIsBroken)
            {
                character.characterAnimatorManager.lastDamageAnimationPlayed = damageAnimation;
                character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
            }
        }
    }
}
