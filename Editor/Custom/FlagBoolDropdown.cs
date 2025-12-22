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
        private readonly Dictionary<string, List<DisplayableFlag>> Channels = new();

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
                    list = new List<DisplayableFlag>();
                    Channels[attr.Channel] = list;
                }
                list.Add(new DisplayableFlag()
                {
                    FieldInfo = field,
                    NiceName = attr.Title ?? ObjectNames.NicifyVariableName(field.Name)
                });
            }
        }

        public void DrawChannelField(GUIContent label, string channel = "default")
        {
            if (!Channels.TryGetValue(channel, out var flags))
            {
                flags = new List<DisplayableFlag>();
            }

            // Build concatenated string of current "on" flags
            var activeNames = flags
                .Where(f => (bool)f.FieldInfo.GetValue(Target))
                .Select(f => f.NiceName);

            string buttonText = string.Join(", ", activeNames);
            if (string.IsNullOrEmpty(buttonText))
                buttonText = "None";

            Rect rect = EditorGUILayout.GetControlRect();
            rect = EditorGUI.PrefixLabel(rect, label);

            if (EditorGUI.DropdownButton(rect, new GUIContent(buttonText), FocusType.Keyboard))
            {
                var menu = new GenericMenu();
                foreach (var f in flags)
                {
                    bool current = (bool)f.FieldInfo.GetValue(Target);
                    string name = f.NiceName;

                    menu.AddItem(new GUIContent(name), current, () =>
                    {
                        Undo.RecordObject(Target, $"Toggle {f.NiceName}");
                        f.FieldInfo.SetValue(Target, !current);
                        EditorUtility.SetDirty(Target);
                    });
                }
                menu.ShowAsContext();
            }
        }

        private struct DisplayableFlag
        {
            public FieldInfo FieldInfo;
            public string NiceName;
        }
    }
}
