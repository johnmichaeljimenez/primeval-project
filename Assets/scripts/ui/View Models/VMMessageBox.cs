using UnityEngine;
using System.ComponentModel;
using UnityWeld.Binding;

namespace Primeval.ViewModels
{
    [Binding]
    public class VMMessageBox : GenericSingletonClass<VMMessageBox>, INotifyPropertyChanged
    {

        private string caption = "";

        [Binding]
        public string Caption
        {
            get
            {
                return caption;
            }
            set
            {
                if (caption == value)
                {
                    return; // No change.
                }

                caption = value;

                OnPropertyChanged("Caption");
            }
        }

        private string content = "";

        [Binding]
        public string Content
        {
            get
            {
                return content;
            }
            set
            {
                if (content == value)
                {
                    return; // No change.
                }

                content = value;

                OnPropertyChanged("Content");
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