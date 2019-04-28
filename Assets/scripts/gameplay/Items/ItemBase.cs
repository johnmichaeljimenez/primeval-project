using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Primeval.PlayerCharacter;
using Primeval.Data;

namespace Primeval.Item
{
    public class ItemBase : NetworkBehaviour
    {
        public ItemBaseData itemData;

        public int currentAmount = 0;
        public int maxAmount = 0;

        Rigidbody rigidbody;

        public override void OnStartLocalPlayer()
        {
            Initialize();
        }

        void Start()
        {
            currentAmount = itemData.amount;
            maxAmount = itemData.amount;
        }

        public virtual void Initialize()
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.isKinematic = !isLocalPlayer;
        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnPickup(GameObject p)
        {
            PlayerCharacter.PlayerCharacter player = p.GetComponent<PlayerCharacter.PlayerCharacter>();

            if (player.isLocalPlayer)
            {
                int playerWeight = player.inventoryModule.currentWeight;
                int maxWeight = player.inventoryModule.capacity;
                int passedAmount = 0;
                int failSafe = 1000;
                
                if (currentAmount == 1 && playerWeight+itemData.weight > maxWeight)
                {
                    player.audioPlayerModule.PlaySound(player.inventoryModule.overCapacityClip, true);
                    // print(currentAmount);
                    // print(itemData.weight);
                    // print(playerWeight);
                    return;
                }


                while (playerWeight < maxWeight && passedAmount < currentAmount)
                {
                    passedAmount += 1;
                    playerWeight += itemData.weight;

                    failSafe -= 1;
                    if (failSafe <= 0)
                    {
                        print("WHILE LOOP ERROR");
                        break;
                    }
                }

                if (passedAmount == 0)
                {
                    player.audioPlayerModule.PlaySound(player.inventoryModule.overCapacityClip, true);
                    return;
                }

                currentAmount -= passedAmount;
                player.audioPlayerModule.PlaySound(player.inventoryModule.addItemClip, true);

                InventoryItem i = new InventoryItem();
                i.data = itemData;
                i.currentAmount = passedAmount;
                if (itemData.itemType == ItemTypes.Weapon)
                {
                    i.currentAmmo = ((WeaponData)itemData).ammo1Capacity;
                }
                i.owner = player;
                player.inventoryModule.AddItem(i);

                player.CmdRemove(gameObject, currentAmount);
            }
        }
    }

}