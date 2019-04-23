using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.Item;
using Primeval.Data;

namespace Primeval.PlayerCharacter
{
    public class ItemEffects : PlayerModuleBase
    {
        public List<Aid> activeEffects;

        public override void Initialize()
        {
            base.Initialize();

            activeEffects = new List<Aid>();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            for (int i = activeEffects.Count - 1; i >= 0 ; i--)
            {
                activeEffects[i].OnUpdate();
                if (activeEffects[i].time <= 0)
                {
                    activeEffects[i].OnExpire();
                    activeEffects.RemoveAt(i);
                }
            }
        }

        public void AddEffect(AidData data)
        {
            Aid a = new Aid();
            a.aidData = data;
            a.OnUse();

            activeEffects.Add(a);
        }
    }
}