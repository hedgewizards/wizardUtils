using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using WizardUtils.UI.Basics;

namespace WizardUtils.UI
{
    /// <summary>
    /// Simple utility to spawn prefab buttons into a layout. if you want to extend this, try <see cref="ElementList{T}"/>
    /// </summary>
    public class BasicButtonList
    {
        [SerializeField]
        private GameObject Prefab;
        [SerializeField]
        private Transform Root;

        public BasicButtonList(GameObject prefab, Transform root)
        {
            Prefab = prefab;
            Root = root;
        }

        public void AddButton(string text, Action clickedAction)
        {
            IListableButton newObj = UnityEngine.Object.Instantiate(Prefab, Root).GetComponent<IListableButton>();
            newObj.AddOnClickListener(new UnityAction(clickedAction));
            newObj.SetText(text);
        }
    }
}
