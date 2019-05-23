using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.ViewModels;
using Primeval.Item;

namespace Primeval.PlayerCharacter
{
    public class InventoryFPSModel : PlayerModuleBase
    {
        public ItemFPSModel activeModel;

        public Dictionary<string, ItemFPSModel> viewModels;
        Transform viewModelContainer;
        List<Transform> modelList;

        public override void Initialize()
        {
            base.Initialize();

            ShowItemModel(null, null);
            VMWeapon.instance.WeaponName = "";
            VMWeapon.instance.WeaponAmmunition = "";
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public void ShowItemModel(string n, InventoryItem inv)
        {
            //TODO: fix hidden weapon on respawn
            if (inv == null)
                activeModel = null;

            if (modelList == null)
            {
                viewModelContainer = CameraManager.instance.itemFPSContainer;
                viewModels = new Dictionary<string, ItemFPSModel>();

                modelList = new List<Transform>();
                foreach (Transform i in viewModelContainer)
                {
                    ItemFPSModel v = i.GetComponent<ItemFPSModel>();
                    viewModels.Add(v.modelName, v);
                    modelList.Add(i);
                }
            }

            foreach (Transform i in modelList)
            {
                i.gameObject.SetActive(false);
            }

            foreach (Transform i in modelList)
            {
                ItemFPSModel v = i.GetComponent<ItemFPSModel>();
                bool show = v.modelName == n;
                if (show)
                {
                    activeModel = v;
                    v.referenceInventoryItem = inv;
                    i.gameObject.SetActive(show);
                    break;
                }
            }
        }
    }
}