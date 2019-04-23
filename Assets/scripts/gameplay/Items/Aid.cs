using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Primeval.Data;
using Primeval.PlayerCharacter;

namespace Primeval.Item
{
    public class Aid
    {
        public AidData aidData;

        float lastTick;
        public float time
        {
            get;
            private set;
        }

        public void OnUse()
        {
            time = aidData.effectDuration;
            lastTick = time;
        }

        public void OnUpdate()
        {
            time -= Time.deltaTime;
            if (time <= lastTick-1)
            {
                lastTick -= 1;
                OnEffect();
            }
        }

        public void OnExpire()
        {

        }

        public void OnEffect()
        {
            PlayerCharacter.PlayerCharacter p = PlayerCharacter.PlayerCharacter.myPlayer;
            if (aidData.itemName == "Medkit")
            {
                p.vitalityModule.HealHitPoints(aidData.effectRate);
            }else if (aidData.itemName == "Armor Charge")
            {
                p.vitalityModule.HealArmorPoints(aidData.effectRate);
            }
        }
    }
}