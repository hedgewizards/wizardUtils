using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.ManifestPattern
{
    public abstract class ManifestedDescriptor<T> : ManifestedDescriptor
        where T : ScriptableObject
    {
        public override Type GetManifestType()
        {
            return typeof(T);
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public abstract class ManifestedDescriptor : ScriptableObject
    {
        public abstract Type GetManifestType();

        public virtual string GetKey()
        {
            return name;
        }

        internal ManifestedDescriptor()
        {

        }
    }
}
