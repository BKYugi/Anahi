using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

namespace FT
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;
        public WeaponModelInstantiationSlot rightHandSlot;
        public WeaponModelInstantiationSlot leftHandSlot;

        [SerializeField] WeaponManager rightWeaponManager;
        [SerializeField] WeaponManager leftWeaponManager;

        public GameObject rightHandWeaponModel;
        public GameObject leftHandWeaponModel;


        private protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
            // GET OUR SLOTS
            InitializeWeaponSlots();
        }

        protected override void Start()
        {
            base.Start();

            LoadWeaponOnBothHands();
        }

        private void InitializeWeaponSlots()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach (var weaponSlot in weaponSlots)
            {
                if(weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
                {
                    leftHandSlot = weaponSlot;
                }

            }
        }

        public void LoadWeaponOnBothHands()
        {
            LoadRightWeapon();
            LoadLeftWeapon();

        }

        // RIGHT WEAPON
        public void SwitchRightWeapon()
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

            WeaponItem selectedWeapon = null;

            // DISABLE TWO HANDING IF WE ARE TWO HANDING
            // CHECK OUR WEAPON INDEX(WE HAVE 3 SLOTS, SO THATS 3 POSSIBLE NUMBERS;
            // ADD ONE TO INDEX TO SWITCH
            player.playerInventoryManager.rightHandWeaponIndex += 1;

            // IF OUR INDEX IS OUT OF BONDS, RESET IT TO POSITION 1
            if (player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
            {
                player.playerInventoryManager.rightHandWeaponIndex = 0;

                // WE CHECK IF WE ARE HOLDING MORE THAN ONE WEAPON
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                    {
                        weaponCount++;
                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.rightHandWeaponIndex = -1;
                    selectedWeapon = Instantiate(WorldItemDatabase.instance.unarmedWeapon);
                    player.CurrentRightHandWeaponID = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                    player.CurrentRightHandWeaponID = firstWeapon.itemID;
                }
                return;
            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
            {
                // CHECK TO SEE IF THIS IS NOT UNARMED WEAPON
                // IF THE NEXT POTENTIAL WEAPON DOES NOT EQUAL UNARMED 
                if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];
                    // ASSIGN THE ID
                    player.CurrentRightHandWeaponID = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                    return;
                }
            }
            if (selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
            {
                SwitchRightWeapon();
            }


        }

        public void LoadRightWeapon()
        {
            if(player.playerInventoryManager.currentRightHandWeapon != null)
            {
                // REMOVE THE OLD WEAPON
                rightHandSlot.UnloadWeapon();

                // BRING THE NEW WEAPON
                rightHandWeaponModel = Instantiate (player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
                // ASSIGN WEAPONS DAMAGE, TO ITS COLLIDER 
            }
        }

        // LEFT WEAPON 
        public void SwitchLeftWeapon()
        {
            player.playerAnimatorManager.PlayTargetActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

            WeaponItem selectedWeapon = null;

            // DISABLE TWO HANDING IF WE ARE TWO HANDING
            // CHECK OUR WEAPON INDEX(WE HAVE 3 SLOTS, SO THATS 3 POSSIBLE NUMBERS;
            // ADD ONE TO INDEX TO SWITCH
            player.playerInventoryManager.leftHandWeaponIndex += 1;

            // IF OUR INDEX IS OUT OF BONDS, RESET IT TO POSITION 1
            if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
            {
                player.playerInventoryManager.leftHandWeaponIndex = 0;

                // WE CHECK IF WE ARE HOLDING MORE THAN ONE WEAPON
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;

                for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInLeftHandSlots[i].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                    {
                        weaponCount++;
                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.leftHandWeaponIndex = -1;
                    selectedWeapon = Instantiate(WorldItemDatabase.instance.unarmedWeapon);
                    player.CurrentLeftHandWeaponID = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                    player.CurrentLeftHandWeaponID = firstWeapon.itemID;
                }
                return;
            }

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInLeftHandSlots)
            {
                // CHECK TO SEE IF THIS IS NOT UNARMED WEAPON
                // IF THE NEXT POTENTIAL WEAPON DOES NOT EQUAL UNARMED 
                if (player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDatabase.instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];
                    // ASSIGN THE ID
                    player.CurrentLeftHandWeaponID = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID;
                    return;
                }
            }
            if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
            {
                SwitchLeftWeapon();
            }


        }

        public void LoadLeftWeapon()
        {
            if (player.playerInventoryManager.currentLeftHandWeapon != null)
            {
                // REMOVE THE OLD WEAPON
                leftHandSlot.UnloadWeapon();

                // BRING THE NEW WEAPON
                leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
                leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
                // ASSIGN WEAPONS DAMAGE, TO ITS COLLIDER 

            }
        }

        // DAMAGE COLLIDERS
        public void OpenDamageCollider()
        {
            // OPEN RIGHT WEAPON DAMAGE COLLIDER
            if (player.isUsingRightHand)
            {
                rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
            }
            // OPEN LEFT WEAPON DAMAGE COLLIDER
            else if (player.isUsingLeftHand)
            {
                leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
            }

            // PLAY SFX
        }

        public void CloseDamageCollider()
        {
            // OPEN RIGHT WEAPON DAMAGE COLLIDER
            if (player.isUsingRightHand)
            {
                rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
            // OPEN LEFT WEAPON DAMAGE COLLIDER
            else if (player.isUsingLeftHand)
            {
                leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }

        }
    }
}
