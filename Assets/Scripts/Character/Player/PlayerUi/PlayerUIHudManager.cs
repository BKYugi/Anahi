using UnityEngine;
using UnityEngine.UI;

namespace FT
{
    public class PlayerUIHudManager : MonoBehaviour
    {
        [Header("STAT BARS")]
        [SerializeField] UI_StatBar healthBar;
        [SerializeField] UI_StatBar staminaBar;

        [Header("QUICK SLOTS")]
        [SerializeField] Image rightWeaponQuickSlotIcon;
        [SerializeField] Image leftWeaponQuickSlotIcon;


        public void RefreshHUD()
        {
            if (healthBar == null || staminaBar == null)
            {
                Debug.LogWarning("HealthBar or StaminaBar is not assigned in PlayerUIHudManager.");
                return;
            }

            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);

            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);

        }

        public void SetNewHealthValue(float oldValue, float newValue)
        {
            //healthBar.SetStat((newValue));
            if (healthBar != null)
                healthBar.SetStat(newValue);
            else
                Debug.LogWarning("HealthBar is not assigned.");
        }

        public void SetMaxHealthValue(float maxHealth)
        {
            healthBar.SetMaxStat(maxHealth);
        }

        public void SetNewStaminaValue(float oldValue, float newValue)
        {
            staminaBar.SetStat((newValue));
        }

        public void SetMaxStaminaValue(float maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }

        public void UpdateHealthAndStaminaBars(float maxHealth, float maxStamina)
        {
            healthBar.SetMaxStat(maxHealth);
            staminaBar.SetMaxStat(maxStamina);
        }

        public void SetRightWeaponQuickSlotIcon(int weaponID)
        {
            // 1. Method - Directly reference the right weapon in the hand of the player
            // Pros: Its super straight forward
            // Cons: If you forget to call this function AFTER you ve loaded your weapons first, it will give you an error
            // Example: You load a previosuly saved game, you go to reference the weapons upon loading UI but they arent instantied yet
            // Final Notes: This method is perfectly fine if you remember your order of opereations

            // 2. Method - REQUIRE an item ID of the weapon, fetch the weapon from our database and use it to get the weapon items icon
            // Pros: Since you always save the current weapons ID, we dont need to wait to et it from the player we could get it before hand if require
            // Cons: Its not as direct
            // Final Notes: This method is great if you dont want to remember another order of operations

            // IF THE DATABASE DOES NOT CONTAIN A WEAPON MATCHING THE GIVEN ID, RETURN
            WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponID);
            if(WorldItemDatabase.instance.GetWeaponByID(weaponID) == null)
            {
                Debug.Log(" ITEM IS NULL ");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
                return;
            }

            if(weapon.itemIcon == null)
            {
                Debug.Log(" ITEM HAS NO ICON ");
                rightWeaponQuickSlotIcon.enabled = false;
                rightWeaponQuickSlotIcon.sprite = null;
            }

            // THIS IS WHERE YOU WOULD CHECK TO SEE IF YOU MEET THE ITEM REQUIREMENTS IF YOU WANT TO CREATE THE WARNIG FOR NOT BEING ABLE TO WIRLD IT IN THE UI
            rightWeaponQuickSlotIcon.enabled = true;
            rightWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        }

        public void SetLeftWeaponQuickSlotIcon(int weaponID)
        {
            // 1. Method - Directly reference the right weapon in the hand of the player
            // Pros: Its super straight forward
            // Cons: If you forget to call this function AFTER you ve loaded your weapons first, it will give you an error
            // Example: You load a previosuly saved game, you go to reference the weapons upon loading UI but they arent instantied yet
            // Final Notes: This method is perfectly fine if you remember your order of opereations

            // 2. Method - REQUIRE an item ID of the weapon, fetch the weapon from our database and use it to get the weapon items icon
            // Pros: Since you always save the current weapons ID, we dont need to wait to et it from the player we could get it before hand if require
            // Cons: Its not as direct
            // Final Notes: This method is great if you dont want to remember another order of operations

            // IF THE DATABASE DOES NOT CONTAIN A WEAPON MATCHING THE GIVEN ID, RETURN
            WeaponItem weapon = WorldItemDatabase.instance.GetWeaponByID(weaponID);
            if (WorldItemDatabase.instance.GetWeaponByID(weaponID) == null)
            {
                Debug.Log(" ITEM IS NULL ");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
                return;
            }

            if (weapon.itemIcon == null)
            {
                Debug.Log(" ITEM HAS NO ICON ");
                leftWeaponQuickSlotIcon.enabled = false;
                leftWeaponQuickSlotIcon.sprite = null;
            }

            // THIS IS WHERE YOU WOULD CHECK TO SEE IF YOU MEET THE ITEM REQUIREMENTS IF YOU WANT TO CREATE THE WARNIG FOR NOT BEING ABLE TO WIRLD IT IN THE UI
            leftWeaponQuickSlotIcon.enabled = true;
            leftWeaponQuickSlotIcon.sprite = weapon.itemIcon;
        }
    }
}
