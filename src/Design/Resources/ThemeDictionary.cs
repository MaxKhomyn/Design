using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows;
using System;

namespace Design.Resources
{
    public class ThemeDictionary : ResourceDictionary, INotifyPropertyChanged
    {
        #region Properties

        private string _ThemeName;
        public string ThemeName
        {
            get => _ThemeName;
            set { _ThemeName = value; OnPropertyChanged(); }
        }
        
        public new Uri Source
        {
            get => base.Source;
            set { base.Source = value; this.OnPropertyChanged(); }
        }

        #endregion Properties

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Events
    }
}