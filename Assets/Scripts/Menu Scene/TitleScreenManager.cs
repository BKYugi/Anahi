using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace FT
{
    public class TitleScreenManager : MonoBehaviour
    {
        public static TitleScreenManager instance;

        [Header("Menus")]
        [SerializeField] GameObject titleScreenMainMenu;
        [SerializeField] GameObject titleScreenLoadMenu;

        [Header("Buttons")]
        [SerializeField] Button loadMenuReturnButton;
        [SerializeField] Button mainMenuLoadGameButton;
        [SerializeField] Button mainMenuNewGameButton;
        [SerializeField] Button deleteCharacterPopUpConfirmButton;

        [Header("Pop Ups")]
        [SerializeField] GameObject noCharacterSlotsPopUp;
        [SerializeField] Button noCharacterSlotsOkayButton;
        [SerializeField] GameObject deleteCharacterSlotPopUp;

        [Header("Character Slots")]
        public CharacterSlot currentSelectedSlot = CharacterSlot.NO_SLOT;

        [Header("Player")]
        [SerializeField] GameObject player;

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
        public void StartNewGame()
        {
            WorldSaveGameManager.instance.AttemptToCreateNewGame();
        }
        
        public void CreatePlayer()
        {
            if (player != null)
            {
                player = Instantiate(player, Vector3.zero, Quaternion.identity);
            }
        }
        public void OpenLoadGameMenu()
        {
            // CLOSE MAIN MENU
            titleScreenMainMenu.SetActive(false);

            // OPEN LOAD MENU
            titleScreenLoadMenu.SetActive(true);

            // FIND THE FIRST LOAD SLOT AND AUTO SELECT IT

            // SELECT THE RETURN BUTTON FIRST
            loadMenuReturnButton.Select();
        }

        public void CloseLoadGameMenu()
        {
            // CLOSE LOAD MENU
            titleScreenLoadMenu.SetActive(false);

            // OPEN MAIN MENU
            titleScreenMainMenu.SetActive(true);

            // SELECT THE LOAD BUTTON
            mainMenuLoadGameButton.Select();
        }

        public void DisplayNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(true);
            noCharacterSlotsOkayButton.Select();
        }

        public void CloseNoFreeCharacterSlotsPopUp()
        {
            noCharacterSlotsPopUp.SetActive(false);
            mainMenuNewGameButton.Select();
        }


        // CHARACTER SLOTS

        public void SelectCharacterSlot(CharacterSlot characterSlot)
        {
            currentSelectedSlot = characterSlot;
        }

        public void SelectNoSlot()
        {
            currentSelectedSlot = CharacterSlot.NO_SLOT;
        }

        public void AttemptToDeleteCharacterSlot()
        {
            if(currentSelectedSlot != CharacterSlot.NO_SLOT)
            {
                deleteCharacterSlotPopUp.SetActive(true);
                deleteCharacterPopUpConfirmButton.Select();
            }

        }

        public void DeleteCharacterSlot()
        {
            deleteCharacterSlotPopUp.SetActive(false) ;
            WorldSaveGameManager.instance.DeleteGame(currentSelectedSlot);
            // REFRESH CHARACTER SLOTS
            titleScreenLoadMenu.SetActive(false);
            titleScreenLoadMenu.SetActive(true);

            loadMenuReturnButton.Select();
        }

        public void CloseDeleteCharacterPopUp()
        {
            deleteCharacterSlotPopUp.SetActive(false );
            loadMenuReturnButton.Select();
        }
    }
}
