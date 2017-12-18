using System.ComponentModel;

namespace Infragistics.Framework  
{
    /// <summary>
    /// Represents a base class enabling INotifyPropertyChanged implementation 
    /// and methods for setting property values.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        /// <summary>
        /// Occurs when a property value was changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool HasPropertyChangedHandler()
        {
            var handler = this.PropertyChanged;
            return handler != null;
        }
        
        /// <summary>
        /// Raises the PropertyChanged event for specified property name
        /// </summary>
        /// <param name="propertyName">The name of the property to raise the PropertyChanged event for.</param>
        public virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Raises the PropertyChanged event for specified property name
        /// </summary>
        protected virtual void OnPropertyChanged(object sender, string propertyName)
        {
            OnPropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
        }
        /// <summary>
        /// Raises the PropertyChanged event  
        /// </summary>
        protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null) 
                eventHandler(sender, e);
            //Log.State(sender, e);
        }
        /// <summary>
        /// Raises the PropertyChanged event  
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            OnPropertyChanged(this, e);
        }
        
        #endregion

        //WPF
        //public void OnAsyncPropertyChanged(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
        //           new ThreadStart(() =>
        //           {
        //              PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //         }));
        //    }
        //}
//#if SILVERLIGHT
//        protected delegate void PropertyChangedDelegate(object sender, string propertyName);

//        public void OnAsyncPropertyChanged(string propertyName)
//        {
//            var handler = this.PropertyChanged;
//            if (handler == null) return;

//            Deployment.Current.Dispatcher.BeginInvoke(new PropertyChangedDelegate(OnPropertyChanged), this, propertyName);
//            //Deployment.Current.Dispatcher.BeginInvoke(() =>
//            //{
//            //    OnPropertyChanged(this, propertyName);
//            //});
//        }
//#endif
//       



    }
}