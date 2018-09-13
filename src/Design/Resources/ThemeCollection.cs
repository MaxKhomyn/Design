using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;

namespace Design.Resources
{
    public class ThemeCollection : ObservableCollection<ThemeDictionary>
    {
        #region Fields

        private IList<ThemeDictionary> _previousList;

        #endregion Fields

        #region Constructor

        public ThemeCollection()
        {
            this._previousList = new List<ThemeDictionary>();
            this.CollectionChanged += ThemeCollection_CollectionChanged;
        }

        #endregion Constructor
    
        #region Events

        private void ThemeCollection_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                foreach (var item in this._previousList)
                    item.PropertyChanged -= Item_PropertyChanged;
                this._previousList.Clear();
            }


            if (e.OldItems != null)
            {
                foreach (ThemeDictionary item in e.OldItems)
                {
                    this._previousList.Remove(item);
                    item.PropertyChanged -= Item_PropertyChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (ThemeDictionary item in e.NewItems)
                {
                    this._previousList.Add(item);
                    item.PropertyChanged += Item_PropertyChanged;
                }
            }
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCollectionChanged
            (
                new System.Collections.Specialized.NotifyCollectionChangedEventArgs(System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            );
        }

        #endregion Events
    }
}