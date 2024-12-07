using UnityEngine;

namespace FT
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        
        // PROCESS INSTANT EFFECTS (TAKING DAMAGE, HEAL)

        // PROCESS OVERTIME EFFECTS (POISON, BUILD UPS)

        // PROCESS STATIC EFFECTS (ADDING OR REMOVING BUFFS FROM ITENS)
        CharacterStatsManager character;

        [Header("VFX")]
        [SerializeField] GameObject bloodSplatterVFX;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterStatsManager>();
        }
        public virtual void ProcessInstantEffects(InstantCharacterEffect effect)
        {
            // TAKE IN AN EFFECT
            // PROCESS IT
            effect.ProcessEffect(character);
        }

        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            // IF WE MANYALLY HAVE PLACED A BLOOD SPLATTER VFX ON THIS MODEL, PLAY ITS VERSION
            if(bloodSplatterVFX != null)
            {
                GameObject bloodSplatter = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            else
            {
                GameObject bloodSplatter = Instantiate(WorldCharacterEffectsManager.instance.bloodSplatterVFX, contactPoint, Quaternion.identity);

            }
        }
    }
}
