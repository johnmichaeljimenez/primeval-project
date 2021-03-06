﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.PlayerCharacter;
using Primeval.Data;
using Photon;

namespace Primeval.Item
{
    public class ItemBase : PunBehaviour
    {
        public ItemBaseData itemData;

        public int currentAmount = 0;
        public int maxAmount = 0;

        Rigidbody rigidbody;

        SyncInventoryItem pastData;

        void Start()
        {
            if (photonView.instantiationData != null && photonView.instantiationData.Length > 0)
                pastData = (SyncInventoryItem)(photonView.instantiationData[0]);

            currentAmount = itemData.amount;
            maxAmount = itemData.amount;

            if (pastData != null)
            {
                currentAmount = pastData.amount;
            }


            if (photonView.isMine)
            {
                Initialize();
            }
        }

        public virtual void Initialize()
        {
            rigidbody = GetComponent<Rigidbody>();
            rigidbody.isKinematic = !photonView.isMine;
        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnPickup(GameObject p)
        {
            PlayerCharacter.PlayerCharacter player = p.GetComponent<PlayerCharacter.PlayerCharacter>();

            if (player.photonView.isMine)
            {
                int playerWeight = player.inventoryModule.currentWeight;
                int maxWeight = player.inventoryModule.capacity;
                int passedAmount = 0;
                int failSafe = 1000;

                if (currentAmount == 1 && playerWeight + itemData.weight > maxWeight)
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
                i.prefab = itemData.itemName;
                i.data = itemData;
                i.currentAmount = passedAmount;
                if (itemData.itemType == ItemTypes.Weapon)
                {
                    if (pastData != null)
                        i.currentAmmo = pastData.currentAmmo;
                    else
                        i.currentAmmo = ((WeaponData)itemData).ammo1Capacity;
                }
                i.owner = player;
                player.inventoryModule.AddItem(i);

                player.CmdRemove(photonView.viewID, currentAmount);
            }
        }

        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {

        }
    }

}