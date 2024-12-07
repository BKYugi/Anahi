using UnityEngine;

namespace FT
{
    public class InstantCharacterEffect : ScriptableObject
    {

        [Header("Effect ID")]
        public int instantEffectID;

        public virtual void ProcessEffect(CharacterStatsManager character)
        {

        }

    }
}

