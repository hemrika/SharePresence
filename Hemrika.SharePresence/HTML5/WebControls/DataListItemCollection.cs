using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web.UI;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// A Collection <see cref="Hemrika.SharePresence.Html5.WebControls.DataListItem" />s 
    /// </summary>
    public class DataListItemCollection : IList, ICollection, IEnumerable, IStateManager
    {
        #region Fields
        private ArrayList listItems = new ArrayList();
        private bool marked = false;
        #endregion
        #region Methods
        /// <summary>
        /// Adds a new item to the collection
        /// </summary>
        /// <param name="item">New Item to Add</param>
        public void Add(string item)
        {
            this.Add(new DataListItem(item));
        }

        /// <summary>
        /// Adds a new item to the collection
        /// </summary>
        /// <param name="item">New Item to Add</param>
        public void Add(DataListItem item)
        {
            this.listItems.Add(item);
            if (this.marked)
            {
                item.Dirty = true;
            }
        }

        /// <summary>
        /// Adds new items to the collection
        /// </summary>
        /// <param name="items">List of new Items to Add</param>
        public void AddRange(DataListItem[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            foreach (DataListItem item in items)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// Clears the collections
        /// </summary>
        public void Clear()
        {
            this.listItems.Clear();
        }

        /// <summary>
        /// Determines if the collection contains a specified item
        /// </summary>
        /// <param name="item">The item to look for</param>
        /// <returns>True if collection contains the specified item; otherwise false</returns>
        public bool Contains(DataListItem item)
        {
            return this.listItems.Contains(item);
        }
        /// <summary>
        /// Copies all the items of collection into another array starting from the specified index
        /// </summary>
        /// <param name="array">The array to copy items to</param>
        /// <param name="index">Start index of copying</param>
        public void CopyTo(Array array, int index)
        {
            this.listItems.CopyTo(array, index);
        }
        /// <summary>
        /// Searches the collection with the specified value string
        /// </summary>
        /// <param name="value">The value to look for</param>
        /// <returns>The item containing the specified value</returns>
        public DataListItem FindByValue(string value)
        {
            int num = this.FindByValueInternal(value, true);
            if (num != -1)
            {
                return (DataListItem)this.listItems[num];
            }
            return null;
        }

        internal int FindByValueInternal(string value, bool includeDisabled)
        {
            int num = 0;
            foreach (DataListItem item in this.listItems)
            {
                if (item.Value.Equals(value) && (includeDisabled || item.Enabled))
                {
                    return num;
                }
                num++;
            }
            return -1;
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An enumerator that iterates through a collection</returns>
        public IEnumerator GetEnumerator()
        {
            return this.listItems.GetEnumerator();
        }

        /// <summary>
        /// Searches for the specified System.Object and returns the zero-based index of the first occurrence within the entire System.Collections.ArrayList.
        /// </summary>
        /// <param name="item">The System.Object to locate in the System.Collections.ArrayList. The value can be null.</param>
        /// <returns>The zero-based index of the first occurrence of value within the entire System.Collections.ArrayList, if found; otherwise, -1.</returns>
        public int IndexOf(DataListItem item)
        {
            return this.listItems.IndexOf(item);
        }
        /// <summary>
        /// Inserts an element into the Collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="item">The value to insert</param>
        public void Insert(int index, string item)
        {
            this.Insert(index, new DataListItem(item));
        }
        /// <summary>
        /// Inserts an element into the Collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="item">The value to insert</param>
        public void Insert(int index, DataListItem item)
        {
            this.listItems.Insert(index, item);
        }

        internal void LoadViewState(object state)
        {
            if (state == null)
            {
                return;
            }
            Pair pair = (Pair)state;
            this.listItems = new ArrayList();
            string[] strArray = (string[])pair.First;
            bool[] third = (bool[])pair.Second;
            for (int j = 0; j < strArray.Length; j++)
            {
                this.Add(new DataListItem(strArray[j], third[j]));
            }
        }

        /// <summary>
        /// Removes the specified item from collection
        /// </summary>
        /// <param name="item">The item to Remove</param>
        public void Remove(string item)
        {
            int index = this.IndexOf(new DataListItem(item));
            if (index >= 0)
            {
                this.RemoveAt(index);
            }
        }
        /// <summary>
        /// Removes the specified item from collection
        /// </summary>
        /// <param name="item">The item to Remove</param>
        public void Remove(DataListItem item)
        {
            int index = this.IndexOf(item);
            if (index >= 0)
            {
                this.RemoveAt(index);
            }
        }
        /// <summary>
        /// Removes the item at the specified index from collection
        /// </summary>
        /// <param name="index">The index of item to be removed</param>
        public void RemoveAt(int index)
        {
            this.listItems.RemoveAt(index);
        }

        internal object SaveViewState()
        {
            int count = this.Count;
            object[] objArray = new string[count];
            bool[] z = new bool[count];
            for (int j = 0; j < count; j++)
            {
                objArray[j] = this[j].Value;
                z[j] = this[j].Enabled;
            }
            return new Pair(objArray, z);
        }

        int IList.Add(object item)
        {
            DataListItem item2 = (DataListItem)item;
            int num = this.listItems.Add(item2);
            if (this.marked)
            {
                item2.Dirty = true;
            }
            return num;
        }

        bool IList.Contains(object item)
        {
            return this.Contains((DataListItem)item);
        }

        int IList.IndexOf(object item)
        {
            return this.IndexOf((DataListItem)item);
        }

        void IList.Insert(int index, object item)
        {
            this.Insert(index, (DataListItem)item);
        }

        void IList.Remove(object item)
        {
            this.Remove((DataListItem)item);
        }

        void IStateManager.LoadViewState(object state)
        {
            this.LoadViewState(state);
        }

        object IStateManager.SaveViewState()
        {
            return this.SaveViewState();
        }

        void IStateManager.TrackViewState()
        {
            this.TrackViewState();
        }

        internal void TrackViewState()
        {
            this.marked = true;
            for (int i = 0; i < this.Count; i++)
            {
                this[i].TrackViewState();
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the capacity of the collection
        /// </summary>
        public int Capacity
        {
            get
            {
                return this.listItems.Capacity;
            }
            set
            {
                this.listItems.Capacity = value;
            }
        }

        /// <summary>
        /// Gets the current number of items added to collection
        /// </summary>
        public int Count
        {
            get
            {
                return this.listItems.Count;
            }
        }

        /// <summary>
        /// Returns if the list is readonly
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return this.listItems.IsReadOnly;
            }
        }

        /// <summary>
        /// Gets a value indicating whether access to the System.Collections.ArrayList is synchronized (thread safe).
        /// </summary>
        public bool IsSynchronized
        {
            get
            {
                return this.listItems.IsSynchronized;
            }
        }

        /// <summary>
        /// Returns the item at the specified index
        /// </summary>
        /// <param name="index">The specified index</param>
        /// <returns>The item at the specified index</returns>
        public DataListItem this[int index]
        {
            get
            {
                return (DataListItem)this.listItems[index];
            }
        }
        /// <summary>
        /// SyncRoot
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this.listItems[index];
            }
            set
            {
                this.listItems[index] = (DataListItem)value;
            }
        }

        bool IStateManager.IsTrackingViewState
        {
            get
            {
                return this.marked;
            }
        }
        #endregion
    }



}
