using UnityEngine;
using System.ComponentModel;
using UnityWeld.Binding;

namespace Primeval.ViewModels
{
    [Binding]
    public class VMVitality : GenericSingletonClass<VMVitality>, INotifyPropertyChanged
    {

        private string hp = "0";

        [Binding]
        public string HP
        {
            get
            {
                return hp;
            }
            set
            {
                if (hp == value)
                {
                    return; // No change.
                }

                hp = value;

                OnPropertyChanged("HP");
            }
        }

        private string ap = "0";

        [Binding]
        public string AP
        {
            get
            {
                return ap;
            }
            set
            {
                if (ap == value)
                {
                    return; // No change.
                }

                ap = value;

                OnPropertyChanged("AP");
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