﻿using System.Collections;
using UnityEngine;

namespace WizardUtils.Equipment
{
    [CreateAssetMenu(fileName = "eqgroup_", menuName = "WizardUtils/Equipment/EquipmentSlotManifest", order = 1)]
    public class EquipmentSlotManifest : ScriptableObject
    {
        public EquipmentTypeDescriptor[] Slots;
    }
}