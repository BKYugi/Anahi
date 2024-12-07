using UnityEngine;

namespace FT
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/ Take Stamina Damage")]
    public class TakeStaminaDamageCharacterEffect : InstantCharacterEffect
    {
        public float staminaDamage;
        public PlayerStatsManager playerStatsManager;

        private void Awake()
        {
            playerStatsManager = FindAnyObjectByType<PlayerStatsManager>();
        }
        public override void ProcessEffect(CharacterStatsManager character)
        {
            CalculateStaminaDamage(character);
        }

        private void CalculateStaminaDamage(CharacterStatsManager character)
        {
            // COMPARED THE BASE STMAINA DAMAGE AGAINST OTHER PLAYER EFFECTS/MODIFIERS
            // CHANGE THE VALUE BEFORE SUBTRACTING/ ADDING IT
            // PLAY SOUND FX OR VFX DURING EFFECTS
            character.CurrentStamina -= staminaDamage;
        }

    }
}
