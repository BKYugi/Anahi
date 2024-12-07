using UnityEngine;
namespace FT
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("Debug Delete Later")]
        [SerializeField] InstantCharacterEffect effectToTest;
        [SerializeField] bool processEffect = false;


        private void Update()
        {
            if (processEffect)
            {
                processEffect = false;
                // WHEN WE INSTANTIATE IT, THE ORIGINAL IS NOT EFFECTED
                TakeStaminaDamageCharacterEffect effect = Instantiate(effectToTest) as TakeStaminaDamageCharacterEffect;
                effect.staminaDamage = 55;
                ProcessInstantEffects(effect);

                // WE DONT INSTANTIA IT, THE ORIGINAL IS CHANGED ( YOU DONT WANT THIS IN MOST CASES)
                //effectToTest.staminaDamage = 55;
                //ProcessInstantEffect(effectToTest);
                // THIS CHANGE THE BASE VALUE OF THE ORIGINAL OBJECT
            }
        }
    }
}
