using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WizardUtils.Audio;

[CustomEditor(typeof(AdvancedSoundEffect))]
public class AdvancedSoundEffectEditor : Editor
{
    private AdvancedSoundEffectTester CurrentTester;

    public void OnDisable()
    {
        if (CurrentTester != null)
        {
            CurrentTester.StopPlaying();
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        if (((AdvancedSoundEffect)target).AudioType == null)
        {
            EditorGUILayout.HelpBox("Select an Audio Type!", MessageType.Error);
        }
        else if (GUILayout.Button("Play"))
        {
            if (CurrentTester != null)
            {
                CurrentTester.StopPlaying();
            }
            else
            {
                GameObject playObject = new GameObject();
                playObject.hideFlags = HideFlags.HideAndDontSave;
                CurrentTester = playObject.AddComponent<AdvancedSoundEffectTester>();
            }

            CurrentTester.PlaySound((AdvancedSoundEffect)target);
        }
    }
}
