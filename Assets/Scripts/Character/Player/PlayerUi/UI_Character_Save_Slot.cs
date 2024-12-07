using TMPro;
using UnityEngine;

namespace FT
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {

        SaveFileDataWriter saveFileWriter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character info")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timePLayed;

        private void OnEnable()
        {
            LoadSaveSlots();
        }

        private void LoadSaveSlots()
        {
            saveFileWriter = new SaveFileDataWriter();
            saveFileWriter.saveDataDirectoryPath = Application.persistentDataPath;

            switch (characterSlot)
            {
                // SAVE SLOT 1
                case CharacterSlot.CharacterSlot_01:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOncharacterSlotBeingUsed(characterSlot);
                    // IF FILE EXIST, GET THE INFO
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot01.characterName;
                    }
                    // IF DOES NOT, DISABLE
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                    // SAVE SLOT 2
                case CharacterSlot.CharacterSlot_02:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOncharacterSlotBeingUsed(characterSlot);
                    // IF FILE EXIST, GET THE INFO
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot02.characterName;
                    }
                    // IF DOES NOT, DISABLE
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                    // SAVE SLOT 3
                case CharacterSlot.CharacterSlot_03:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOncharacterSlotBeingUsed(characterSlot);
                    // IF FILE EXIST, GET THE INFO
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot03.characterName;
                    }
                    // IF DOES NOT, DISABLE
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                    // SAVE SLOT 4
                case CharacterSlot.CharacterSlot_04:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOncharacterSlotBeingUsed(characterSlot);
                    // IF FILE EXIST, GET THE INFO
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot04.characterName;
                    }
                    // IF DOES NOT, DISABLE
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                    // SAVE SLOT 5
                case CharacterSlot.CharacterSlot_05:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOncharacterSlotBeingUsed(characterSlot);
                    // IF FILE EXIST, GET THE INFO
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot05.characterName;
                    }
                    // IF DOES NOT, DISABLE
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                    // SAVE SLOT 6
                case CharacterSlot.CharacterSlot_06:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOncharacterSlotBeingUsed(characterSlot);
                    // IF FILE EXIST, GET THE INFO
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot06.characterName;
                    }
                    // IF DOES NOT, DISABLE
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                    // SAVE SLOT 7
                case CharacterSlot.CharacterSlot_07:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOncharacterSlotBeingUsed(characterSlot);
                    // IF FILE EXIST, GET THE INFO
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot07.characterName;
                    }
                    // IF DOES NOT, DISABLE
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                    // SAVE SLOT 8
                case CharacterSlot.CharacterSlot_08:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOncharacterSlotBeingUsed(characterSlot);
                    // IF FILE EXIST, GET THE INFO
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot08.characterName;
                    }
                    // IF DOES NOT, DISABLE
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                    // SAVE SLOT 9
                case CharacterSlot.CharacterSlot_09:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOncharacterSlotBeingUsed(characterSlot);
                    // IF FILE EXIST, GET THE INFO
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot09.characterName;
                    }
                    // IF DOES NOT, DISABLE
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                    // SAVE SLOT 10 
                case CharacterSlot.CharacterSlot_10:
                    saveFileWriter.saveFileName = WorldSaveGameManager.instance.DecideCharacterFileNameBasedOncharacterSlotBeingUsed(characterSlot);
                    // IF FILE EXIST, GET THE INFO
                    if (saveFileWriter.CheckToSeeIfFileExists())
                    {
                        characterName.text = WorldSaveGameManager.instance.characterSlot10.characterName;
                    }
                    // IF DOES NOT, DISABLE
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
            }
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.instance.LoadGame();
        }

        public void SelectCurrentSlot()
        {
            TitleScreenManager.instance.SelectCharacterSlot(characterSlot);
        }

    }
}
