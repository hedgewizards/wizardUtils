using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using WizardUtils.Audio;

namespace WizardUtils
{
    [CustomEditor(typeof(AudioController))]
    public class AudioControllerEditor : Editor
    {
        AudioController self;

        SerializedProperty m_Clips;

        private const string tooltip_ReplayDelay = "Will only emit a sound if the last sound it emitted was at least this long ago";
        SerializedProperty m_ReplayDelay;
        
        SerializedProperty m_RandomizePitch;
        SerializedProperty m_RandomizePitchMinimum;
        SerializedProperty m_RandomizePitchMaximum;
        

        public override VisualElement CreateInspectorGUI()
        {
            self = target as AudioController;

            m_Clips                 = serializedObject.FindProperty(nameof(AudioController.Clips));
            m_ReplayDelay           = serializedObject.FindProperty(nameof(AudioController.ReplayDelay));
            m_RandomizePitch        = serializedObject.FindProperty(nameof(AudioController.RandomizePitch));
            m_RandomizePitchMinimum = serializedObject.FindProperty(nameof(AudioController.RandomizePitchMaximum));
            m_RandomizePitchMaximum = serializedObject.FindProperty(nameof(AudioController.RandomizePitchMinimum));

            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_Clips);
            EditorGUILayout.PropertyField(m_ReplayDelay, new GUIContent("Minimum Replay Delay", tooltip_ReplayDelay));
            EditorGUILayout.PropertyField(m_RandomizePitch);
            if (self.RandomizePitch)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_RandomizePitchMinimum);
                EditorGUILayout.PropertyField(m_RandomizePitchMaximum);
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}