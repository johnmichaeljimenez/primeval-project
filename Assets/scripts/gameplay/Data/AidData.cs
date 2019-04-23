using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Primeval.Data
{
    public class AidData : ItemBaseData
    {
        [Header("Stats")]
        public int effectAmount;
        public float effectDuration;

        public int effectRate
        {
            get{
                float f = (float)effectAmount;
                f = f/effectDuration;

                return Mathf.RoundToInt(f);
            }
        }
    }
}