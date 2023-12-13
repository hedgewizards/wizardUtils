using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using WizardUtils.ManifestPattern;
using System.Linq;

namespace WizardUtils.GameSettings
{
    [CustomEditor(typeof(GameSettingDescriptor))]
    public class GameSettingDescriptorEditor : Editor
    {
        GameSettingDescriptor self;

        DescriptorManifestAssigner<GameSettingManifest, GameSettingDescriptor> dropdown;

        public override VisualElement CreateInspectorGUI()
        {
            self = target as GameSettingDescriptor;
            dropdown = new DescriptorManifestAssigner<GameSettingManifest, GameSettingDescriptor>();
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            // Draw the GUI unity generates normally
            DrawDefaultInspector();

            dropdown.DrawRegisterButtons(targets.Cast<GameSettingDescriptor>().ToArray());
        }
    }
}