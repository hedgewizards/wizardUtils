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

        /// <summary>
        /// Set scene enabled in build settings
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="enabled"></param>
        /// <returns>the buildId of the scene if enabled, otherwise -1</returns>
        public static int SetSceneEnabledInBuildSettings(SceneAsset asset, bool enabled)
        {
            string assetPath = AssetDatabase.GetAssetPath(asset);
            int sceneId = -1;

            List<EditorBuildSettingsScene> list = new List<EditorBuildSettingsScene>();
            list.AddRange(EditorBuildSettings.scenes);

            bool found = false;
            for (int i = 0; i < list.Count; i++)
            {
                EditorBuildSettingsScene scene = list[i];
                if (scene.enabled)
                {
                    sceneId++;
                }
                if (scene.path == assetPath)
                {
                    found = true;
                    scene.enabled = enabled;
                    break;
                }
            }

            if (!found && enabled)
            {
                sceneId = list.Count();
                list.Add(new EditorBuildSettingsScene(assetPath, enabled));
            }

            EditorBuildSettings.scenes = list.ToArray();
            return enabled ? sceneId : -1;
        }
    }
}
