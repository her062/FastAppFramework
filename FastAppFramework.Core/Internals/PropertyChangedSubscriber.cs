using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastAppFramework.Core.Internals
{
    internal class PropertyChangedSubscriber<TObject, TProperty> : IDisposable where TObject : INotifyPropertyChanged
    {
#region Properties
        public TObject? Source
        {
            get => this._source;
            set => UpdateSource(value);
        }
#endregion

        #region Fields
        private TObject? _source;
        private string _propertyName;
        private Func<TObject, TProperty?>? _accessor;
        private IObserver<TProperty?>? _observer;

        private bool _disposed;
#endregion

#region Constructor/Destructor
        public PropertyChangedSubscriber(TObject source, string propertyName, Func<TObject, TProperty?> accessor, IObserver<TProperty?> observer)
        {
            // Setup Fields.
            {
                this._source = source;
                this._propertyName = propertyName;
                this._accessor = accessor;
                this._observer = observer;
            }

            // Subscribes.
            {
                this._source.PropertyChanged += Source_PropertyChanged;
            }
        }
        ~PropertyChangedSubscriber()
        {
            Dispose(false);
        }
#endregion

#region Public Functions
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual TProperty? OnNext()
        {
            if ((this._source == null) || (this._accessor == null))
                throw new ObjectDisposedException(this.GetType().Name);

            var value = this._accessor(this._source);
            this._observer?.OnNext(value);
            return value;
        }
#endregion

#region Protected Functions
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                this._observer?.OnCompleted();

                if (disposing)
                {
                    this._source!.PropertyChanged -= Source_PropertyChanged;

                    this._source = default(TObject);
                    this._accessor = null;
                    this._observer = null;
                }
                _disposed = true;
            }
        }
#endregion

#region Private Functions
        private void UpdateSource(TObject? value)
        {
            if (this._source != null)
                this._source.PropertyChanged -= Source_PropertyChanged;

            this._source = value;
            if (this._source != null)
            {
                this._source.PropertyChanged += Source_PropertyChanged;
                OnNext();
            }
        }
        private void Source_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this._propertyName)
                OnNext();
        }
#endregion
    }
}