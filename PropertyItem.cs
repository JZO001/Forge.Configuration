/* *********************************************************************
 * Date: 20 Feb 2008
 * Created by: Zoltan Juhasz
 * E-Mail: forge@jzo.hu
***********************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Forge.Configuration.Shared;
using Forge.Shared;

namespace Forge.Configuration
{

    /// <summary>
    /// Property data representation
    /// </summary>
    [Serializable]
    [DebuggerDisplay("[{GetType().Name}, Id = '{Id}', Value = '{Value}']")]
    public sealed class PropertyItem : ICloneable, IEnumerable<IPropertyItem>, IPropertyItem
    {

        #region Field(s)

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string mId = string.Empty;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string mValue = string.Empty;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Dictionary<string, PropertyItem> mPropertyItems = null;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Prevents a default instance of the <see cref="PropertyItem"/> class from being created.
        /// </summary>
        public PropertyItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyItem"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public PropertyItem(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                ThrowHelper.ThrowArgumentNullException("id");
            }
            mId = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyItem"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        public PropertyItem(string id, string value)
            : this(id)
        {
            mValue = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IPropertyItem"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="propertyItems">The property items.</param>
        public PropertyItem(string id, IEnumerable<IPropertyItem> propertyItems)
            : this(id)
        {
            if (propertyItems != null)
            {
                mPropertyItems = new Dictionary<string, PropertyItem>();
                foreach (PropertyItem pi in propertyItems)
                {
                    mPropertyItems.Add(pi.Id, pi);
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IPropertyItem"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="value">The value.</param>
        /// <param name="propertyItems">The property items.</param>
        public PropertyItem(string id, string value, IEnumerable<IPropertyItem> propertyItems)
            : this(id, propertyItems)
        {
            mValue = value;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [DebuggerHidden]
        public string Id
        {
            get { return mId; }
            set { mId = value; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [DebuggerHidden]
        public string Value
        {
            get { return mValue; }
            set { mValue = value; }
        }

        /// <summary>
        /// Gets the property items.
        /// </summary>
        /// <value>
        /// The property items.
        /// </value>
        [DebuggerHidden]
        public Dictionary<string, PropertyItem> Items
        {
            get
            {
                if (mPropertyItems == null)
                {
                    lock (this)
                    {
                        if (mPropertyItems == null)
                        {
                            mPropertyItems = new Dictionary<string, PropertyItem>();
                        }
                    }
                }
                return mPropertyItems;
            }
        }

        Dictionary<string, IPropertyItem> IPropertyItem.Items
        {
            get
            {
                if (mPropertyItems == null)
                {
                    lock (this)
                    {
                        if (mPropertyItems == null)
                        {
                            mPropertyItems = new Dictionary<string, PropertyItem>();
                        }
                    }
                }
                Dictionary<string, IPropertyItem> result = new Dictionary<string, IPropertyItem>();
                foreach(KeyValuePair<string, PropertyItem> kv in mPropertyItems)
                {
                    result.Add(kv.Key, kv.Value);
                }
                return result;
            }
        }

        #endregion

        #region Public method(s)

        /// <summary>
        /// Creates the category property item.
        /// </summary>
        /// <returns></returns>
        public CategoryPropertyItem CreateCategoryPropertyItem()
        {
            CategoryPropertyItem result = new CategoryPropertyItem();

            result.Id = Id;
            result.EntryValue = Value;
            result.PropertyItems = new CategoryPropertyItems();
            foreach (PropertyItem pi in Items.Values)
            {
                result.PropertyItems.Add(pi.CreateCategoryPropertyItem());
            }

            return result;
        }

        /// <summary>
        /// Gets the value by path.
        /// </summary>
        /// <param name="propertyItems">The property items.</param>
        /// <param name="configPath">The config path.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// propertyItems
        /// or
        /// configPath
        /// </exception>
        public static string GetValueByPath(Dictionary<string, IPropertyItem> propertyItems, string configPath)
        {
            if (propertyItems == null)
            {
                throw new ArgumentNullException("propertyItems");
            }
            if (string.IsNullOrEmpty(configPath))
            {
                throw new ArgumentNullException("configPath");
            }

            List<string> keys = new List<string>(configPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries));
            string result = FindValueByKey(propertyItems, keys);
            keys.Clear();
            return result;
        }

        /// <summary>
        /// Gets the category property by path.
        /// </summary>
        /// <param name="propertyItems">The property items.</param>
        /// <param name="configPath">The config path.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// propertyItems
        /// or
        /// configPath
        /// </exception>
        public static IPropertyItem GetCategoryPropertyByPath(Dictionary<string, IPropertyItem> propertyItems, string configPath)
        {
            if (propertyItems == null)
            {
                throw new ArgumentNullException("propertyItems");
            }
            if (string.IsNullOrEmpty(configPath))
            {
                throw new ArgumentNullException("configPath");
            }

            List<string> keys = new List<string>(configPath.Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries));
            IPropertyItem result = FindCategoryPropertyByKey(propertyItems, keys);
            keys.Clear();
            return result;
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            PropertyItem cloned = new PropertyItem();

            cloned.mId = mId;
            cloned.mValue = mValue;

            if (mPropertyItems != null)
            {
                foreach (KeyValuePair<string, PropertyItem> kv in mPropertyItems)
                {
                    cloned.Items.Add(kv.Key, (PropertyItem)kv.Value.Clone());
                }
            }

            return cloned;
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IPropertyItem> GetEnumerator()
        {
            return Items.Values.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Private static method(s)

        private static string FindValueByKey(Dictionary<string, IPropertyItem> propertyItems, List<string> keys)
        {
            string result = null;
            if (propertyItems.ContainsKey(keys[0]))
            {
                IPropertyItem item = propertyItems[keys[0]];
                if (item.Id.Equals(keys[0]))
                {
                    if (keys.Count == 1)
                    {
                        result = item.Value;
                    }
                    else
                    {
                        keys.RemoveAt(0);
                        result = FindValueByKey(item.Items, keys);
                    }
                }
            }
            return result;
        }

        private static IPropertyItem FindCategoryPropertyByKey(Dictionary<string, IPropertyItem> propertyItems, List<string> keys)
        {
            IPropertyItem result = null;
            if (propertyItems.ContainsKey(keys[0]))
            {
                IPropertyItem item = propertyItems[keys[0]];
                if (item.Id.Equals(keys[0]))
                {
                    if (keys.Count == 1)
                    {
                        result = item;
                    }
                    else
                    {
                        keys.RemoveAt(0);
                        result = FindCategoryPropertyByKey(item.Items, keys);
                    }
                }
            }
            return result;
        }

        #endregion

    }

}
