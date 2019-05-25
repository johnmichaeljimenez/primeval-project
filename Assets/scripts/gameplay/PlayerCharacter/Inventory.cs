using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.ViewModels;
using Primeval.Item;
using Primeval.Data;
using Photon;

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

                if (playerCharacter.photonView.isMine)
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
        public int currentEquippedItem;
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
            for (int i = itemList.Count - 1; i >= 0; i--)
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


                    int cap = (lastItem.data.maxStackAmount - lastItem.currentAmount);
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

            if (playerCharacter.photonView.isMine)
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

        public void DropItem(InventoryItem x, bool all = true)
        {
            //all: true-> drop all, or else, drop only half
            int count = all ? x.currentAmount : (x.currentAmount == 1 ? 1 : x.currentAmount / 2);
            
            object[] data = new object[1] { SyncInventoryItem.ToSync(x) };
            Vector3 pos = playerCharacter.transform.position;

            CmdDropItem(x.prefab, SyncInventoryItem.ToSync(x), pos);
            RemoveItem(x, count);
        }

        public void RemoveItem(InventoryItem x, int amount = 1)
        {
            ItemFPSModel m = PlayerCharacter.myPlayer.inventoryFPSModelModule.activeModel;
            if (m)
            {
                if (m.modelData == x.data)
                {
                playerCharacter.inventoryFPSModelModule.ShowItemModel(null, null);
                }
            }

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

            if (playerCharacter.photonView.isMine)
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
            CmdSyncInventory(SyncInventoryItem.ToSyncList(itemList.ToArray()).ToArray(), playerCharacter.photonView.viewID);

            VMInventory.instance.items.Clear();
            foreach (InventoryItem i in itemList)
            {
                int amt = i.currentAmount;
                int w = amt * i.data.weight;
                if (i.data.itemType == ItemTypes.Weapon)
                {
                    amt = i.currentAmmo;
                    w = i.data.weight;
                }

                VMInventory.instance.items.Add(new VMInventoryItem(i.data.itemName, amt, w, i.data.defaultKeyBinding));
            }
            CalculateFuelCount();
        }

        public void UseItem(int index)
        {
            InventoryItem i = itemList[index];
            playerCharacter.audioPlayerModule.PlaySound(i.data.useClip, true);

            if (i.data.itemType == ItemTypes.Aid)
            {
                playerCharacter.itemEffectsModule.AddEffect((AidData)i.data);
                RemoveItem(i, 1);
            }
            else
            {
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

            if (playerCharacter.photonView.isMine)
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



        //[Command]
        public void CmdSyncInventory(SyncInventoryItem[] i, int id)
        {
            photonView.RPC("RpcSyncInventory", PhotonTargets.All, i, id);
        }

        [PunRPC]
        public void RpcSyncInventory(SyncInventoryItem[] i, int id)
        {
            if (!playerCharacter.photonView.isMine)
            {
                itemList = SyncInventoryItem.FromSyncList(i, allItemData.ToArray());
                CalculateFuelCount();
            }
        }

        

        //[Command]
        public void CmdDropItem(string n, SyncInventoryItem d, Vector3 p)
        {
            photonView.RPC("RpcDropItem", PhotonTargets.All, n, d, p);
        }

        [PunRPC]
        public void RpcDropItem(string n, SyncInventoryItem d, Vector3 p)
        {
            if (PhotonNetwork.isMasterClient)
            {
                print("master client drop item: " + PhotonNetwork.isMasterClient);
                PhotonNetwork.InstantiateSceneObject(n, p, Quaternion.identity, 0, new object[1]{d});
            }
        }
    }
}