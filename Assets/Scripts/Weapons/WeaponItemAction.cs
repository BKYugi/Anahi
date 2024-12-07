using UnityEngine;

namespace FT
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Test Action")]
    public class WeaponItemAction : ScriptableObject
    {

        public int actionID;

        public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            // WHAT DOES EVERY WEAPON ACTION HAVE IN COMMON?
            // 1. WE SHOULD AWAYS KEEP TRACK OF HICH WEAPON IS CURRENTLY BEING USED
            playerPerformingAction.CurrentWeaponBeingUsedID = weaponPerformingAction.itemID;
        }
        
        
    }
}