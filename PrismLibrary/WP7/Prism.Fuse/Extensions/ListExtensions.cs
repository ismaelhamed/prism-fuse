namespace System.Collections.Generic
{
    public static class ListExtensions
    {
        /// <summary> 
        /// Removes all elements from the List that match the conditions defined by the specified predicate. 
        /// </summary> 
        /// <typeparam name="T">The type of elements held by the List.</typeparam> 
        /// <param name="list">The List to remove the elements from.</param> 
        /// <param name="match">The Predicate delegate that defines the conditions of the elements to remove.</param> 
        public static int RemoveAll<T>(this List<T> list, Func<T, bool> match)
        {
            var numberRemoved = 0;

            // Loop through every element in the List, in reverse order since we are removing items. 
            for (var i = (list.Count - 1); i >= 0; i--)
            {
                // If the predicate function returns true for this item, remove it from the List. 
                if (!match(list[i])) 
                    continue;

                list.RemoveAt(i);
                numberRemoved++;
            }

            // Return how many items were removed from the List. 
            return numberRemoved;
        } 
    }
}
