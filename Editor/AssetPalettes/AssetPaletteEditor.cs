using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace WizardUtils.AssetPalettes
{
    [CustomEditor(typeof(AssetPalette))]
    public class AssetPaletteEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Open Palette"))
            {
                var self = target as AssetPalette;
                var window = EditorWindow.GetWindow<AssetPaletteWindow>("Asset Palette");
                window.AddPalette(self, true);
                window.Show();
            }

            if (GUILayout.Button("Add Selection to Palette"))
            {
                var self = target as AssetPalette;
                Object[] selectedObjects = Selection.GetFiltered<Object>(SelectionMode.Assets);
                if (self.Entries == null) self.Entries = new AssetPaletteEntry[0];

                Undo.RecordObject(self, "Add Selection to Palette");
                var newArray = new AssetPaletteEntry[self.Entries.Length + selectedObjects.Length];
                System.Array.Copy(self.Entries, newArray, self.Entries.Length);
                for (int n = 0; n < selectedObjects.Length; n++)
                {
                    newArray[self.Entries.Length + n] = new AssetPaletteEntry()
                    {
                        Asset = selectedObjects[n],
                        DisplayName = selectedObjects[n].name,
                        Tooltip = selectedObjects[n].name
                    };
                }
                self.Entries = newArray;
                EditorUtility.SetDirty(self);
            }
        }
    }
}
