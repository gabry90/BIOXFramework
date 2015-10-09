using System;
using System.Linq;
using System.Collections.Generic;
using BIOXFramework.Utility.Extensions;

namespace BIOXFramework.Utility
{
    #region event class

    public class ExtendedListCollectionChangedArgs<T> : EventArgs
    {
        public ExtendedListCollectionChangedArgs(T item, ExtendedListChangeStatus status)
        {
            ItemChanged = item;
            Status = status;
        }

        public T ItemChanged { get; private set; }
        public ExtendedListChangeStatus Status { get; private set; }
    }

    #endregion

    #region enums

    public enum ExtendedListChangeStatus
    {
        Added,
        Removed
    }

    #endregion

    public class ExtendedList<T> : List<T>, ICloneable, IDisposable
    {
        #region vars

        public event EventHandler<ExtendedListCollectionChangedArgs<T>> CollectionChanged;

        public bool EnableRaisingEvents { get; set; }

        #endregion

        #region constructors

        public ExtendedList()
            : base()
        {
            EnableRaisingEvents = true;
        }

        public ExtendedList(IEnumerable<T> list)
            : base(list)
        {
            EnableRaisingEvents = true;
        }

        #endregion

        #region new implementations

        public new void Add(T item)
        {
            base.Add(item);
            if (EnableRaisingEvents) CollectionChangedDispatcher(new ExtendedListCollectionChangedArgs<T>(item, ExtendedListChangeStatus.Added));
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            try
            {
                base.AddRange(collection);
                if (EnableRaisingEvents) collection.ToList().ForEach(x => new ExtendedListCollectionChangedArgs<T>(x, ExtendedListChangeStatus.Added));
            }
            catch (Exception ex) { throw ex; }
        }

        public new bool Remove(T item)
        {
            if (base.Remove(item))
            {
                if (EnableRaisingEvents) CollectionChangedDispatcher(new ExtendedListCollectionChangedArgs<T>(item, ExtendedListChangeStatus.Removed));
                return true;
            }
            return false;
        }

        public new void RemoveAt(int index)
        {
            int total = this.Count();
            if (total == 0 || index < 0 || index >= total)
                return;

            try
            {
                if (EnableRaisingEvents)
                {
                    T item = this[index];
                    base.RemoveAt(index);
                    CollectionChangedDispatcher(new ExtendedListCollectionChangedArgs<T>(item, ExtendedListChangeStatus.Removed));
                }
                else
                    base.RemoveAt(index);
            }
            catch (Exception ex) { throw ex; }
        }

        public new void RemoveRange(int index, int count)
        {
            try
            {
                if (EnableRaisingEvents)
                {
                    List<T> items = this.GetRange(index, count);
                    base.RemoveRange(index, count);
                    items.ForEach(x => CollectionChangedDispatcher(new ExtendedListCollectionChangedArgs<T>(x, ExtendedListChangeStatus.Removed)));
                }
                else
                    base.RemoveRange(index, count);
            }
            catch (Exception ex) { throw ex; }
        }

        public new void Clear()
        {
            if (EnableRaisingEvents)
            {
                List<T> items = this.ToList();
                base.Clear();
                items.ForEach(x => CollectionChangedDispatcher(new ExtendedListCollectionChangedArgs<T>(x, ExtendedListChangeStatus.Removed)));
            }
            else
                base.Clear();
        }

        #endregion

        #region custom methods

        public void AddExclusive(T item)
        {
            if (!this.Contains(item))
                Add(item);
        }

        public List<Y> GetByType<Y>()
        {
            List<Y> list = new List<Y>();
            return (List<Y>)this.Where(x => x.GetType() == typeof(Y));
        }

        public bool ContainsAll(params T[] items)
        {
            return this.Count(x => items.Contains(x)) == items.Count();
        }

        public bool ContainsOneOrMore(params T[] items)
        {
            return this.Count(x => items.Contains(x)) > 0;
        }

        #endregion

        #region dispatchers

        private void CollectionChangedDispatcher(ExtendedListCollectionChangedArgs<T> e)
        {
            var h = CollectionChanged;
            if (h != null)
                h(this, e);
        }

        #endregion

        #region interface implementations

        public object Clone()
        {
            return new ExtendedList<T>(this.Select(x => object.Equals(typeof(T).GetDefaultValue(), (object)default(T)) ? default(T) : x.CloneEx()).ToList());
        }

        public void Dispose()
        {
            this.ForEach(x => { if (!object.Equals(typeof(T).GetDefaultValue(), (object)default(T))) { x.DisposeEx(); } });
            this.Clear();
        }

        #endregion
    }
}