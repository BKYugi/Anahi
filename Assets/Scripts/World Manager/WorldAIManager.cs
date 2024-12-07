using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FT
{
    public class WorldAIManager : MonoBehaviour
    {
        public static WorldAIManager instance;

        [Header("Debug")]
        [SerializeField] bool despawnCharacters = false;
        [SerializeField] bool spawnCharacters = false;


        [Header("Characters")]
        [SerializeField] GameObject[] aiCharacters;
        [SerializeField] List<GameObject> spawnedInCharacters;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            StartCoroutine(WaitForSceneToLoadThenSpawnCharacters());
        }

        private void Update()
        {
            if(despawnCharacters)
            {
                despawnCharacters = false;
                DespawnAllCharacters();
            }
            if(spawnCharacters)
            {
                spawnCharacters = false;
                SpawnAllCharacter();
            }
        }

        private IEnumerator WaitForSceneToLoadThenSpawnCharacters()
        {
            while (!SceneManager.GetActiveScene().isLoaded)
            {
                yield return null; 
            }

            SpawnAllCharacter();
        }

        private void SpawnAllCharacter()
        {
            foreach (var character in aiCharacters)
            {
                GameObject instantiatedCharacter = Instantiate(character);
                spawnedInCharacters.Add(instantiatedCharacter );


                Debug.Log($"Spawned AI Character: {instantiatedCharacter.name}");
            }
        }

        private void DespawnAllCharacters()
        {
            foreach (var character in spawnedInCharacters)
            {
                if(character != null) 
                {
                    Destroy(character);
                }
                
            }


            spawnedInCharacters.Clear();
        }

        private void DisableAllCharacter()
        {
            // TO DO DISABLE CHARACTER GAME OBJECTS, SYNC DISABLE
            // DISABLE GAMEOBJECTS WHEN QUIT
            // CAN BE USED TO DISABLE CHARACTER THAT ARE FAR FROM PLAYERS TO SAVE MEMORY
            // CHARACTER CAN BE SPLIT INTO AREAS INTO AREAS (AREA_00, AREA_01 ETC)
        }
    }
}