using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace FastAppFramework.Core
{
    public abstract class ModelBase : IDisposable, INotifyPropertyChanged
    {
#region Events
        public event PropertyChangedEventHandler? PropertyChanged;
#endregion

#region Fields
        private bool _disposed;
#endregion

#region Constructor/Destructor
        protected ModelBase()
        {
        }
        ~ModelBase()
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
#endregion

#region Protected Functions
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    this.PropertyChanged = null;
                }
                _disposed = true;
            }
        }
        protected void SetValue<T>(ref T obj, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(obj, value))
                return;

            obj = value;
            OnPropertyChanged(propertyName);
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
#endregion
    }
}