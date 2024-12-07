using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FT
{
    public class WorldSaveGameManager : MonoBehaviour
    {
        public static WorldSaveGameManager instance;

        public PlayerManager player;

        [Header("SAVE/LOAD")]
        [SerializeField] bool saveGame;
        [SerializeField] bool loadGame;

        [Header("World Scene Index")]
        [SerializeField] int worldSceneIndex = 1;

        [Header("Save Data Writer")]
        private SaveFileDataWriter saveFileDataWriter;

        [Header("Current Character Data")]
        public CharacterSlot currentCharacterSlotBeingUsed;
        public CharacterSaveData currentCharacterData;
        private string saveFileName;

        [Header("Character Slots")]
        public CharacterSaveData characterSlot01;
        public CharacterSaveData characterSlot02;
        public CharacterSaveData characterSlot03;
        public CharacterSaveData characterSlot04;
        public CharacterSaveData characterSlot05;
        public CharacterSaveData characterSlot06;
        public CharacterSaveData characterSlot07;
        public CharacterSaveData characterSlot08;
        public CharacterSaveData characterSlot09;
        public CharacterSaveData characterSlot10;




        public void Awake()
        {
            //THERE CAN ONLY BE ONE INSTANCE OF THIS SCRIPT AT ONE TIME, IF ANOTHER EXIST, DESTROY THIS
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
            DontDestroyOnLoad(gameObject);

            LoadAllCharacterProfiles();
        }

        private void Update()
        {
            if (saveGame)
            {
                saveGame = false;
                SaveGame();
            }
            if (loadGame)
            {
                loadGame = false;
                LoadGame();
            }
        }

        public string DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot characterSlot)
        {
            string fileName = "";
            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    fileName = "characterSlot_01";
                    break; 
                case CharacterSlot.CharacterSlot_02:
                    fileName = "characterSlot_02";
                    break; 
                case CharacterSlot.CharacterSlot_03:
                    fileName = "characterSlot_03";
                    break;
                case CharacterSlot.CharacterSlot_04:
                    fileName = "characterSlot_04";
                    break; 
                case CharacterSlot.CharacterSlot_05:
                    fileName = "characterSlot_05";
                    break; 
                case CharacterSlot.CharacterSlot_06:
                    fileName = "characterSlot_06";
                    break;
                case CharacterSlot.CharacterSlot_07:
                    fileName = "characterSlot_07";
                    break; 
                case CharacterSlot.CharacterSlot_08:
                    fileName = "characterSlot_08";
                    break; 
                case CharacterSlot.CharacterSlot_09:
                    fileName = "characterSlot_09";
                    break;
                case CharacterSlot.CharacterSlot_10:
                    fileName = "characterSlot_10";
                    break;
                default:
                    break;
            }
            return fileName;

        }

        public void AttemptToCreateNewGame()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            // CHECK TO SEE IF HAVE A FILE
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            // IF SLOT IS EMPTY USE THIS
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_01;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);

            // IF SLOT IS EMPTY USE THIS
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_02;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);

            // IF SLOT IS EMPTY USE THIS
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_03;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);

            // IF SLOT IS EMPTY USE THIS
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_04;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);

            // IF SLOT IS EMPTY USE THIS
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_05;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);

            // IF SLOT IS EMPTY USE THIS
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_06;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);

            // IF SLOT IS EMPTY USE THIS
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_07;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);

            // IF SLOT IS EMPTY USE THIS
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_08;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);

            // IF SLOT IS EMPTY USE THIS
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_09;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);

            // IF SLOT IS EMPTY USE THIS
            if (!saveFileDataWriter.CheckToSeeIfFileExists())
            {
                currentCharacterSlotBeingUsed = CharacterSlot.CharacterSlot_10;
                currentCharacterData = new CharacterSaveData();
                NewGame();
                return;
            }

            // IF THERE ARE NO FREE SLOTS, NOTIFY THE PLAYER
            TitleScreenManager.instance.DisplayNoFreeCharacterSlotsPopUp();

        }

        private void NewGame()
        {
            // SAVES THE NEWLY CREATED CHARACTERS STATS, ITENS (WHEN CREATION SCREEN IS ADDED)
            SaveGame();
            StartCoroutine(LoadWorldScene());
        }

        public void LoadGame()
        {
            //LOAD A PREVIOUS FILE, WITH A FILE NAME DEPENDING ON WHICH SLOT WE ARE USING
            saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();
            // GENERALLY WORKS ON MULTIPLE MACHINE TYPES 
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;
            currentCharacterData = saveFileDataWriter.LoadSaveFile();

            StartCoroutine(LoadWorldScene());
        }

        public void SaveGame()
        {
            // SAVE THE CURRENT FILE UNDER A FILE NAME DEPENDNIG ON WHICH SLOT WE ARE USING
            saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(currentCharacterSlotBeingUsed);

            saveFileDataWriter = new SaveFileDataWriter();
            // GENERALLY WORKS ON MULTIPLE MACHINE TYPES 
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = saveFileName;

            // PASS THE PLAYERS INFO, FROM GAME, TO THEIR SAVE FILE
            player.SaveGameDataToCurrentCharacterData(ref currentCharacterData);

            // WRITE THAT INFO ONTO A JSON FILE, SAVED TO THIS MACHINE
            saveFileDataWriter.CreateNewCharacterSaveFile(currentCharacterData);
        }

        public void DeleteGame(CharacterSlot characterSlot)
        {
            // CHOOSE FILE BASED ON NAME

            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;
            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(characterSlot);

            saveFileDataWriter.DeleteSaveFile();
        }

        // LOAD ALL CHARACTER PROGILES ON DEVICE WHEN STARTING GAME
        private void LoadAllCharacterProfiles()
        {
            saveFileDataWriter = new SaveFileDataWriter();
            saveFileDataWriter.saveDataDirectoryPath = Application.persistentDataPath;

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_01);
            characterSlot01 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_02);
            characterSlot02 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_03);
            characterSlot03 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_04);
            characterSlot04 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_05);
            characterSlot05 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_06);
            characterSlot06 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_07);
            characterSlot07 = saveFileDataWriter.LoadSaveFile();


            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_08);
            characterSlot08 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_09);
            characterSlot09 = saveFileDataWriter.LoadSaveFile();

            saveFileDataWriter.saveFileName = DecideCharacterFileNameBasedOncharacterSlotBeingUsed(CharacterSlot.CharacterSlot_10);
            characterSlot10 = saveFileDataWriter.LoadSaveFile();

        }

        public IEnumerator LoadWorldScene()
        {
            // USE THIS IF YOU HAVE JUST 1 SCENE IN THE WORLD
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(worldSceneIndex);

            // USE THIS IF YOU HAVE MORE THAN 1 SCENE IN THE GAME
            //AsyncOperation loadOperation = SceneManager.LoadSceneAsync(currentCharacterData.sceneIndex);


            yield return new WaitUntil(() => loadOperation.isDone);

            // Após a cena carregar completamente, procure pelo Player
            player = FindObjectOfType<PlayerManager>();

            if (player == null)
            {
                Debug.LogError("Player is null in LoadWorldScene. Confirm that it is present in the scene.");
            }
            else
            {
                // O Player foi encontrado, você pode agora executar o restante do seu código
                player.LoadGameDataFromCurrentCharacterData(ref currentCharacterData);
            }

            yield return null;
        }

        public int GetWorldSceneIndex()
        {
            return worldSceneIndex;
        }


    }
}
