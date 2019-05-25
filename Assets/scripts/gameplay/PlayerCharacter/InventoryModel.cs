using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.ViewModels;
using Primeval.Item;
using Primeval.Data;
using Photon;

namespace Primeval.PlayerCharacter
{
    [System.Serializable]
    public class InventoryItem
    {
        [HideInInspector]
        public string prefab;
        public ItemBaseData data;
        public int currentAmount;
        public PlayerCharacter owner;


        //weapons only
        public int currentAmmo;
        public bool hasAmmo
        {
            get
            {
                return currentAmmo > 0;
            }
        }

        public void SetAmmoCount(int amount)
        {
            currentAmmo = amount;
            owner.inventoryModule.RefreshList();
        }

        public int GetReservedAmmo()
        {
            int a = 0;
            AmmoData ammoType = ((WeaponData)data).ammunitionType;

            foreach (InventoryItem i in owner.inventoryModule.itemList)
            {
                if (i.data.itemType != ItemTypes.Ammo)
                    continue;

                AmmoData ad = (AmmoData)i.data;
                if (ad.itemName != ammoType.itemName)
                    continue;

                a += i.currentAmount;
            }

            return a;
        }

        public void ReloadAmmo()
        {
            int ammo2 = GetReservedAmmo();

            int n = Mathf.Min(ammo2, ((WeaponData)data).ammo1Capacity - currentAmmo);

            currentAmmo += n;

            foreach (InventoryItem i in owner.inventoryModule.itemList)
            {
                if (i.data.itemType != ItemTypes.Ammo)
                    continue;

                AmmoData a1, a2;
                a1 = ((WeaponData)data).ammunitionType;
                a2 = (AmmoData)i.data;

                if (a1 != a2)
                    continue;

                owner.inventoryModule.RemoveItem(i, n);
                break;
            }

            owner.inventoryModule.RefreshList();
        }
    }

    [System.Serializable]
    public class SyncInventoryItem
    {
        public string itemName {get; set;}
        public int amount {get; set;}

        public int currentAmmo {get; set;}

        public int owner {get; set;}

        public static object Deserialize(byte[] data)
        {
            // var result = new SyncInventoryItem();
            // result.itemName = data[0].ToString();
            // result.amount = (int)data[1];
            // result.currentAmmo = (int)data[2];
            // result.owner = (int)data[3];
            // return result;
            return data.Deserialize();
        }

        public static byte[] Serialize(object customType)
        {
            // var c = (SyncInventoryItem)customType;
            // // return new byte[] { Utils.BytesFromString(c.itemName), c.amount, c.currentAmmo, c.owner };
            return customType.SerializeToByteArray();
        }

        public static SyncInventoryItem ToSync(InventoryItem i)
        {
            SyncInventoryItem s = new SyncInventoryItem();
            s.itemName = i.data.itemName;
            s.amount = i.currentAmount;
            s.currentAmmo = i.currentAmmo;
            s.owner = i.owner.photonView.viewID;
            return s;
        }

        public static List<SyncInventoryItem> ToSyncList(InventoryItem[] l)
        {
            List<SyncInventoryItem> s = new List<SyncInventoryItem>();

            foreach (InventoryItem i in l)
            {
                s.Add(ToSync(i));
            }

            return s;
        }

        public static InventoryItem FromSync(SyncInventoryItem s, ItemBaseData[] dataSet)
        {
            InventoryItem i = new InventoryItem();
            foreach (ItemBaseData j in dataSet)
            {
                if (j.itemName == s.itemName)
                {
                    i.data = j;
                    break;
                }
            }

            i.currentAmount = s.amount;
            i.currentAmmo = s.currentAmmo;
            i.owner = PlayerCharacter.FindByID(s.owner);
            return i;
        }

        public static List<InventoryItem> FromSyncList(SyncInventoryItem[] s, ItemBaseData[] dataSet)
        {
            // Mono print("from sync inventory " + playerCharacter.netId);
            List<InventoryItem> l = new List<InventoryItem>();
            foreach (SyncInventoryItem i in s)
            {
                l.Add(FromSync(i, dataSet));
            }

            return l;
        }
    }
}