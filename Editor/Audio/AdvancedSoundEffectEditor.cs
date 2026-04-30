using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WizardUtils.Audio;

[CustomEditor(typeof(AdvancedSoundEffect))]
public class AdvancedSoundEffectEditor : Editor
{
    private static AdvancedSoundEffectTester CurrentTester;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        if (((AdvancedSoundEffect)target).AudioType == null)
        {
            EditorGUILayout.HelpBox("Select an Audio Type!", MessageType.Error);
            return;
        }
        
        if (GUILayout.Button("Play"))
        {
            if (CurrentTester != null)
            {
                CurrentTester.StopPlaying();
            }
            else
            {
                GameObject playObject = new GameObject();
                playObject.hideFlags = HideFlags.HideAndDontSave;
                CurrentTester = new AdvancedSoundEffectTester();
            }

            CurrentTester.PlaySound((AdvancedSoundEffect)target);
        }

        if (CurrentTester != null
            && CurrentTester.IsPlaying)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Stop"))
            {
                CurrentTester.StopPlaying(false);
            }
            else if (GUILayout.Button("Hard Stop"))
            {
                CurrentTester.StopPlaying();
            }
            GUILayout.EndHorizontal();

        }
    }
}
