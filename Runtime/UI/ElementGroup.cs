using System;
using UnityEngine;

namespace WizardUtils.UI
{
    /// <summary>
    /// Abuse First-Class functions to create a layout full of similar UI elements
    /// </summary>
    /// <typeparam name="T">customization data for elements</typeparam>
    public class ElementGroup<T>
    {
        private GameObject Prefab;
        private Transform Root;
        private Action<T, GameObject> CreatedCallback;

        /// <summary>
        /// Abuse First-Class functions to create a layout full of similar UI elements
        /// </summary>
        /// <param name="prefab">new elements are instantiated as clones of this</param>
        /// <param name="root">new elements are created as children of this transform</param>
        /// <param name="createdCallback">Consume <typeparamref name="T"/> to customize the new <see cref="GameObject"/></param>
        public ElementGroup(GameObject prefab, Transform root, Action<T, GameObject> createdCallback)
        {
            Prefab = prefab;
            Root = root;
            CreatedCallback = createdCallback;
        }

        public void AddElement(T data)
        {
            GameObject newObj = UnityEngine.Object.Instantiate(Prefab, Root);
            CreatedCallback(data, newObj);
        }
    }
}
