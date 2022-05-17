using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace WizardUtils
{
    public class KeystrokeChecker
    {
        public EditorWindow Window;

        public Event Check()
        {
            if (EditorWindow.focusedWindow != SceneView.currentDrawingSceneView
                && EditorWindow.focusedWindow != Window) return null;
            Event e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                return e;
            }
            else
            {
                return null;
            }
        }
    }
}
