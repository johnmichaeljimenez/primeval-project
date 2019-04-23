using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.Data;
using Primeval.PlayerCharacter;

namespace Primeval.Item
{
    public class ItemFPSModel : MonoBehaviour
    {
        public string modelName
        {
            get{
                return modelData.itemName;
            }
        }

        public InventoryItem referenceInventoryItem;
        public ItemBaseData modelData;
    }
}