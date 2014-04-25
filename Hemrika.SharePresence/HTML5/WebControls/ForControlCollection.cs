using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Hemrika.SharePresence.Html5.WebControls
{
    /// <summary>
    /// A list of one or more controls
    /// </summary>
    public class ForControlCollection : IList, ICollection, IEnumerable
    {
        #region Fields
        private ArrayList listItems = new ArrayList();
        #endregion
        #region Methods
        /// <summary>
        /// Adds a new item to the collection
        /// </summary>
        /// <param name="item">New Item to Add</param>
        public void Add(string item)
        {
            this.Add(new ForControl(item));
        }
        /// <summary>
        /// Adds a new item to the collection
        /// </summary>
        /// <param name="item">New Item to Add</param>
        public void Add(ForControl item)
        {
            this.listItems.Add(item);
        }
        /// <summary>
        /// Adds new items to the collection
        /// </summary>
        /// <param name="items">List of new Items to Add</param>
        public void AddRange(ForControl[] items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }
            foreach (ForControl item in items)
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
        public bool Contains(ForControl item)
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
        public int IndexOf(ForControl item)
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
            this.Insert(index, new ForControl(item));
        }
        /// <summary>
        /// Inserts an element into the Collection at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="item">The value to insert</param>
        public void Insert(int index, ForControl item)
        {
            this.listItems.Insert(index, item);
        }
        /// <summary>
        /// Removes the specified item from collection
        /// </summary>
        /// <param name="item">The item to Remove</param>
        public void Remove(string item)
        {
            int index = this.IndexOf(new ForControl(item));
            if (index >= 0)
            {
                this.RemoveAt(index);
            }
        }

        /// <summary>
        /// Removes the specified item from collection
        /// </summary>
        /// <param name="item">The item to Remove</param>
        public void Remove(ForControl item)
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

        int IList.Add(object item)
        {
            ForControl item2 = (ForControl)item;
            int num = this.listItems.Add(item2);
            return num;
        }

        bool IList.Contains(object item)
        {
            return this.Contains((ForControl)item);
        }

        int IList.IndexOf(object item)
        {
            return this.IndexOf((ForControl)item);
        }

        void IList.Insert(int index, object item)
        {
            this.Insert(index, (ForControl)item);
        }

        void IList.Remove(object item)
        {
            this.Remove((ForControl)item);
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
        public ForControl this[int index]
        {
            get
            {
                return (ForControl)this.listItems[index];
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
        /// <summary>
        /// False, Collection is not FixedSize
        /// </summary>
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
                this.listItems[index] = (ForControl)value;
            }
        }
        #endregion
    }
}
