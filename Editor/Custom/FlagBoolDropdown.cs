using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using WizardUtils.InspectorAttributes;

namespace WizardUtils.Custom
{
    public class FlagBoolDropdown
    {
        private readonly UnityEngine.Object _target;
        private readonly Dictionary<string, List<FieldInfo>> _channels = new();

        public FlagBoolDropdown(UnityEngine.Object target)
        {
            _target = target;

            var flags = target.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                .Where(f => f.FieldType == typeof(bool))
                .Select(f => (field: f, attr: f.GetCustomAttribute<FlagBoolAttribute>(true)))
                .Where(x => x.attr != null);

            foreach (var (field, attr) in flags)
            {
                if (!_channels.TryGetValue(attr.Channel, out var list))
                {
                    list = new List<FieldInfo>();
                    _channels[attr.Channel] = list;
                }
                list.Add(field);
            }
        }

        public void DrawChannelField(GUIContent label, string channel = "default")
        {
            if (!_channels.TryGetValue(channel, out var fields))
            {
                fields = new List<FieldInfo>();
            }

            // Build concatenated string of current "on" flags
            var activeNames = fields
                .Where(f => (bool)f.GetValue(_target))
                .Select(f => ObjectNames.NicifyVariableName(f.Name));

            string buttonText = string.Join(", ", activeNames);
            if (string.IsNullOrEmpty(buttonText))
                buttonText = "None";

            Rect rect = EditorGUILayout.GetControlRect();
            rect = EditorGUI.PrefixLabel(rect, label);

            if (EditorGUI.DropdownButton(rect, new GUIContent(buttonText), FocusType.Keyboard))
            {
                var menu = new GenericMenu();
                foreach (var f in fields)
                {
                    bool current = (bool)f.GetValue(_target);
                    string name = ObjectNames.NicifyVariableName(f.Name);

                    menu.AddItem(new GUIContent(name), current, () =>
                    {
                        Undo.RecordObject(_target, $"Toggle {f.Name}");
                        f.SetValue(_target, !current);
                        EditorUtility.SetDirty(_target);
                    });
                }
                menu.ShowAsContext();
            }
        }
    }
}
