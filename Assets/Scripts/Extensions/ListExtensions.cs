using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public static class ListExtensions
    {
        public static IList<T> QuickSort<T>(this IList<T> list) where T : IComparable
        {
            QuickSortInternal(list, 0, list.Count - 1);
            return list;
        }

        private static void QuickSortInternal<T>(IList<T> list, int left, int right) where T : IComparable
        {
            if(left >= right)
            {
                return;
            }

            var partition = PartitionInternal(list, left, right);
            QuickSortInternal(list, left, partition - 1);
            QuickSortInternal(list, partition + 1, right);
        }

        private static int PartitionInternal<T>(IList<T> list, int left, int right) where T : IComparable
        {
            var partition = list[right];
            var swapIndex = left;
            for (var i = left; i < right; i++)
            {
                var item = list[i];
                if (item.CompareTo(partition) > 0) continue;
                list[i] = list[swapIndex];
                list[swapIndex] = item;
                swapIndex++;
            }
            list[right] = list[swapIndex];
            list[swapIndex] = partition;
            return right;
        }
        
        public static IList<T> Stretch<T>(this IList<T> list, int size)
        {
            var tempArray = list.Take(size).ToList();
            var tempIndex = 0;
            while (tempArray.Count < size)
            {
                tempArray.Add(list[tempIndex]);
                tempIndex = (tempIndex + 1) % list.Count;
            }
            return tempArray;
        }
        
        public static T GetRandomElement<T>(this IList<T> list)
        {
            return list.ElementAtOrDefault(Random.Range(0, list.Count));
        }

        public static int GetRandomIndex<T>(this IList<T> list)
        {
            if (list.Count == 0)
                return -1;
            return Random.Range(0, list.Count);
        }
        
        public static void Swap<T>(this IList<T> list, int k, int n)
        {
            (list[k], list[n]) = (list[n], list[k]);
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var n = list.Count;
            var randomSeed = new System.Random();

            while (n > 1)
            {
                n--;
                int k = randomSeed.Next(n + 1);
                list.Swap(k, n);
            }
        }
        public static IList<T> GetShuffle<T>(this IList<T> list)
        {
            var outList = new T[list.Count];
            list.CopyTo(outList,0);
            outList.Shuffle();
            return outList;
        }
        
        public static void AbsoluteShuffle<T>(this IList<T> list) // TODO Optimize and add max reshuffle count later
        {
            var tempCount = 0;
            var tempArray = new T[list.Count];

            list.CopyTo(tempArray, 0);
            list.Shuffle();

            for (var i = 0; i < list.Count; i++)
                if (list[i].Equals(tempArray[i])) tempCount++; 
            
            if (tempCount == list.Count)
            {
                try 
                { 
                    list.AbsoluteShuffle(); 
                }
                catch(StackOverflowException e) 
                { 
                    Debug.LogWarning(e); 
                }
            }
        }
    }
