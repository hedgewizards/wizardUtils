using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WizardUtils.Tools
{
    /// <summary>
    /// An array ordered from newest to oldest. inserting more than <see cref="MaxSize"/> items overwrites the oldest
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CircleCache<T>
    {
        T[] items;
        int oldestIndex;
        int count;

        public int MaxSize => items.Length;
        public int Count => count;

        public CircleCache(int maxSize)
        {
            items = new T[maxSize];
            count = 0;
        }

        public T Get(int index)
        {
            if (index < 0 || index >= count) throw new IndexOutOfRangeException();

            int offset = (oldestIndex - 1 - index + count) % count;
            return items[offset];
        }

        public void Push(T newItem)
        {
            if (count < MaxSize)
            {
                items[count] = newItem;
                count++;
            }
            else
            {
                items[oldestIndex] = newItem;
                oldestIndex = (oldestIndex + 1 ) % count;
            }
        }
    }
}
