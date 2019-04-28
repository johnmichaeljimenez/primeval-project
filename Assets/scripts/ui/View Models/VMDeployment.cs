using UnityEngine;
using System.ComponentModel;
using UnityWeld.Binding;

namespace Primeval.ViewModels
{
    [Binding]
    public class VMDeployment : GenericSingletonClass<VMDeployment>, INotifyPropertyChanged
    {
        //altitude
        //target height
        //starting height
        //status


        private float altitude = 0;

        [Binding]
        public float Altitude
        {
            get
            {
                return altitude;
            }
            set
            {
                if (altitude == value)
                {
                    return; // No change.
                }

                altitude = value;

                OnPropertyChanged("Altitude");
            }
        }

        private string targetHeight = "0.0m";

        [Binding]
        public string TargetHeight
        {
            get
            {
                return targetHeight;
            }
            set
            {
                if (targetHeight == value)
                {
                    return; // No change.
                }

                targetHeight = value;

                OnPropertyChanged("TargetHeight");
            }
        }

        private string startingHeight = "0.0m";

        [Binding]
        public string StartingHeight
        {
            get
            {
                return startingHeight;
            }
            set
            {
                if (startingHeight == value)
                {
                    return; // No change.
                }

                startingHeight = value;

                OnPropertyChanged("StartingHeight");
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