using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using WizardUtils.InspectorAttributes;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Presets;
using System.Reflection;

[CustomPropertyDrawer(typeof(QuickCreateableAttribute))]
public class QuickCreateableDrawer : PropertyDrawer
{
    new QuickCreateableAttribute attribute => (QuickCreateableAttribute)base.attribute;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Layout: property field + plus button
        float buttonWidth = 20f;
        Rect fieldRect = new Rect(position.x, position.y, position.width - buttonWidth - 2, position.height);
        Rect buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, position.height);


        if (!GUI.enabled)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        Type fieldType = fieldInfo.FieldType;
        Type internalFieldType = fieldType;
        if (fieldType.IsArray)
        {
            internalFieldType = fieldType.GetElementType();
        }
        else
        {
            var enumerableType = fieldType.GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            if (enumerableType != null)
            {
                internalFieldType = enumerableType.GetGenericArguments()[0];
            }
        }

        Type targetType;
        if (attribute.AssetType != null)
        {
            if (!internalFieldType.IsAssignableFrom(attribute.AssetType))
            {
                Debug.LogError($"QuickCreateableAttribute: {attribute.AssetType.Name} is not assignable to field {fieldInfo.Name} in {property.serializedObject.targetObject.GetType()}");
                EditorGUI.PropertyField(position, property, label);
                return;
            }
            targetType = attribute.AssetType;
        }
        else
        {
            targetType = internalFieldType;
        }

        EditorGUI.PropertyField(fieldRect, property, label);

        if (GUI.Button(buttonRect, "+"))
        {
            if (attribute.ListSubclasses)
            {
                ShowTypeDropdown(targetType, property);
            }
            else if (attribute.ShowPresets)
            {
                ShowPresetsDropdown(targetType, property);
            }
            else
            {
                CreateAndAssignAsset(targetType, property);
            }
        }
    }

    private void ShowTypeDropdown(Type baseType, SerializedProperty property)
    {
        List<Type> types = TypeCache.GetTypesDerivedFrom(baseType)
            .Where(t => !t.IsAbstract && !t.IsGenericType)
            .ToList();

        if (!baseType.IsAbstract)
        {
            types.Add(baseType);
        }

        var menu = new GenericMenu();
        foreach (var type in types)
        {
            DisplayableType displayableType = new DisplayableType(type);
            if (attribute.ShowPresets)
            {
                var presets = FindPresetsForType(type);
                if (presets.Count > 0)
                {
                    menu.AddItem(new GUIContent($"{displayableType.Path}/Default Preset"), true, () =>
                    {
                        CreateAndAssignAsset(type, property);
                    });
                    foreach (var preset in presets)
                    {
                        menu.AddItem(new GUIContent($"{displayableType.Path}/{preset.name}"), false, () =>
                        {
                            CreateAndAssignAsset(type, property, preset);
                        });
                    }
                }
            }
            menu.AddItem(new GUIContent(displayableType.Path), false, () =>
            {
                CreateAndAssignAsset(type, property);
            });
        }
        menu.ShowAsContext();
    }

    private void ShowPresetsDropdown(Type baseType, SerializedProperty property)
    {
        var menu = new GenericMenu();
        var presets = FindPresetsForType(baseType);
        if (presets.Count == 0)
        {
            CreateAndAssignAsset(baseType, property);
            return;
        }

        menu.AddItem(new GUIContent($"Default Preset"), true, () =>
        {
            CreateAndAssignAsset(baseType, property);
        });

        foreach (var preset in presets)
        {
            menu.AddItem(new GUIContent($"Default Preset/{preset.name}"), false, () =>
            {
                CreateAndAssignAsset(baseType, property, preset);
            });
        }

        menu.ShowAsContext();
    }

    private void CreateAndAssignAsset(Type type, SerializedProperty property, Preset preset = null)
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
        if (preset != null)
        {
            preset.ApplyTo(newAsset);
        }
        else
        {
            var defaultPresets = Preset.GetDefaultPresetsForObject(newAsset);
            if (defaultPresets.Length > 0)
            {
                defaultPresets[0].ApplyTo(newAsset);
            }
        }
        AssetDatabase.CreateAsset(newAsset, fullPath);
        AssetDatabase.SaveAssets();
        property.objectReferenceValue = newAsset;
        EditorGUIUtility.PingObject(newAsset);
        property.serializedObject.ApplyModifiedProperties();
    }

    private static List<Preset> FindPresetsForType(Type type)
    {
        List<Preset> presets = new List<Preset>();
        var presetGuids = AssetDatabase.FindAssets("t:Preset");
        foreach (var presetGuid in presetGuids)
        {
            var preset = AssetDatabase.LoadAssetAtPath<Preset>(AssetDatabase.GUIDToAssetPath(presetGuid));
            if (preset != null && preset.GetTargetFullTypeName() == type.FullName)
            {
                presets.Add(preset);
            }
        }
        return presets;
    }

    private struct DisplayableType
    {
        public Type Type;
        public string Path;

        public DisplayableType(Type type) : this()
        {
            Type = type;
            var attribute = Type.GetCustomAttribute<QuickCreatableOrderAttribute>();
            if (attribute == null)
            {
                Path = ObjectNames.NicifyVariableName(type.Name);
            }
            else
            {
                string name = attribute.DisplayName ?? ObjectNames.NicifyVariableName(type.Name);
                if (!string.IsNullOrEmpty(attribute.Path))
                {
                    Path = $"{attribute.Path}/{name}";
                }
                else
                {
                    Path = name;
                }
            }
        }
    }
}
