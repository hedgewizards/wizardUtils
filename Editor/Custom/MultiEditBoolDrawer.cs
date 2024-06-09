using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace WizardUtils.Custom
{
    public class MultiEditBoolDrawer<TTarget> : MultiEditPropertyDrawer<bool, TTarget>
        where TTarget : UnityEngine.Object
    {
        public MultiEditBoolDrawer(string label, TTarget[] targets, Func<TTarget, bool> get, Action<TTarget, bool> set) : base(label, targets, get, set)
        {
        }

        protected override bool DrawField(GUIContent label, bool value)
        {
            return EditorGUILayout.Toggle(label, value);
        }
    }
}
