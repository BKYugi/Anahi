using NUnit.Framework;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

namespace FT
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager instance;

        [Header("VFX")]
        public GameObject bloodSplatterVFX;

        [Header("Damage")]
        public TakeDamageEffect takeDamageEffect;

        [SerializeField] List<InstantCharacterEffect> instantEffects;


        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
            GenerateEffectIDs();
        }

        public void GenerateEffectIDs()
        {
            for(int i = 0; i < instantEffects.Count; i++)
            {
                instantEffects[i].instantEffectID = i;
            }
        }

    }
}
