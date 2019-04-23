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
        public KeyCode defaultKeyCode;
        public int maxStackAmount = 1;
        
        [Header("Item")]
        public ItemTypes itemType;
        public UsageTypes usageType;

    }


    public enum ItemTypes
    {
        FuelCell, Weapon, Aid, Armor, Tools, Ammo
    }

    public enum UsageTypes
    {
        Normal, OneTime
    }
}