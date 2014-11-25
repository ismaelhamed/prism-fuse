using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Practices.Prism
{
    /// <summary>
    /// Represents a collection of associated String keys and String values that can be accessed either with the key 
    /// or with the index.
    /// </summary>
    public class NavigationParameters : IEnumerable<KeyValuePair<string, object>>
    {
        // Fields
        private readonly List<KeyValuePair<string, object>> items;

        /// <summary>
        /// Gets the entry at the specified index of the NameValueCollection.
        /// </summary>
        /// <param name="index">The zero-based index of the entry to locate in the collection.</param>
        /// <returns>A <see cref="T:System.String" /> that contains the comma-separated list of values at the specified index of the collection.</returns>
        public string this[int index]
        {
            get { return Get(items[index].Key); }
        }

        /// <summary>
        /// Gets the entry with the specified key in the NameValueCollection.
        /// </summary>
        /// <param name="name">The String key of the entry to locate. The key can be null.</param>
        /// <returns>A <see cref="T:System.String" /> that contains the comma-separated list of values associated with the specified key, if found; otherwise, null.</returns>
        public string this[string name]
        {
            get { return Get(name); }
        }

        /// <summary>
        /// Gets all the keys in the NameValueCollection.
        /// </summary>
        /// <remarks>If the collection is empty, this method returns an empty String array, not null.</remarks>
        public IEnumerable<string> AllKeys
        {
            get { return items.Select(kvp => kvp.Key).Distinct(); } // <-- No duplicates returned.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationParameters"/> class that is empty, has the default initial 
        /// capacity and uses the default case-insensitive hash code provider and the default case-insensitive comparer.
        /// </summary>
        public NavigationParameters()
        {
            // initialize collection
            items = new List<KeyValuePair<string, object>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationParameters"/> class that is empty, has the specified initial 
        /// capacity and uses the default case-insensitive hash code provider and the default case-insensitive comparer.
        /// </summary>
        /// <param name="capacity"></param>
        public NavigationParameters(int capacity)
        {
            // initialize collection
            items = new List<KeyValuePair<string, object>>(capacity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationParameters"/> class with a query string.
        /// </summary>
        /// <param name="query">The query string.</param>
        internal NavigationParameters(string query)
        {
            // initialize collection
            items = new List<KeyValuePair<string, object>>();

            // parse querystring
            FillFromString(query);
        }

        /// <summary>
        /// Copies the entries in the specified <see cref="T:Microsoft.Practices.Prism.NavigationParameters" /> to the current <see cref="T:Microsoft.Practices.Prism.NavigationParameters" />.
        /// </summary>
        /// <param name="pairs">The <see cref="T:Microsoft.Practices.Prism.NavigationParameters" /> to copy to the current <see cref="T:Microsoft.Practices.Prism.NavigationParameters" />.</param>
        public void Add(IEnumerable<KeyValuePair<string, object>> pairs)
        {
            if (pairs == null)
                throw new ArgumentNullException("pairs");

            foreach (var kvp in pairs)
            {
                Add(kvp.Key, kvp.Value);
            }
        }

        /// <summary>
        /// Adds an entry with the specified name and value to the <see cref="NavigationParameters"/>.
        /// </summary>
        /// <param name="name">The String key of the entry to add. The key can be null.</param>
        /// <param name="value">The Object value of the entry to add. The value can be null.</param>
        public void Add(string name, object value)
        {
            name = name ?? String.Empty;
            value = value ?? String.Empty;

            items.Add(new KeyValuePair<string, object>(name, value));
        }

        /// <summary>
        /// Gets the values associated with the specified name combined into one comma-separated list.
        /// </summary>
        /// <param name="name">The name of the entry that contains the values to get. The name can be null.</param>
        /// <returns>A <see cref="T:System.String" /> that contains a comma-separated list of url encoded values associated with the specified name if found; otherwise, null. The values are Url encoded.</returns>
        public string Get(string name)
        {
            return String.Join(",", GetValuesInternal(name));
        }

        /// <summary>
        /// Gets the values associated with the specified key from the NameValueCollection.
        /// </summary>
        /// <param name="name">The name of the entry that contains the values to get. The name can be null.</param>
        /// <returns>A <see cref="T:System.String" /> that contains url encoded values associated with the name, or null if the name does not exist.</returns>
        public object[] GetValues(string name)
        {
            return GetValuesInternal(name).ToArray();
        }

        private IEnumerable<object> GetValuesInternal(string name)
        {
            return items.Where(kvp => String.Compare(kvp.Key, name, StringComparison.OrdinalIgnoreCase) == 0).Select(kvp => kvp.Value).ToList();
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        /// <summary>
        /// Converts the content of this instance to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of the value of this instance, multiple values with a single key are comma separated.</returns>
        public override string ToString()
        {
            return String.Concat("?", String.Join("&", AllKeys.SelectMany(GetValues, (key, value) => String.Join("=", Uri.EscapeDataString(key), Uri.EscapeDataString(value.ToString())))));
        }

        /// <summary>
        /// Parses a query string into a <see cref="NavigationParameters" />
        /// </summary>
        /// <param name="query">The query string to parse.</param>
        private void FillFromString(string query)
        {
            if (string.IsNullOrEmpty(query))
                return;

            if (query.Length > 0 && query[0] == '?')
                query = query.Substring(1);

            var num1 = query.Length;

            for (var i = 0; i < num1; i++)
            {
                var num2 = i;
                var num3 = -1;

                while (i < num1)
                {
                    var ch = query[i];
                    if (ch == '=')
                    {
                        if (num3 < 0)
                        {
                            num3 = i;
                        }
                    }
                    else if (ch == '&')
                    {
                        break;
                    }

                    i++;
                }

                string str = null;
                string str1;
                if (num3 >= 0)
                {
                    str = query.Substring(num2, num3 - num2);
                    str1 = query.Substring(num3 + 1, i - num3 - 1);
                }
                else
                {
                    str1 = query.Substring(num2, i - num2);
                }

                Add(str != null ? Uri.UnescapeDataString(str) : null, Uri.UnescapeDataString(str1));

                if (i == num1 - 1 && query[i] == '&')
                {
                    Add(null, string.Empty);
                }
            }
        }
    }
}
