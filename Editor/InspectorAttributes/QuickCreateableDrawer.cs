using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using WizardUtils.InspectorAttributes;
using System.IO;

[CustomPropertyDrawer(typeof(QuickCreateableAttribute))]
public class QuickCreateableDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = (QuickCreateableAttribute)attribute;

        // Layout: property field + plus button
        float buttonWidth = 20f;
        Rect fieldRect = new Rect(position.x, position.y, position.width - buttonWidth - 2, position.height);
        Rect buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, position.height);


        if (GUI.enabled)
        {
            EditorGUI.PropertyField(fieldRect, property, label);

            if (GUI.Button(buttonRect, "+"))
            {
                if (attr.ListSubclasses)
                {
                    ShowTypeDropdown(attr.AssetType, property);
                }
                else
                {
                    CreateAndAssignAsset(attr.AssetType, property);
                }
            }
        }

        else
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    private void ShowTypeDropdown(Type baseType, SerializedProperty property)
    {
        var types = TypeCache.GetTypesDerivedFrom(baseType)
            .Where(t => !t.IsAbstract && !t.IsGenericType)
            .ToList();

        if (!baseType.IsAbstract)
        {
            types.Add(baseType);
        }

        var menu = new GenericMenu();
        foreach (var type in types)
        {
            menu.AddItem(new GUIContent(type.Name), false, () =>
            {
                CreateAndAssignAsset(type, property);
            });
        }
        menu.ShowAsContext();
    }

    private void CreateAndAssignAsset(Type type, SerializedProperty property)
    {
        var instance = ScriptableObject.CreateInstance(type);

        string parentPath;

        if (property.serializedObject.targetObject is MonoBehaviour monoBehaviour)
        {
            parentPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(monoBehaviour.gameObject);
        }
        else
        {
            parentPath = AssetDatabase.GetAssetPath(property.serializedObject.targetObject);
        }
        string fullPath;
        if (string.IsNullOrEmpty(parentPath))
        {
            fullPath = EditorUtility.SaveFilePanelInProject(
                "Create " + type.Name,
                $"New {type.Name}.asset",
                "asset",
                $"Enter a file name for the new {type.Name}"
            );
        }
        else
        {
            string folder = Path.GetDirectoryName(parentPath);

            string fileName = $"New {type.Name}.asset";
            fullPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(folder, fileName));
        }

        // Create and save the new asset
        var newAsset = ScriptableObject.CreateInstance(type);
        AssetDatabase.CreateAsset(newAsset, fullPath);
        AssetDatabase.SaveAssets();
        property.objectReferenceValue = newAsset;
        EditorGUIUtility.PingObject(newAsset);
        property.serializedObject.ApplyModifiedProperties();
    }
}
