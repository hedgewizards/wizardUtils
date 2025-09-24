using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Tools
{
    /// <summary>
    /// an array of enum values that unity can actually implement a custom propertydrawer for
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public struct MultiEnum<T> : ICollection<T>
        where T : Enum
    {
        [SerializeField]
        private T[] Values;

        public int Count => Values.Count();

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            if (!Values.Contains(item))
            {
                ArrayHelper.InsertAndResize(ref Values, item);
            }
        }

        public void Clear()
        {
            Values = new T[0];
        }

        public bool Contains(T item)
            => Values.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
            => (IEnumerator<T>)Values.GetEnumerator();

        public bool Remove(T item)
        {
            return ArrayHelper.DeleteAndResize(ref Values, item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
