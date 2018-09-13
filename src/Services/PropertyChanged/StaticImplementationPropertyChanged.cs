using System.Runtime.CompilerServices;
using System.ComponentModel;
using System;

namespace Services.PropertyChanged
{
    public class StaticImplementationPropertyChanged : ImplementationPropertyChanged
    {
        #region Events

        public static event EventHandler<PropertyChangedEventArgs> Static_PropertyChanged;

        #endregion Events

        #region Methods

        public static void Static_OnPropertyChanged([CallerMemberName]string PropertyName = "") =>
            Static_PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(PropertyName));

        #endregion Methods
    }
}