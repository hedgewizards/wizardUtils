using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace WizardUtils.Animation
{
    public class AnimatorStringToHashWindow : EditorWindow
    {
        private string InputString = "";
        private int Hash;

        [MenuItem("Window/Animation/StringToHash")]
        public static void ShowWindow()
        {
            var window = GetWindow<AnimatorStringToHashWindow>("String To Hash");
            window.Show();
        }

        private void OnGUI()
        {
            string newString = EditorGUILayout.TextField("Input String", InputString);
            if (newString != InputString)
            {
                InputString = newString;
                if (!string.IsNullOrEmpty(InputString))
                {
                    Hash = Animator.StringToHash(newString);
                }
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("Hash", Hash);
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Copy to Clipboard"))
            {
                GUIUtility.systemCopyBuffer = Hash.ToString();
            }
        }
    }
}
