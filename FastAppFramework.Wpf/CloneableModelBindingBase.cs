using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FastAppFramework.Core;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace FastAppFramework.Wpf
{
    public static class CloneableModelBindingBaseExtensions
    {
        public static TProperty Observe<TProperty, TDataType>(this TProperty self, CloneableModelBindingBase observer) where TProperty : IObservable<TDataType>
        {
            self.Subscribe(v => observer.Validate()).AddTo(observer);
            return self;
        }
        public static ReactiveProperty<TDataType> Observe<TDataType>(this ReactiveProperty<TDataType> self, CloneableModelBindingBase observer)
        {
            observer.ErrorObservers.Add(self);
            return self.Observe<ReactiveProperty<TDataType>, TDataType>(observer);
        }
    }

    public class CloneableModelBindingBase : ModelBindingBase
    {
#region Constants
        public enum UpdateModelTrigger
        {
            Default,
            Immediately,
            Commit,
        }
#endregion

#region Commands
        public ReactiveCommand CommitCommand
        {
            get; private set;
        }
        public ReactiveCommand RollbackCommand
        {
            get; private set;
        }
#endregion

#region Properties
        public new CloneableModelBase Model => (CloneableModelBase)base.Model;

        public CloneableModelBase Snapshot => this._snapshot;

        public ReadOnlyReactivePropertySlim<bool> IsDirty
        {
            get; private set;
        }
        public ReadOnlyReactivePropertySlim<bool> HasErrors
        {
            get; private set;
        }
        public ReactiveCollection<IReactiveProperty> ErrorObservers
        {
            get; private set;
        }
#endregion

#region Fields
        private UpdateModelTrigger _trigger;
        private CloneableModelBase _snapshot;
        private ReactivePropertySlim<bool> _isDirty;
#endregion

#region Constructor/Destructor
        public CloneableModelBindingBase(CloneableModelBase model, UpdateModelTrigger trigger = UpdateModelTrigger.Default) : base(TakeModel(model, trigger))
        {
            // Setup Fields.
            {
                this._trigger = trigger;
                this._snapshot = TakeSnapshot(model, this._trigger);
                this._isDirty = new ReactivePropertySlim<bool>(false).AddTo(this);
            }

            // Setup Properties.
            {
                this.IsDirty = this._isDirty.ToReadOnlyReactivePropertySlim().AddTo(this);
                this.ErrorObservers = new ReactiveCollection<IReactiveProperty>().AddTo(this);
                this.HasErrors = this.ErrorObservers.ObserveElementObservableProperty(o => o.ObserveHasErrors).CombineLatest(this.ErrorObservers.CollectionChangedAsObservable()).Select(_ => this.ErrorObservers.Any(v => v.ObserveHasErrors.MostRecent(false).First())).ToReadOnlyReactivePropertySlim().AddTo(this);
            }

            // Setup Commands.
            {
                this.CommitCommand = Observable.CombineLatest(
                        this.IsDirty, this.HasErrors,
                        (d, e) => (d && !e)
                    ).ToReactiveCommand()
                    .WithSubscribe(() => {
                        this.Model.CopyTo(this.Snapshot);
                        Validate();
                    }).AddTo(this);
                this.RollbackCommand = this.IsDirty.ToReactiveCommand()
                    .WithSubscribe(() => {
                        this.Snapshot.CopyTo(this.Model);
                        Validate();
                    }).AddTo(this);
            }
        }
#endregion

#region Public Functions
        public void Validate()
        {
            this._isDirty.Value = (this.Model.CompareTo(this.Snapshot) != 0);
        }
#endregion

#region Private Functions
        private static CloneableModelBase TakeModel(CloneableModelBase source, UpdateModelTrigger trigger)
        {
            switch (trigger)
            {
                case UpdateModelTrigger.Default:
                case UpdateModelTrigger.Immediately:
                    return source;
                case UpdateModelTrigger.Commit:
                    return (CloneableModelBase)source.Clone();
                default:
                    break;
            }
            throw new NotImplementedException();
        }
        private static CloneableModelBase TakeSnapshot(CloneableModelBase source, UpdateModelTrigger trigger)
        {
            switch (trigger)
            {
                case UpdateModelTrigger.Default:
                case UpdateModelTrigger.Immediately:
                    return (CloneableModelBase)source.Clone();
                case UpdateModelTrigger.Commit:
                    return source;
                default:
                    break;
            }
            throw new NotImplementedException();
        }
#endregion
    }

    public abstract class CloneableModelBindingBase<T> : CloneableModelBindingBase where T : CloneableModelBase
    {
#region Properties
        public new T Model => (T)base.Model;
        public new T Snapshot => (T)base.Snapshot;
#endregion

#region Constructor/Destructor
        protected CloneableModelBindingBase(T model, UpdateModelTrigger trigger = UpdateModelTrigger.Default) : base(model, trigger)
        {
        }
#endregion
    }
}