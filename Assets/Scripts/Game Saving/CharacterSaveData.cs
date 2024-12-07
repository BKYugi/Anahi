using UnityEngine;
namespace FT
{

    [System.Serializable]
    // SINCE WE WANT TO REFERENCE THIS DATA FOR EVERY SAVE FILE, THIS SCRIPT IS NOT A MONOBEHAVIOUR AND IS INSTEAD SERIALIZABLE 
    public class CharacterSaveData
    {

        [Header("Scene Index")]
        public int sceneIndex;

        [Header("Character Name")]
        public string characterName = "Anahi";

        [Header("Time Played")]
        public float secondsPlayed;


        // WHY NOT USER A VECTOR 3 - CAN SAVE DATA ONLY FROM "BASIC" VARIABLE DATAS (STRING, FLOAT, INT ETC)
        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Resources")]
        public float currentHealth;
        public float currentStamina;

        [Header("Stats")]
        public int vitality;
        public int endurance;
    }
}
