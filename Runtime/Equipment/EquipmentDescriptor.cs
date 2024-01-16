using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Equipment
{
    [CreateAssetMenu(fileName = "eq_", menuName = "WizardUtils/Equipment/EquipmentDescriptor", order = 1)]

    public class EquipmentDescriptor : ScriptableObject
    {
        public EquipmentTypeDescriptor SlotType;
        public GameObject Prefab;
    }
}
