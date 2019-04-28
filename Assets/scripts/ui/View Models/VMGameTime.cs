using UnityEngine;
using System.ComponentModel;
using UnityWeld.Binding;

namespace Primeval.ViewModels
{
    [Binding]
    public class VMGameTime : MonoBehaviour, INotifyPropertyChanged
    {

        private string text = "00:00.0";

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
            int ct = Mathf.RoundToInt(GameManager.gameTime);
            float diff = GameManager.gameTime-ct;
            string ds = diff.ToString("F1");
            int mm, ss;
            mm = ct / 60;
            ss = ct % 60;
            Text = mm.ToString().PadLeft(2, '0') + ":" + ss.ToString().PadLeft(2, '0') + ds.Substring(ds.IndexOf("."));
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