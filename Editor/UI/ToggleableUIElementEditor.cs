using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WizardUtils.UI.Inspector
{
    [CustomEditor(typeof(ToggleableUIElement))]
    class ToggleableUIElementEditor : Editor
    {
        ToggleableUIElement self;

        public override void OnInspectorGUI()
        {
            self = target as ToggleableUIElement;
            DrawDefaultInspector();

            bool wasOpen = self.IsOpen;
            bool isOpenNow = EditorGUILayout.Toggle("Menu Open", wasOpen);
            if (wasOpen != isOpenNow)
            {
                self.SetOpen(isOpenNow);
                Undo.RecordObject(self, "Toggle Menu");
                PrefabUtility.RecordPrefabInstancePropertyModifications(self);
            }
        }
    }
}