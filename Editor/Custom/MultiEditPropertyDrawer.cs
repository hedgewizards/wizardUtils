using System;
using UnityEditor;
using UnityEngine;

namespace WizardUtils.Custom
{
    public abstract class MultiEditPropertyDrawer<TValue, TTarget>
        where TTarget : UnityEngine.Object
        where TValue : IEquatable<TValue>
    {
        private MultiEditProperty<TValue,TTarget> Property;
        private GUIContent Label;


        public MultiEditPropertyDrawer(
            string label,
            TTarget[] targets,
            Func<TTarget, TValue> get, Action<TTarget, TValue> set)
        {
            Label = new GUIContent(label);
            Property = new MultiEditProperty<TValue, TTarget>(targets, get, set);
        }

        public MultiEditPropertyDrawer(
            GUIContent label,
            TTarget[] targets,
            Func<TTarget, TValue> get, Action<TTarget, TValue> set)
        {
            Label = label;
            Property = new MultiEditProperty<TValue, TTarget>(targets, get, set);
        }

        public bool HasMixedValue() => Property.HasMixedValue();
        public TValue Read() => Property.Read();
        public void Write(TValue value) => Property.Write(value);

        public void Draw()
        {
            TValue initial = Property.Read();
            EditorGUI.showMixedValue = Property.HasMixedValue();
            TValue final = DrawField(Label, initial);
            EditorGUI.showMixedValue = false;

            if (!Equals(final,initial))
            {
                Property.Write(final);
            }
        }

        protected abstract TValue DrawField(GUIContent label, TValue value);
    }
}
