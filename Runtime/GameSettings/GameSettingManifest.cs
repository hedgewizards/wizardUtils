using System;
using System.Collections.Generic;
using UnityEngine;
using WizardUtils.ManifestPattern;

namespace WizardUtils.GameSettings
{
    [CreateAssetMenu(fileName = "GameSettingManifest", menuName = "WizardUtils/GameSetting/Manifest", order = 0)]
    public class GameSettingManifest : ScriptableObject, IDescriptorManifest<GameSettingDescriptor>
    {
        public GameSettingDescriptor[] Descriptors;

        public ushort GetEntityTypeId(GameSettingDescriptor entity)
        {
            int index = Array.IndexOf(Descriptors, entity);
#if UNITY_EDITOR
            if (index == -1)
            {
                throw new KeyNotFoundException($"Entity {entity.name} not in Manifest {name}");
            }
#endif
            return (ushort)index;
        }

        public GameSettingDescriptor GetEntityTypeById(ushort id)
        {
            return Descriptors[id];
        }

        bool IDescriptorManifest<GameSettingDescriptor>.Contains(GameSettingDescriptor descriptor)
        {
            return Array.IndexOf(Descriptors, descriptor) != -1;
        }

        void IDescriptorManifest<GameSettingDescriptor>.Add(GameSettingDescriptor descriptor)
        {
            ArrayHelper.InsertAndResize(ref Descriptors, descriptor);
        }

        void IDescriptorManifest<GameSettingDescriptor>.Remove(GameSettingDescriptor descriptor)
        {
            ArrayHelper.DeleteAndResize(ref Descriptors, descriptor);
        }
    }
}