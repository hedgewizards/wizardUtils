using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;

namespace WizardUtils.Custom
{
    public class MultiEditProperty<TValue,TTarget>
        where TValue : IEquatable<TValue>
        where TTarget : UnityEngine.Object
    {
        private Func<TTarget,TValue> Get;
        private Action<TTarget,TValue> Set;
        private TTarget[] Targets;

        public MultiEditProperty(TTarget[] targets, Func<TTarget, TValue> get, Action<TTarget, TValue> set)
        {
            Targets = targets;
            Get = get;
            Set = set;
        }

        public bool HasMixedValue()
        {
            TValue firstValue = Get(Targets[0]);
            for (int n = 1; n < Targets.Length; n++)
            {
                if (!Equals(Targets[1], firstValue))
                {
                    return true;
                }
            }

            return false;
        }

        public TValue Read() => HasMixedValue() ? default : Get(Targets[0]);

        public void Write(TValue value)
        {
            using var scope = new UndoScope("Write MultiProperty");

            foreach(var target in Targets)
            {
                Undo.RecordObject(target, "");
                Set(target, value);
                EditorUtility.SetDirty(target);
            }
        }
    }
}
