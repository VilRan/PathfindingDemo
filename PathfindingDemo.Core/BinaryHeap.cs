﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathfindingDemo
{
    /// <summary>
    /// A data structure that automatically keeps itself sorted when adding and removing items.
    /// If the items are modified while on the heap, manual sorting may be necessary.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinaryHeap<T> : IEnumerable<T> where T : IComparable<T>
    {
        T[] items;
        int itemCount = 0;

        public int Count { get { return itemCount; } }

        public BinaryHeap()
        {
            items = new T[7];
        }

        public BinaryHeap(int size)
        {
            items = new T[size];
        }

        public void Add(T item)
        {
            if (itemCount == items.Length)
                Resize(items.Length * 2);

            items[itemCount] = item;

            int position = itemCount;
            int parent = GetParent(position);
            while (position > 0 && items[position].CompareTo(items[parent]) <= 0)
            {
                Swap(position, parent);
                position = parent;
                parent = GetParent(position);
            }

            itemCount++;
        }

        /// <summary>
        /// Returns the top of the heap without removing it.
        /// </summary>
        /// <returns></returns>
        public T Peek()
        {
            return items[0];
        }

        /// <summary>
        /// Removes and returns the top of the heap. 
        /// As long as the items in the heap haven't modified, this is guaranteed to be the minimum item.
        /// If you do modify the values that affect an items comparisons, call the Sort() method first.
        /// </summary>
        /// <returns></returns>
        public T Remove()
        {
            return RemoveAt(0);
        }

        public T RemoveAt(int position)
        {
            if (position >= itemCount)
                return default(T);

            T item = items[position];
            itemCount--;
            items[position] = items[itemCount];
            items[itemCount] = default(T);
            
            while (true)
            {
                int child = GetSmallerChild(position);
                if (child == position)
                    break;

                if (items[position].CompareTo(items[child]) > 0)
                {
                    Swap(position, child);
                    position = child;
                }
                else
                    break;
            }

            return item;
        }

        public void Remove(T item)
        {
            int position = Array.IndexOf(items, item);
            RemoveAt(position);
        }

        /// <summary>
        /// Using this method is necessary if and only if you modify an item in the heap in a way that affects its comparison with other items.
        /// </summary>
        public void Sort()
        {
            Array.Sort(items, 0, itemCount);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        void Resize(int newLength)
        {
            T[] newData = new T[newLength];
            Array.Copy(items, newData, items.Length);
            items = newData;
        }

        void Swap(int positionA, int positionB)
        {
            T dataB = items[positionB];
            items[positionB] = items[positionA];
            items[positionA] = dataB;
        }

        int GetParent(int position)
        {
            return (position - 1) / 2;
        }

        int GetLeftChild(int position)
        {
            return position * 2 + 1;
        }

        int GetRightChild(int position)
        {
            return position * 2 + 2;
        }

        int GetSmallerChild(int position)
        {
            int leftChild = GetLeftChild(position);
            if (leftChild >= itemCount)
                return position;

            int rightChild = GetRightChild(position);
            if (rightChild >= itemCount || items[leftChild].CompareTo(items[rightChild]) < 0)
                return leftChild;
            else
                return rightChild;
        }
    }
}
