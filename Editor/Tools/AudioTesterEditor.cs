using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace WizardUtils
{
    [CustomEditor(typeof(AudioTester))]
    public class AudioTesterEditor: Editor
    {
        AudioTester self;

        SerializedProperty m_Clips;

        private const string tooltip_ReplayDelay = "Will only emit a sound if the last sound it emitted was at least this long ago";
        SerializedProperty m_ReplayDelay;
        
        SerializedProperty m_RandomizePitch;
        SerializedProperty m_RandomizePitchMinimum;
        SerializedProperty m_RandomizePitchMaximum;
        

        public override VisualElement CreateInspectorGUI()
        {
            self = target as AudioTester;

            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Test"))
            {
                self.PlayNow();
            }
        }
    }
}