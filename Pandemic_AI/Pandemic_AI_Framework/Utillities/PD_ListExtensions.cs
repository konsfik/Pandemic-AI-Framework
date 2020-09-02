using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Pandemic_AI_Framework
{
    public static class PD_ListExtensions
    {
        #region equalities
        public static bool Array_Equal_S<T>(
            this T[] this_array, T[] other_array
            )
            where T : struct
        {
            // compare list sizes
            if (this_array.Length != other_array.Length)
            {
                return false;
            }

            // compare list elements
            for (int i = 0; i < this_array.Length; i++)
            {
                if (this_array[i].Equals(other_array[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool List_Equal_S<T>(
            this List<T> this_list, List<T> other_list
            )
            where T : struct
        {
            // compare list sizes
            if (this_list.Count != other_list.Count)
            {
                return false;
            }

            // compare list elements
            for (int i = 0; i < this_list.Count; i++)
            {
                if (this_list[i].Equals(other_list[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool List_Equal(
            this List<List<int>> this_list, List<List<int>> other_list
            )
        {
            // compare list sizes
            if (this_list.Count != other_list.Count)
            {
                return false;
            }

            // compare list elements
            for (int i = 0; i < this_list.Count; i++)
            {
                if (this_list[i].List_Equal_S(other_list[i]) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Dictionary_Equal(
            this Dictionary<int, string> this_dictionary,
            Dictionary<int, string> other_dictionary
            )
        {
            List<int> this_dictionary_keys = this_dictionary.Keys.ToList();
            List<int> other_dictionary_keys = other_dictionary.Keys.ToList();

            // compare keys - size
            if (this_dictionary_keys.Count != other_dictionary_keys.Count)
            {
                return false;
            }

            for (int k = 0; k < this_dictionary.Keys.Count; k++)
            {
                if (this_dictionary_keys[k].Equals(other_dictionary_keys[k]) == false)
                {
                    return false;
                }
            }

            foreach (int key in this_dictionary_keys)
            {
                string this_value = this_dictionary[key];
                string other_value = other_dictionary[key];
                if (this_value.Equals(other_value) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Dictionary_Equal(
            this Dictionary<int, PD_Point> this_dictionary,
            Dictionary<int, PD_Point> other_dictionary
            )
        {
            List<int> this_dictionary_keys = this_dictionary.Keys.ToList();
            List<int> other_dictionary_keys = other_dictionary.Keys.ToList();

            // compare keys - size
            if (this_dictionary_keys.Count != other_dictionary_keys.Count)
            {
                return false;
            }

            for (int k = 0; k < this_dictionary.Keys.Count; k++)
            {
                if (this_dictionary_keys[k].Equals(other_dictionary_keys[k]) == false)
                {
                    return false;
                }
            }

            foreach (int key in this_dictionary_keys)
            {
                PD_Point this_value = this_dictionary[key];
                PD_Point other_value = other_dictionary[key];
                if (this_value.Equals(other_value) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Dictionary_Equal(
            this Dictionary<int, List<int>> this_dictionary,
            Dictionary<int, List<int>> other_dictionary
            )
        {
            List<int> this_dictionary_keys = this_dictionary.Keys.ToList();
            List<int> other_dictionary_keys = other_dictionary.Keys.ToList();

            // compare keys - size
            if (this_dictionary_keys.Count != other_dictionary_keys.Count)
            {
                return false;
            }

            for (int k = 0; k < this_dictionary.Keys.Count; k++)
            {
                if (this_dictionary_keys[k].Equals(other_dictionary_keys[k]) == false)
                {
                    return false;
                }
            }

            foreach (int key in this_dictionary_keys)
            {
                List<int> this_value = this_dictionary[key];
                List<int> other_value = other_dictionary[key];
                if (this_value.List_Equal_S(other_value) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Dictionary_Equal_S<K, V>(
            this Dictionary<K, V> this_dictionary,
            Dictionary<K, V> other_dictionary
            )
            where K : struct
            where V : struct
        {
            List<K> this_dictionary_keys = this_dictionary.Keys.ToList();
            List<K> other_dictionary_keys = other_dictionary.Keys.ToList();

            // compare keys - size
            if (this_dictionary_keys.Count != other_dictionary_keys.Count)
            {
                return false;
            }

            for (int k = 0; k < this_dictionary.Keys.Count; k++)
            {
                if (this_dictionary_keys[k].Equals(other_dictionary_keys[k]) == false)
                {
                    return false;
                }
            }

            foreach (K key in this_dictionary_keys)
            {
                V this_value = this_dictionary[key];
                V other_value = other_dictionary[key];
                if (this_value.Equals(other_value) == false)
                {
                    return false;
                }
            }

            return true;
        }

        //public static bool Dictionary_Equal(
        //    this Dictionary<int, List<int>> this_dictionary,
        //    Dictionary<int, List<int>> other_dictionary
        //    )
        //{
        //    List<int> this_dictionary_keys = this_dictionary.Keys.ToList();
        //    List<int> other_dictionary_keys = other_dictionary.Keys.ToList();

        //    // compare keys - size
        //    if (this_dictionary_keys.Count != other_dictionary_keys.Count)
        //    {
        //        return false;
        //    }

        //    for (int k = 0; k < this_dictionary.Keys.Count; k++)
        //    {
        //        if (this_dictionary_keys[k] != other_dictionary_keys[k])
        //        {
        //            return false;
        //        }
        //    }

        //    foreach (int key in this_dictionary_keys)
        //    {
        //        List<int> this_value = this_dictionary[key];
        //        List<int> other_value = other_dictionary[key];
        //        if (this_value.SequenceEqual(other_value) == false)
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        public static bool Dictionary_Equal(
            this Dictionary<int, Dictionary<int, int>> this_dictionary,
            Dictionary<int, Dictionary<int, int>> other_dictionary
            )
        {
            List<int> this_dictionary_keys = this_dictionary.Keys.ToList();
            List<int> other_dictionary_keys = other_dictionary.Keys.ToList();

            // compare keys - size
            if (this_dictionary_keys.Count != other_dictionary_keys.Count)
            {
                return false;
            }

            for (int k = 0; k < this_dictionary.Keys.Count; k++)
            {
                if (this_dictionary_keys[k] != other_dictionary_keys[k])
                {
                    return false;
                }
            }

            foreach (int key in this_dictionary_keys)
            {
                Dictionary<int, int> this_value = this_dictionary[key];
                Dictionary<int, int> other_value = other_dictionary[key];
                if (this_value.SequenceEqual(other_value) == false)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Dictionary_Equal(
            this Dictionary<int, int> this_dictionary,
            Dictionary<int, int> other_dictionary
            )
        {
            List<int> this_dictionary_keys = this_dictionary.Keys.ToList();
            List<int> other_dictionary_keys = other_dictionary.Keys.ToList();

            // compare keys - size
            if (this_dictionary_keys.Count != other_dictionary_keys.Count)
            {
                return false;
            }

            for (int k = 0; k < this_dictionary.Keys.Count; k++)
            {
                if (this_dictionary_keys[k] != other_dictionary_keys[k])
                {
                    return false;
                }
            }

            foreach (int key in this_dictionary_keys)
            {
                int this_value = this_dictionary[key];
                int other_value = other_dictionary[key];
                if (this_value != other_value)
                {
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region custom hash codes
        public static int Custom_HashCode(this int[] my_array)
        {
            int hash = 7;
            hash = hash * 13 + my_array.Length;
            foreach (int element in my_array)
            {
                hash = hash * 13 + element;
            }
            return hash;
        }

        public static int Custom_HashCode(this List<int> my_list)
        {
            int hash = 7;
            hash = hash * 13 + my_list.Count;
            foreach (int element in my_list)
            {
                hash = hash * 13 + element;
            }
            return hash;
        }

        public static int Custom_HashCode(this List<List<int>> my_list)
        {
            int hash = 7;
            hash = hash * 13 + my_list.Count;
            foreach (List<int> element in my_list)
            {
                hash = hash * 13 + element.Custom_HashCode();
            }
            return hash;
        }

        public static int Custom_HashCode(this Dictionary<int, int> my_dictionary)
        {
            int hash = 7;
            hash = hash * 13 + my_dictionary.Count;
            foreach (KeyValuePair<int, int> key_value_pair in my_dictionary)
            {
                hash = hash * 13 + key_value_pair.Key;
                hash = hash * 13 + key_value_pair.Value;
            }
            return hash;
        }

        public static int Custom_HashCode(this Dictionary<int, string> my_dictionary)
        {
            int hash = 7;
            hash = hash * 13 + my_dictionary.Count;
            foreach (KeyValuePair<int, string> key_value_pair in my_dictionary)
            {
                hash = hash * 13 + key_value_pair.Key;
                hash = hash * 13 + key_value_pair.Value.GetHashCode();
            }
            return hash;
        }

        public static int Custom_HashCode(this Dictionary<int, bool> my_dictionary)
        {
            int hash = 7;
            hash = hash * 13 + my_dictionary.Count;
            foreach (KeyValuePair<int, bool> key_value_pair in my_dictionary)
            {
                hash = hash * 13 + key_value_pair.Key;
                hash = hash * 13 + key_value_pair.Value.GetHashCode();
            }
            return hash;
        }

        public static int Custom_HashCode(this Dictionary<int, PD_Point> my_dictionary)
        {
            int hash = 7;
            hash = hash * 13 + my_dictionary.Count;
            foreach (KeyValuePair<int, PD_Point> key_value_pair in my_dictionary)
            {
                hash = hash * 13 + key_value_pair.Key;
                hash = hash * 13 + key_value_pair.Value.GetHashCode();
            }
            return hash;
        }

        public static int Custom_HashCode(this Dictionary<int, List<int>> my_dictionary)
        {
            int hash = 7;
            hash = hash * 13 + my_dictionary.Count;
            foreach (KeyValuePair<int, List<int>> key_value_pair in my_dictionary)
            {
                hash = hash * 13 + key_value_pair.Key;
                hash = hash * 13 + key_value_pair.Value.Custom_HashCode();
            }
            return hash;
        }

        public static int Custom_HashCode(this Dictionary<int, Dictionary<int,int>> my_dictionary)
        {
            int hash = 7;
            hash = hash * 13 + my_dictionary.Count;
            foreach (KeyValuePair<int, Dictionary<int, int>> key_value_pair in my_dictionary)
            {
                hash = hash * 13 + key_value_pair.Key;
                hash = hash * 13 + key_value_pair.Value.Custom_HashCode();
            }
            return hash;
        }

        #endregion

        public static bool ContainsAll<T>(this List<T> mainList, List<T> containedList)
        {
            foreach (T item in containedList)
            {
                if (mainList.Contains(item) == false)
                {
                    return false;
                }
            }
            return true;
        }

        public static List<int> CustomDeepCopy(this List<int> originalList)
        {
            List<int> copyList = new List<int>();

            foreach (int element in originalList)
            {
                copyList.Add(element);
            }

            return copyList;
        }

        public static List<T> CustomDeepCopy<T>(this List<T> originalList)
            where T : ICustomDeepCopyable<T>
        {
            List<T> copyList = new List<T>();

            foreach (var element in originalList)
            {
                copyList.Add(element.GetCustomDeepCopy());
            }

            return copyList;
        }

        public static List<PD_PlayerCardBase> CustomDeepCopy(this List<PD_PlayerCardBase> originalList)
        {
            List<PD_PlayerCardBase> copyList = new List<PD_PlayerCardBase>();

            foreach (var element in originalList)
            {
                if (element.GetType() == typeof(PD_CityCard))
                {
                    copyList.Add(((PD_CityCard)element).GetCustomDeepCopy());
                }
                else if (element.GetType() == typeof(PD_InfectionCard))
                {
                    copyList.Add(((PD_InfectionCard)element).GetCustomDeepCopy());
                }
                else if (element.GetType() == typeof(PD_EpidemicCard))
                {
                    copyList.Add(((PD_EpidemicCard)element).GetCustomDeepCopy());
                }

            }

            return copyList;
        }

        public static List<List<int>> CustomDeepCopy(this List<List<int>> originalListOfLists)
        {
            List<List<int>> copyListOfLists = new List<List<int>>();

            foreach (var subList in originalListOfLists)
            {
                copyListOfLists.Add(subList.CustomDeepCopy());
            }

            return copyListOfLists;
        }

        public static List<List<PD_PlayerCardBase>> CustomDeepCopy(this List<List<PD_PlayerCardBase>> originalListOfLists)
        {
            List<List<PD_PlayerCardBase>> copyListOfLists = new List<List<PD_PlayerCardBase>>();

            foreach (var subList in originalListOfLists)
            {
                copyListOfLists.Add(subList.CustomDeepCopy());
            }

            return copyListOfLists;
        }

        public static Dictionary<int, int> CustomDeepCopy(
            this Dictionary<int, int> originalDictionary
            )
        {
            Dictionary<int, int> dictionaryCopy = new Dictionary<int, int>();

            foreach (var kvp in originalDictionary)
            {
                dictionaryCopy.Add(kvp.Key, kvp.Value);
            }

            return dictionaryCopy;
        }

        public static Dictionary<int, Dictionary<int, int>> CustomDeepCopy(
            this Dictionary<int, Dictionary<int, int>> originalDictionary
            )
        {
            Dictionary<int, Dictionary<int, int>> dictionaryCopy
                = new Dictionary<int, Dictionary<int, int>>();

            foreach (var kvp in originalDictionary)
            {
                dictionaryCopy.Add(kvp.Key, kvp.Value.CustomDeepCopy());
            }

            return dictionaryCopy;
        }

        public static Dictionary<int, bool> CustomDeepCopy(
            this Dictionary<int, bool> originalDictionary
            )
        {
            Dictionary<int, bool> dictionaryCopy = new Dictionary<int, bool>();

            foreach (var kvp in originalDictionary)
            {
                dictionaryCopy.Add(kvp.Key, kvp.Value);
            }

            return dictionaryCopy;
        }

        public static Dictionary<int, List<int>> CustomDeepCopy(
            this Dictionary<int, List<int>> originalDictionary
            )
        {
            Dictionary<int, List<int>> dictionaryCopy = new Dictionary<int, List<int>>();

            foreach (var kvp in originalDictionary)
            {
                dictionaryCopy.Add(kvp.Key, kvp.Value.CustomDeepCopy());
            }

            return dictionaryCopy;
        }

        public static Dictionary<int, string> CustomDeepCopy(
            this Dictionary<int, string> originalDictionary
            )
        {
            Dictionary<int, string> dictionaryCopy = new Dictionary<int, string>();

            foreach (var kvp in originalDictionary)
            {
                dictionaryCopy.Add(kvp.Key, kvp.Value);
            }

            return dictionaryCopy;
        }

        public static Dictionary<int, PD_Point> CustomDeepCopy(
            this Dictionary<int, PD_Point> originalDictionary
            )
        {
            Dictionary<int, PD_Point> dictionaryCopy = new Dictionary<int, PD_Point>();

            foreach (var kvp in originalDictionary)
            {
                dictionaryCopy.Add(kvp.Key, kvp.Value.GetCustomDeepCopy());
            }

            return dictionaryCopy;
        }

        public static Dictionary<PD_CityEdge_Directed, List<PD_City>> CustomDeepCopy(
            this Dictionary<PD_CityEdge_Directed, List<PD_City>> originalDictionary
            )
        {
            Dictionary<PD_CityEdge_Directed, List<PD_City>> dictionaryCopy =
                new Dictionary<PD_CityEdge_Directed, List<PD_City>>();

            foreach (var kvp in originalDictionary)
            {
                var key = kvp.Key;
                var value = kvp.Value;
                dictionaryCopy.Add(key.GetCustomDeepCopy(), value.CustomDeepCopy());
            }

            return dictionaryCopy;
        }

        public static Dictionary<int, List<PD_City>> CustomDeepCopy(
            this Dictionary<int, List<PD_City>> originalDictionary
            )
        {
            Dictionary<int, List<PD_City>> dictionaryCopy =
                new Dictionary<int, List<PD_City>>();

            foreach (var kvp in originalDictionary)
            {
                var key = kvp.Key;
                List<PD_City> value = kvp.Value;
                dictionaryCopy.Add(key, value.CustomDeepCopy());
            }

            return dictionaryCopy;
        }

        public static Dictionary<PD_City, List<PD_City>> CustomDeepCopy(
            this Dictionary<PD_City, List<PD_City>> originalDictionary
            )
        {
            Dictionary<PD_City, List<PD_City>> dictionaryCopy =
                new Dictionary<PD_City, List<PD_City>>();

            foreach (var kvp in originalDictionary)
            {
                var key = kvp.Key;
                var value = kvp.Value;
                dictionaryCopy.Add(key.GetCustomDeepCopy(), value.CustomDeepCopy());
            }

            return dictionaryCopy;
        }

        public static Dictionary<int, List<PD_PlayerCardBase>> CustomDeepCopy(
            this Dictionary<int, List<PD_PlayerCardBase>> originalDictionary)
        {
            Dictionary<int, List<PD_PlayerCardBase>> dictionaryCopy =
                new Dictionary<int, List<PD_PlayerCardBase>>();

            foreach (var kvp in originalDictionary)
            {
                int key = kvp.Key;
                List<PD_PlayerCardBase> value = kvp.Value;
                dictionaryCopy.Add(key, value.CustomDeepCopy());
            }

            return dictionaryCopy;
        }

        public static Dictionary<PD_Player, List<PD_PlayerCardBase>> CustomDeepCopy(
            this Dictionary<PD_Player, List<PD_PlayerCardBase>> originalDictionary)
        {
            Dictionary<PD_Player, List<PD_PlayerCardBase>> dictionaryCopy =
                new Dictionary<PD_Player, List<PD_PlayerCardBase>>();

            foreach (var kvp in originalDictionary)
            {
                var key = kvp.Key;
                var value = kvp.Value;
                dictionaryCopy.Add(key.GetCustomDeepCopy(), value.CustomDeepCopy());
            }

            return dictionaryCopy;
        }

        public static Dictionary<T, List<T>> CustomDeepCopy<T>(this Dictionary<T, List<T>> originalDictionary)
            where T : ICustomDeepCopyable<T>
        {
            Dictionary<T, List<T>> dictionaryCopy = new Dictionary<T, List<T>>();

            foreach (var kvp in originalDictionary)
            {
                var key = kvp.Key;
                var value = kvp.Value;
                dictionaryCopy.Add(key.GetCustomDeepCopy(), value.CustomDeepCopy());
            }

            return dictionaryCopy;
        }

        public static Dictionary<T, List<R>> CustomDeepCopy<T, R>(this Dictionary<T, List<R>> originalDictionary)
            where T : ICustomDeepCopyable<T>
            where R : ICustomDeepCopyable<R>
        {
            Dictionary<T, List<R>> dictionaryCopy = new Dictionary<T, List<R>>();

            foreach (var kvp in originalDictionary)
            {
                var key = kvp.Key;
                var value = kvp.Value;
                dictionaryCopy.Add(key.GetCustomDeepCopy(), value.CustomDeepCopy());
            }

            return dictionaryCopy;
        }

        public static Dictionary<T, R> CustomDeepCopy<T, R>(this Dictionary<T, R> originalDictionary)
            where T : ICustomDeepCopyable<T>
            where R : ICustomDeepCopyable<R>
        {
            Dictionary<T, R> dictionaryCopy = new Dictionary<T, R>();

            foreach (var kvp in originalDictionary)
            {
                var key = kvp.Key;
                var value = kvp.Value;
                dictionaryCopy.Add(key.GetCustomDeepCopy(), value.GetCustomDeepCopy());
            }

            return dictionaryCopy;
        }

        public static Dictionary<int, T> CustomDeepCopy<T>(this Dictionary<int, T> originalDictionary)
            where T : ICustomDeepCopyable<T>
        {
            Dictionary<int, T> dictionaryCopy = new Dictionary<int, T>();

            foreach (var kvp in originalDictionary)
            {
                var key = kvp.Key;
                var value = kvp.Value;
                dictionaryCopy.Add(key, value.GetCustomDeepCopy());
            }

            return dictionaryCopy;
        }

        public static Dictionary<int, PD_ME_PlayerPawn> CustomDeepCopy(this Dictionary<int, PD_ME_PlayerPawn> originalDictionary)
        {
            Dictionary<int, PD_ME_PlayerPawn> dictionaryCopy = new Dictionary<int, PD_ME_PlayerPawn>();

            foreach (var kvp in originalDictionary)
            {
                var key = kvp.Key;
                var value = kvp.Value;
                dictionaryCopy.Add(key, value.GetCustomDeepCopy());
            }

            return dictionaryCopy;
        }

        public static Dictionary<int, List<T>> CustomDeepCopy<T>(this Dictionary<int, List<T>> originalDictionary)
            where T : ICustomDeepCopyable<T>
        {
            Dictionary<int, List<T>> dictionaryCopy = new Dictionary<int, List<T>>();

            foreach (var kvp in originalDictionary)
            {
                int key = kvp.Key;
                var value = kvp.Value;
                dictionaryCopy.Add(key, value.CustomDeepCopy());
            }

            return dictionaryCopy;
        }

        public static List<List<T>> CustomDeepCopy<T>(this List<List<T>> originalListOfLists) where T : ICustomDeepCopyable<T>
        {
            List<List<T>> listOfListsCopy = new List<List<T>>();
            foreach (var list in originalListOfLists)
            {
                listOfListsCopy.Add(list.CustomDeepCopy());
            }
            return listOfListsCopy;
        }



        public static List<List<T>> GetNonSamePairs<T>(this List<T> originalList)
        {
            if (originalList.Count < 2)
            {
                return new List<List<T>>();
            }
            else
            {
                int originalListSize = originalList.Count;
                List<List<T>> listOfPairs = new List<List<T>>();
                for (int i = 0; i < originalListSize; i++)
                {
                    for (int j = 0; j < originalListSize; j++)
                    {
                        if (i != j)
                        {
                            T element1 = originalList[i];
                            T element2 = originalList[j];
                            List<T> pair = new List<T>() {
                                element1, element2
                            };
                            listOfPairs.Add(pair);
                        }
                    }
                }
                return listOfPairs;
            }
        }

        //private static Random rnd;
        //static PD_ListExtensions()
        //{
        //    rnd = new Random();
        //}


        public static void SetFirstItemByIndex<T>(
            this List<T> items,
            int index
            )
        {
            List<T> reorderedItems = new List<T>();
            for (int i = index; i < items.Count; i++)
            {
                reorderedItems.Add(items[i]);
            }
            for (int i = 0; i < index; i++)
            {
                reorderedItems.Add(items[i]);
            }

            items.Clear();

            foreach (var rItem in reorderedItems)
            {
                items.Add(rItem);
            }

            //items = new List<T>(reorderedItems);
        }

        #region Getters
        /// <summary>
        /// Returns the last element of a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T GetLast<T>(
            this List<T> items
            )
        {
            if (items.Count > 0)
            {
                int lastItemIndex = items.Count - 1;
                return items[lastItemIndex];
            }
            else
            {
                throw new System.Exception("List is empty, cannot return last element of empty list.");
            }
        }

        /// <summary>
        /// Returns the first element of a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T GetFirst<T>(
            this List<T> items
            )
        {
            if (items.Count > 0)
            {
                int firstItemIndex = 0;
                return items[firstItemIndex];
            }
            else
            {
                throw new System.Exception("List is empty, cannot return first element of empty list.");
            }
        }

        /// <summary>
        /// Gets a random item from the list
        /// returns it but does not remove it from the list.
        /// In order to remove at the same time
        /// use DrawOneRandom instead
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T GetOneRandom<T>(
            this List<T> items,
            Random randomness_provider
            )
        {
            if (items.Count < 1)
            {
                throw new System.Exception("List is empty, cannot get random element");
            }
            else if (items.Count == 1)
            {
                return items[0];
            }
            //rnd = new Random();
            int randomIndex = randomness_provider.Next(0, items.Count);
            //int randomIndex = UnityEngine.Random.Range(0, items.Count);
            T randomItem = items[randomIndex];
            return randomItem;
        }

        /// <summary>
        /// Returns a number of random items from one list,
        /// without returning the same item twice.
        /// Items can be the same, if the list contained the same items...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="numRandomUniqueItemsToGet"></param>
        /// <returns></returns>
        public static List<T> GetManyRandomUnique<T>(
            this List<T> items,
            Random randomness_provider,
            int numRandomUniqueItemsToGet)
        {
            // check whether the amount of requested items exists
            if (numRandomUniqueItemsToGet > items.Count)
            {
                throw new System.Exception("Requested number of items is greater than existing number of items");
            }

            // create a temporary list of items (from which items will be drawn)
            List<T> tempItems = new List<T>(items);

            // create temporary list of items that will be returned
            List<T> itemsToGet = new List<T>();

            // repeat process as many times as the items requested
            for (int i = 0; i < numRandomUniqueItemsToGet; i++)
            {
                // remove one random item from the temp items list
                // and add it to the items to get list.
                itemsToGet.Add(tempItems.DrawOneRandom(randomness_provider));
            }

            return itemsToGet;

        }
        #endregion

        #region Drawers

        /// <summary>
        /// Draws all items from the list of items (cards)
        /// and also clears the list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static List<T> DrawAll<T>(this List<T> items)
        {
            List<T> drawnItems = new List<T>(items);
            items.Clear();
            return drawnItems;
        }

        /// <summary>
        /// Draws a specific item from the list,
        /// based on the item's index in the list.
        /// Removes the item from the original list in the process.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T DrawOneByIndex<T>(this List<T> items, int index)
        {
            T drawnItem = items[index];
            items.RemoveAt(index);
            return drawnItem;
        }

        /// <summary>
        /// Draws the last element of a list
        /// (removes it from the list itself and returns it)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T DrawLast<T>(this List<T> items)
        {
            if (items.Count > 0)
            {
                int lastItemIndex = items.Count - 1;
                T item = items[lastItemIndex];
                items.Remove(item);
                return item;
            }
            else
            {
                throw new System.Exception("List is empty, cannot draw last element of empty list.");
            }
        }

        /// <summary>
        /// Draws the first element of a list
        /// (removes it from the list itself and returns it)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T DrawFirst<T>(this List<T> items)
        {
            if (items.Count > 0)
            {
                int firstItemIndex = 0;
                T item = items[firstItemIndex];
                items.Remove(item);
                return item;
            }
            else
            {
                throw new System.Exception("List is empty, cannot draw first element of empty list.");
            }
        }

        /// <summary>
        /// Draws a random item from the list
        /// returns it and also removes it from the list itself
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T DrawOneRandom<T>(
            this List<T> items,
            Random randomness_provider
            )
        {
            int randomIndex = randomness_provider.Next(items.Count);
            T drawnItem = items[randomIndex];
            items.RemoveAt(randomIndex);

            return drawnItem;
        }

        /// <summary>
        /// Draws a number of random items
        /// and also removes them from the list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="numberOfItems"></param>
        /// <returns></returns>
        public static List<T> DrawManyRandomUnique<T>(
            this List<T> items,
            Random randomness_provider,
            int numberOfUniqueItemsToDraw)
        {
            // check whether the amount of requested items exists
            if (numberOfUniqueItemsToDraw > items.Count)
            {
                throw new System.Exception("Requested number of items is greater than existing number of items");
            }

            // initialize the temporary list of drawn items (it will be returned)
            List<T> drawnItems = new List<T>();

            // repeat as many times as the requested items
            for (int i = 0; i < numberOfUniqueItemsToDraw; i++)
            {
                // draw one random from the items list and add it to the drawn items list
                drawnItems.Add(items.DrawOneRandom(randomness_provider));
            }

            return drawnItems;
        }

        #endregion

        public static void Shuffle<T>(
            this List<T> items,
            Random randomness_provider
            )
        {
            List<T> shuffledItems = new List<T>();

            for (int i = items.Count - 1; i >= 0; i--)
            {
                shuffledItems.Add(items.DrawOneRandom(randomness_provider));
            }

            items.Clear();
            for (int i = shuffledItems.Count - 1; i >= 0; i--)
            {
                items.Add(shuffledItems.DrawOneByIndex(i));
            }

        }

        public static T GetLastElementOfLastSubList<T>(
            this List<List<T>> listOfLists
            )
        {
            var lastSubList = listOfLists.GetLast();
            var lastElement = lastSubList.GetLast();
            return lastElement;
        }

        public static T DrawLastElementOfLastSubList<T>(
            this List<List<T>> listOfLists
            )
        {
            var lastSubList = listOfLists.GetLast();
            if (lastSubList.Count > 0)
            {
                int lastItemIndex = lastSubList.Count - 1;
                T item = lastSubList[lastItemIndex];
                lastSubList.RemoveAt(lastItemIndex);

                if (lastSubList.Count <= 0)
                {
                    listOfLists.RemoveAt(listOfLists.Count - 1);
                }

                return item;
            }
            else
            {
                throw new System.Exception("List is empty, cannot draw last element of empty list.");
            }
        }

        public static T GetFirstElementOfFirstSubList<T>(
            this List<List<T>> listOfLists
            )
        {
            var firstSubList = listOfLists.GetFirst();
            var firstElement = firstSubList.GetFirst();
            return firstElement;
        }

        public static T DrawFirstElementOfFirstSubList<T>(
            this List<List<T>> listOfLists
            )
        {
            var firstSubList = listOfLists.GetFirst();
            if (firstSubList.Count > 0)
            {
                int firstItemIndex = 0;
                T item = firstSubList[firstItemIndex];
                firstSubList.RemoveAt(firstItemIndex);

                if (firstSubList.Count <= 0)
                {
                    // remove first sublist
                    listOfLists.RemoveAt(0);
                }

                return item;
            }
            else
            {
                throw new System.Exception("List is empty, cannot draw last element of empty list.");
            }
        }

        public static List<T> DrawAllElementsOfAllSubListsAsOneList<T>(
            this List<List<T>> listOfLists
            )
        {
            List<T> newList = new List<T>();
            while (listOfLists.Count > 0)
            {
                newList.Add(listOfLists.DrawFirstElementOfFirstSubList());
            }
            return newList;
        }

        public static List<T> GetAllElementsOfAllSublistsAsOneList<T>(
            this List<List<T>> listOfLists
            )
        {
            List<T> newList = new List<T>();
            foreach (var sublist in listOfLists)
            {
                foreach (var element in sublist)
                {
                    newList.Add(element);
                }
            }
            return newList;
        }

        public static void ShuffleAllSubListsElements<T>(
            this List<List<T>> listOfLists,
            Random randomness_provider
            )
        {
            foreach (var subList in listOfLists)
            {
                subList.Shuffle(randomness_provider);
            }
        }

        public static int GetNumberOfElementsOfAllSubLists<T>(
            this List<List<T>> listOfLists
            )
        {
            int numberOfElements = 0;
            foreach (var subList in listOfLists)
            {
                numberOfElements += subList.Count;
            }
            return numberOfElements;
        }

        public static List<List<T>> GetAllSubSetsOfSpecificSize<T>(
            this List<T> originalGroup,
            int subGroupsSize
            )
        {
            if (subGroupsSize > originalGroup.Count)
            {
                throw new System.Exception("error!");
            }

            if (subGroupsSize == originalGroup.Count)
            {
                return new List<List<T>>() { originalGroup };
            }

            List<List<T>> subGroups = new List<List<T>>();
            int originalGroupSize = originalGroup.Count;

            int numPossibilities = 0;
            for (int i = 0; i < originalGroupSize; i++)
            {
                numPossibilities += (int)Math.Pow(2, i);
            }

            List<string> allPossibilities = new List<string>();

            for (int i = 0; i < numPossibilities; i++)
            {
                string possibility = Convert.ToString(i, 2).PadLeft(originalGroupSize, '0');
                allPossibilities.Add(possibility);
            }

            List<string> legitimatePossibilities = new List<string>();
            foreach (var possibility in allPossibilities)
            {
                int numOnesInPos = possibility.Count(x => x == '1');
                if (numOnesInPos == subGroupsSize)
                {
                    legitimatePossibilities.Add(possibility);
                }
            }

            foreach (var possibility in legitimatePossibilities)
            {
                List<T> subGroup = new List<T>();

                for (int i = 0; i < possibility.Length; i++)
                {
                    char c = possibility[i];
                    if (c == '1')
                    {
                        subGroup.Add(originalGroup[i]);
                    }
                }

                subGroups.Add(subGroup);
            }

            return subGroups;
        }
    }
}