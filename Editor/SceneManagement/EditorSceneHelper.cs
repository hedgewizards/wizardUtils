using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace WizardUtils.SceneManagement
{
    public class EditorSceneHelper
    {
        public static bool IsSceneInBuildSettings(SceneAsset asset)
        {
            EditorBuildSettingsScene[] existingScenes = EditorBuildSettings.scenes;
            string assetpath = AssetDatabase.GetAssetPath(asset);

            foreach(var scene in existingScenes)
            {
                if (scene.path == assetpath)
                {
                    return scene.enabled;
                }
            }
            
            return false;
        }

        public static void SetSceneEnabledInBuildSettings(SceneAsset asset, bool enabled)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);

            List<EditorBuildSettingsScene> list = new List<EditorBuildSettingsScene>();
            list.AddRange(EditorBuildSettings.scenes);

            bool found = false;
            foreach(var scene in list)
            {
                if (scene.path == assetPath)
                {
                    found = true;
                    scene.enabled = enabled;
                }
            }

            if (!found)
            {
                list.Add(new EditorBuildSettingsScene(assetPath, enabled));
            }

            EditorBuildSettings.scenes = list.ToArray();
        }
    }
}
