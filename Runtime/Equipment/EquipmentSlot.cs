using System;
using UnityEngine;

namespace WizardUtils.Equipment
{
    [Serializable]
    public class EquipmentSlot
    {
        public EquipmentTypeDescriptor EquipmentType;
        public EquipmentDescriptor Equipment;
        public Transform PrefabInstantiationParent;
        public GameObject ObjectInstance;

        public EquipmentSlot(EquipmentTypeDescriptor equipmentType)
        {
            EquipmentType = equipmentType;
        }
    }
}
