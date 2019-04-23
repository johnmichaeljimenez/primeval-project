using UnityEngine;
using System.ComponentModel;
using UnityWeld.Binding;

namespace Primeval.ViewModels
{
    [Binding]
    public class VMGameTime : MonoBehaviour, INotifyPropertyChanged
    {

        private string text = "<Type some text>";

        [Binding]
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                if (text == value)
                {
                    return; // No change.
                }

                text = value;

                OnPropertyChanged("Text");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void Start()
        {
            Update();
        }

        void Update()
        {
            Text = GameManager.gameTime.ToString("F2");
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}