using System;
using UnityEngine;

namespace WizardUtils.UI
{
    public class ElementList<T>
    {
        private GameObject Prefab;
        private Transform Root;
        private Action<T, GameObject> CreatedCallback;

        public ElementList(GameObject prefab, Transform root, Action<T, GameObject> createdCallback)
        {
            Prefab = prefab;
            Root = root;
            CreatedCallback = createdCallback;
        }

        public void AddButton(T data)
        {
            GameObject newObj = UnityEngine.Object.Instantiate(Prefab, Root);
            CreatedCallback(data, newObj);
        }
    }
}
