using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace FT
{
    public class WorldItemDatabase : MonoBehaviour
    {

        public static WorldItemDatabase instance;

        public WeaponItem unarmedWeapon;

        [Header("Weapons")]
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();

        // A LIST OF EVERY ITEM WE HAVE IN THE GAME
        [Header("Items")]
        private List<Item> items = new List<Item>();

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

            // ADD ALL WEAPONS TO ITEMS LIST
            foreach (var weapon in weapons) 
            {
                items.Add(weapon);
            }

            // ASSIGN ALL OF OUR ITEMS A UNIQUE ID 
            for(int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }

        }

        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }

    }
}