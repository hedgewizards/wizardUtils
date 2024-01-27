using UnityEditor;
using UnityEngine;

namespace WizardUtils.Configurations
{
    [CustomPropertyDrawer(typeof(ExplicitConfigEntry))]
    public class ExplicitConfigEntryDrawer : EasyPropertyDrawer
    {
        protected override float lineCount => 1;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var keyProperty = property.FindPropertyRelative(nameof(ExplicitConfigEntry.Key));
            var valueProperty = property.FindPropertyRelative(nameof(ExplicitConfigEntry.Value));

            SplittablePropertyField[] propertyFields = new SplittablePropertyField[2]
            {
                new SplittablePropertyField()
                {
                    Property = keyProperty,
                    Label = GUIContent.none,
                    WidthFraction = 0.4f
                },
                new SplittablePropertyField()
                {
                    Property = valueProperty,
                    Label = GUIContent.none,
                    ExpandWidth = true,
                }
            };

            EasySplitProperty(position, propertyFields);

            EditorGUI.EndProperty();
        }
    }
}
