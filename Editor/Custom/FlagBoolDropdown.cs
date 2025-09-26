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
        private readonly UnityEngine.Object Target;
        private readonly Dictionary<string, List<FieldInfo>> Channels = new();

        public FlagBoolDropdown(UnityEngine.Object target)
        {
            Target = target;

            var flags = target.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                .Where(f => f.FieldType == typeof(bool))
                .Select(f => (field: f, attr: f.GetCustomAttribute<FlagBoolAttribute>(true)))
                .Where(x => x.attr != null);

            foreach (var (field, attr) in flags)
            {
                if (!Channels.TryGetValue(attr.Channel, out var list))
                {
                    list = new List<FieldInfo>();
                    Channels[attr.Channel] = list;
                }
                list.Add(field);
            }
        }

        public void DrawChannelField(GUIContent label, string channel = "default")
        {
            if (!Channels.TryGetValue(channel, out var fields))
            {
                fields = new List<FieldInfo>();
            }

            // Build concatenated string of current "on" flags
            var activeNames = fields
                .Where(f => (bool)f.GetValue(Target))
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
                    bool current = (bool)f.GetValue(Target);
                    string name = ObjectNames.NicifyVariableName(f.Name);

                    menu.AddItem(new GUIContent(name), current, () =>
                    {
                        Undo.RecordObject(Target, $"Toggle {f.Name}");
                        f.SetValue(Target, !current);
                        EditorUtility.SetDirty(Target);
                    });
                }
                menu.ShowAsContext();
            }
        }
    }
}
