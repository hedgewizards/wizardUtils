using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WizardUtils.SceneManagement
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            float idWidth = 100f;
            float buttonWidth = 20f;

            var sceneAssetProperty = property.FindPropertyRelative(nameof(SceneReference.SceneAsset));
            var sceneIdProperty = property.FindPropertyRelative(nameof(SceneReference.SceneId));

            int sceneId = sceneIdProperty.intValue;
            var sceneAsset = sceneAssetProperty.boxedValue as SceneAsset;
            bool toggleShouldAdd;

            bool sceneAdded = sceneId != -1;
            if (sceneAdded)
            {
                if (sceneAsset == null)
                {
                    sceneAdded = false;
                    sceneIdProperty.intValue = -1;
                    sceneId = -1;
                }
                else
                {
                    string sceneAssetPath = AssetDatabase.GetAssetPath(sceneAsset);
                    string sceneIdPath = SceneUtility.GetScenePathByBuildIndex(sceneId);
                    if (sceneAssetPath != sceneIdPath)
                    {
                        // fix mismatched scene ID
                        sceneId = SceneUtility.GetBuildIndexByScenePath(sceneAssetPath);
                        sceneIdProperty.intValue = sceneId;
                        sceneAdded = sceneId != -1;
                    }
                }
            }

            (Rect sceneAssetRect, Rect idAndButtonRect) = RectExtensions.CutRectHorizontallyAbsoluteRight(position, idWidth + buttonWidth + 2);
            (Rect idRect, Rect buttonRect) = RectExtensions.CutRectHorizontallyAbsoluteRight(idAndButtonRect, buttonWidth);

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                EditorGUI.PropertyField(sceneAssetRect, sceneAssetProperty, label);

                if (check.changed)
                {
                    if (sceneAsset == null)
                    {
                        sceneAdded = false;
                        sceneIdProperty.intValue = -1;
                        sceneId = -1;
                    }
                    else
                    {
                        string sceneAssetPath = AssetDatabase.GetAssetPath(sceneAsset);
                        string sceneIdPath = SceneUtility.GetScenePathByBuildIndex(sceneId);
                        if (sceneAssetPath != sceneIdPath)
                        {
                            // fix mismatched scene ID
                            sceneId = SceneUtility.GetBuildIndexByScenePath(sceneAssetPath);
                            sceneIdProperty.intValue = sceneId;
                            sceneAdded = sceneId != -1;
                        }
                    }
                }
            }


            using (new EditorGUI.DisabledScope(true))
            {
                if (sceneId == -1)
                {
                    EditorGUI.TextField(idRect, "UNSET");
                }
                else
                {
                    EditorGUI.TextField(idRect, sceneId.ToString());
                }
            }

            using (new EditorGUI.DisabledScope(sceneAsset == null))
            {
                toggleShouldAdd = GUI.Toggle(buttonRect, sceneAdded, GUIContent.none);
            }
            if (toggleShouldAdd && !sceneAdded)
            {
                sceneIdProperty.intValue = EditorSceneHelper.SetSceneEnabledInBuildSettings(sceneAsset, true);
            }
            else if (sceneAdded && !toggleShouldAdd)
            {
                sceneIdProperty.intValue = EditorSceneHelper.SetSceneEnabledInBuildSettings(sceneAsset, false);
            }

            if (sceneAsset != null && sceneId == -1)
            {
                DrawBorder(idRect, Color.yellow);
                DrawBorder(buttonRect, Color.yellow);
            }

            EditorGUI.EndProperty();
        }
        private static void DrawBorder(Rect rect, Color color)
        {
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - 1, rect.width, 1), color);
            EditorGUI.DrawRect(new Rect(rect.x, rect.y, 1, rect.height), color);
            EditorGUI.DrawRect(new Rect(rect.xMax - 1, rect.y, 1, rect.height), color);
        }
    }
}
