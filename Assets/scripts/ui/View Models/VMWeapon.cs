using UnityEngine;
using System.ComponentModel;
using UnityWeld.Binding;

namespace Primeval.ViewModels
{
    [Binding]
    public class VMWeapon : GenericSingletonClass<VMWeapon>, INotifyPropertyChanged
    {
        public Transform panel;

        private string weaponName = "";

        [Binding]
        public string WeaponName
        {
            get
            {
                return weaponName;
            }
            set
            {
                if (weaponName == value)
                {
                    return; // No change.
                }

                weaponName = value;

                panel.gameObject.SetActive(weaponName.Length > 0);

                OnPropertyChanged("WeaponName");
            }
        }
        

        private string weaponAmmunition = "00/000";

        [Binding]
        public string WeaponAmmunition
        {
            get
            {
                return weaponAmmunition;
            }
            set
            {
                if (weaponAmmunition == value)
                {
                    return; // No change.
                }

                weaponAmmunition = value;

                OnPropertyChanged("WeaponAmmunition");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}