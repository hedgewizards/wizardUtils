using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace WizardUtils.InspectorAttributes
{
    [CustomPropertyDrawer(typeof(FilepathAttribute))]
    public class FilepathAttributeDrawer : PropertyDrawer
    {
        new FilepathAttribute attribute => (FilepathAttribute)base.attribute;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUI.HelpBox(position, "FilepathAttribute can only be used on string fields", MessageType.Error);
            }

            // Layout: property field + plus button
            float buttonWidth = 20f;
            Rect fieldRect = new Rect(position.x, position.y, position.width - buttonWidth - 2, position.height);
            Rect buttonRect = new Rect(position.x + position.width - buttonWidth, position.y, buttonWidth, position.height);


            if (!GUI.enabled)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }


            EditorGUI.PropertyField(fieldRect, property, label);

            if (GUI.Button(buttonRect, "⌕"))
            {
                string path;
                if (attribute.SelectionMode == FilepathAttribute.SelectionStyles.Files)
                {
                    path = EditorUtility.OpenFilePanel("Select File", "Assets", "");
                }
                else
                {
                    path = EditorUtility.OpenFolderPanel("Select Folder", "Assets", "");
                }

                if (!string.IsNullOrEmpty(path))
                {
                    if (attribute.Relative)
                    {
                        if (path.StartsWith(Application.dataPath))
                        {
                            property.stringValue = "Assets" + path.Substring(Application.dataPath.Length);
                        }
                        else
                        {
                            Debug.LogWarning($"FilepathAttribute: Failed to set Filepath for {property.name}. Only relative paths supported in this mode");
                        }
                    }
                    else
                    {
                        property.stringValue = path;
                    }
                    property.serializedObject.ApplyModifiedProperties();
                }

                // workaround to a weird UI bug with OpenFolderPanel. doesn't need to be here probably
                GUIUtility.ExitGUI();
            }
        }
    }
}
