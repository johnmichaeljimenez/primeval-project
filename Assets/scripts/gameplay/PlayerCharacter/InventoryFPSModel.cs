using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.Item;

namespace Primeval.PlayerCharacter
{
    public class InventoryFPSModel : PlayerModuleBase
    {
        public Dictionary<string, ItemFPSModel> viewModels;
        Transform viewModelContainer;
        List<Transform> modelList;

        public override void Initialize()
        {
            base.Initialize();

            viewModelContainer = CameraManager.instance.itemFPSContainer;
            viewModels = new Dictionary<string, ItemFPSModel>();

            modelList = new List<Transform>();
            foreach (Transform i in viewModelContainer)
            {
                ItemFPSModel v = i.GetComponent<ItemFPSModel>();
                viewModels.Add(v.modelName, v);
                modelList.Add(i);
            }

            ShowItemModel(null, null);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public void ShowItemModel(string n, InventoryItem inv)
        {
            foreach (Transform i in modelList)
            {
                i.gameObject.SetActive(false);
            }

            foreach (Transform i in modelList)
            {
                ItemFPSModel v = i.GetComponent<ItemFPSModel>();
                bool show = v.modelName == n;
                i.gameObject.SetActive(show);
                if (show)
                {
                    v.referenceInventoryItem = inv;
                    break;
                }
            }
        }
    }
}