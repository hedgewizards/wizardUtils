using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WizardUtils.SceneManagement
{
    public static class SceneHelper
    {
        public static void DeactivateScene(Scene scene)
        {
            foreach(var gameObject in scene.GetRootGameObjects())
            {
                gameObject.SetActive(false);
            }
        }

        public static int FindSceneBuildIndex(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                if (sceneNameFromPath == sceneName)
                {
                    return i;
                }
            }

            throw new KeyNotFoundException($"Failed to find BuildIndex for scene named '{sceneName}'");
        }

        public static T FindComponentOnRootObjects<T>(Scene scene)
            where T : Component
        {
            if (!scene.IsValid() || !scene.isLoaded)
            {
                throw new InvalidOperationException($"Tried to inspect unloaded scene {scene}");
            }

            foreach(var root in scene.GetRootGameObjects())
            {
                if (root.TryGetComponent<T>(out T result))
                {
                    return result;
                }
            }

            return null;
        }
    }
}
