using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Primeval.Data
{
    public class ItemBaseData : ScriptableObject
    {
        [Header("Descriptions")]
        public string itemName;
        public string itemDescription;
        public Sprite iconSprite;

        [Header("Inventory")]
        [Range(0, 50)]
        public int weight = 0;
        public int amount = 1;
        public bool canBeEquipped;
        public KeyBindings defaultKeyBinding;
        public int maxStackAmount = 1;
        
        [Header("Item")]
        public ItemTypes itemType;
        public UsageTypes usageType;
        
        [Header("Audio")]
        public AudioClip useClip;

    }


    public enum ItemTypes
    {
        FuelCell, Weapon, Aid, Armor, Tools, Ammo
    }

    public enum UsageTypes
    {
        Normal, OneTime
    }

    public enum KeyBindings
    {
        None = -1,
        Alpha1 = KeyCode.Alpha1,
        Alpha2 = KeyCode.Alpha2,
        Alpha3 = KeyCode.Alpha3,
        Alpha4 = KeyCode.Alpha4,
        Alpha5 = KeyCode.Alpha5,
        Alpha6 = KeyCode.Alpha6,
        Alpha7 = KeyCode.Alpha7,
        Alpha8 = KeyCode.Alpha8,
        Alpha9 = KeyCode.Alpha9
    }
}