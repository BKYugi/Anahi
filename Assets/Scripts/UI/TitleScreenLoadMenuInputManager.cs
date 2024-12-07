using UnityEngine;

namespace FT
{
    public class TitleScreenLoadMenuInputManager : MonoBehaviour
    {

        [SerializeField] PlayerControls playerControls;

        [Header("Title Screen Inputs")]
        [SerializeField] bool deleteCharacterSlot = false;

        private void Update()
        {
            if(deleteCharacterSlot)
            {
                deleteCharacterSlot = false;
                TitleScreenManager.instance.AttemptToDeleteCharacterSlot();
            }
        }

        private void OnEnable()
        {
            if(playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.UI.X.performed += i => deleteCharacterSlot = true;
            }

            playerControls.Enable();
        }
        private void OnDisable()
        {
            playerControls.Disable();

        }

    }
}
