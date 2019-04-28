using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.ViewModels;
using Primeval.Item;
using Primeval.Data;
using Mirror;

namespace Primeval.PlayerCharacter
{
    public class Inventory : PlayerModuleBase
    {
        int mFuelCount;
        public int fuelCount
        {
            get
            {
                return mFuelCount;
            }
            private set
            {
                mFuelCount = value;

                if (playerCharacter.isLocalPlayer)
                {
                    VMFuelCount.instance.Text = mFuelCount.ToString();
                }
            }
        }

        public int currentWeight
        {
            get;
            private set;
        }

        public List<ItemBaseData> allItemData;
        public List<InventoryItem> itemList = new List<InventoryItem>();
        int currentEquippedItem;
        public int capacity = 120;

        public AudioClip addItemClip, overCapacityClip;

        public override void Initialize()
        {
            base.Initialize();

            itemList = new List<InventoryItem>();
            VMInventory.instance.items.Clear();
            CalculateFuelCount();
            CalculateWeight();

            currentEquippedItem = -1;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            KeyBindingEvents();
        }

        public void KeyBindingEvents()
        {
            for (int i = itemList.Count - 1; i >= 0 ; i--)
            {
                InventoryItem n = itemList[i];

                if (n.data.defaultKeyBinding == KeyBindings.None)
                    continue;

                if (Input.GetKeyDown((KeyCode)n.data.defaultKeyBinding))
                {
                    UseItem(i);
                    break;
                }
            }
        }

        public void AddItem(InventoryItem x)
        {
            int left = x.currentAmount;
            int diff = 0;
            int n = 0;
            int safe = 1000;

            while (left > 0)
            {
                InventoryItem lastItem = null;
                foreach (InventoryItem i in itemList)
                {
                    if (i.data.name == x.data.name)
                    {
                        if (i.currentAmount < i.data.maxStackAmount)
                        {
                            lastItem = i;
                            break;
                        }
                    }
                }

                if (lastItem != null)
                {
                    // int cap = (lastItem.data.maxStackAmount-lastItem.currentAmount);
                    // diff = Mathf.Min(cap, left);
                    // n = left-diff;
                    // lastItem.currentAmount = diff;
                    // left = n;


                    int cap = (lastItem.data.maxStackAmount-lastItem.currentAmount);
                    diff = Mathf.Min(cap, left);
                    lastItem.currentAmount += diff;

                }
                else
                {
                    // diff = Mathf.Min(x.data.maxStackAmount, left);
                    // n = left-diff;
                    // x.currentAmount = diff;
                    // itemList.Add(x);
                    // left = n;

                    diff = Mathf.Min(x.data.maxStackAmount, left);
                    x.currentAmount = diff;
                    itemList.Add(x);
                }

                left -= diff;
                safe -= 1;
                if (safe <= 0)
                {
                    print("WHILE LOOP ERROR");
                    break;
                }
            }

            if (playerCharacter.isLocalPlayer)
            {
                RefreshList();
            }
        }

        public void DropFuel()
        {
            if (itemList.Count == 0)
                return;

            foreach (InventoryItem i in itemList)
            {
                if (i.data.itemType == ItemTypes.FuelCell)
                {
                    //TODO: add score
                    RemoveItem(i);
                    break;
                }
            }
        }

        public void RemoveItem(InventoryItem x, int amount = 1)
        {
            //TODO: disable weapon view on remove
            bool exists = false;
            int n = 0;
            foreach (InventoryItem i in itemList)
            {
                if (i.data.itemName == x.data.itemName)
                {
                    exists = true;
                    break;
                }
                n++;
            }

            if (exists)
            {
                InventoryItem j = itemList[n];
                j.currentAmount -= amount;
                itemList[n] = j;
                if (j.currentAmount <= 0)
                    itemList.RemoveAt(n);
            }

            if (playerCharacter.isLocalPlayer)
            {
                RefreshList();
            }
        }

        // public void TransferItem(InventoryItem x, GameObject otherPlayer, int amount = 1)
        // {
        //     PlayerCharacter p = otherPlayer.GetComponent<PlayerCharacter>();

        //     //TODO: weight
        //     InventoryItem n = x;
        //     n.currentAmount = amount;
        //     n.owner = p;
        //     RemoveItem(x, amount);
        //     p.inventoryModule.AddItem(n);
        // }

        // public void OpenInventory(PlayerCharacter p)
        // {
        //     if (itemList.Count > 0)
        //     {
        //         TransferItem(itemList[0], p.gameObject);
        //     }
        // }

        public void RefreshList()
        {
            CalculateWeight();
            CmdSyncInventory(SyncInventoryItem.ToSyncList(itemList.ToArray()).ToArray(), playerCharacter.netId);

            VMInventory.instance.items.Clear();
            foreach (InventoryItem i in itemList)
            {
                int amt = i.currentAmount;
                if (i.data.itemType == ItemTypes.Weapon)
                    amt = i.currentAmmo;

                VMInventory.instance.items.Add(new VMInventoryItem(i.data.itemName, amt));
            }
            CalculateFuelCount();
        }

        public void UseItem(int index)
        {
            InventoryItem i = itemList[index];

            if (i.data.itemType == ItemTypes.Aid)
            {
                playerCharacter.itemEffectsModule.AddEffect((AidData)i.data);
                RemoveItem(i, 1);
            }else{
                if (i.data.canBeEquipped)
                    EquipItem(index);
            }
        }

        public void EquipItem(int index)
        {
            if (currentEquippedItem == index)
                return;

            currentEquippedItem = index;

            if (index == -1)
            {
                playerCharacter.inventoryFPSModelModule.ShowItemModel(null, null);
                return;
            }

            if (Weapon.interrupt)
                return;

            InventoryItem i = itemList[index];

            if (playerCharacter.isLocalPlayer)
            {
                playerCharacter.inventoryFPSModelModule.ShowItemModel(i.data.itemName, i);
            }
            else
            {
                //TODO: show tps model
            }
        }

        public void CalculateFuelCount()
        {
            fuelCount = 0;
            foreach (InventoryItem i in itemList)
            {
                if (i.data.itemType == ItemTypes.FuelCell)
                    fuelCount += i.currentAmount;
            }
        }

        public void CalculateWeight()
        {
            int n = 0;

            foreach (InventoryItem i in itemList)
            {
                n += i.data.weight * i.currentAmount;
            }

            VMWeight.instance.Text = n + "/" + capacity;
            currentWeight = n;
        }



        [Command]
        public void CmdSyncInventory(SyncInventoryItem[] i, uint id)
        {
            RpcSyncInventory(i, id);
        }

        [ClientRpc]
        public void RpcSyncInventory(SyncInventoryItem[] i, uint id)
        {
            if (!playerCharacter.isLocalPlayer)
            {
                itemList = SyncInventoryItem.FromSyncList(i, allItemData.ToArray());
                CalculateFuelCount();
            }
        }
    }
}