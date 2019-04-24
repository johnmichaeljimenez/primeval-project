using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Primeval.Data
{
    public class WeaponData : ItemBaseData
    {
        [Header("Stats")]
        public int damage;
        [Range(0, 10)]
        public float spread = 3;
        public WeaponTypes weaponType;
        public FiringTypes firingType;
        public bool canAim;

        [Header("Ammunition")]
        public int ammo1Capacity;
        public int ammo2Capacity;
        public AmmoData ammunitionType;
    }


    public enum FiringTypes
    {
        OneShot, Automatic
    }

    public enum WeaponTypes
    {
        Melee, Firearm
    }
}