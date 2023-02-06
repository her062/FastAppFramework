using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reactive.Disposables;
using System.Collections;
using Microsoft.Extensions.Logging;

namespace FastAppFramework.Wpf
{
    public abstract class BindableBase : IDisposable, ICollection<IDisposable>, INotifyPropertyChanged
    {
#region Events
        public event PropertyChangedEventHandler? PropertyChanged;
#endregion

#region Properties
        public bool IsReadOnly => this._disposables.IsReadOnly;
        public int Count => this._disposables.Count;
#endregion

#region Fields
        private bool disposedValue;
        private CompositeDisposable _disposables;

#endregion

#region Constructor/Destructor
        protected BindableBase()
        {
            // Setup Fields.
            {
                this.PropertyChanged = null;
                this._disposables = new CompositeDisposable();
            }

            FastWpfApplication.Current.Logger.LogDebug($"{this.GetType().FullName}");
        }
        ~BindableBase()
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
        public IEnumerator<IDisposable> GetEnumerator()
        {
            return this._disposables.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this._disposables).GetEnumerator();
        }
        public bool Contains(IDisposable item)
        {
            return this._disposables.Contains(item);
        }
        public void Add(IDisposable item)
        {
            this._disposables.Add(item);
        }
        public bool Remove(IDisposable item)
        {
            return this._disposables.Remove(item);
        }
        public void Clear()
        {
            this._disposables.Clear();
        }
        public void CopyTo(IDisposable[] array, int arrayIndex)
        {
            this._disposables.CopyTo(array, arrayIndex);
        }
#endregion

#region Protected Functions
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this._disposables.Dispose();
                }

                disposedValue = true;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
#endregion
    }
}