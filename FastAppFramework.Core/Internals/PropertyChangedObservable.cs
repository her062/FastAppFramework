using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastAppFramework.Core.Internals
{
    internal class PropertyChangedObservable<TObject, TProperty> : IObservable<TProperty?> where TObject : INotifyPropertyChanged
    {
#region Fields
        private TObject _source;
        private string _propertyName;
        private Func<TObject, TProperty?> _accessor;
        private bool _subscribeOnInitialized;
#endregion

#region Constructor/Destructor
        public PropertyChangedObservable(TObject source, string propertyName, Func<TObject, TProperty?> accessor, bool subscribeOnInitialized = true)
        {
            // Setup Fields.
            {
                this._source = source;
                this._propertyName = propertyName;
                this._accessor = accessor;
                this._subscribeOnInitialized = subscribeOnInitialized;
            }
        }
#endregion

#region Public Functions
        public IDisposable Subscribe(IObserver<TProperty?> observer)
        {
            var subscriber = new PropertyChangedSubscriber<TObject, TProperty>(this._source, this._propertyName, this._accessor, observer);
            if (this._subscribeOnInitialized)
                subscriber.OnNext();
            return subscriber;
        }
#endregion
    }
}