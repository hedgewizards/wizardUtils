using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace WizardUtils.Prefabs
{
    public static class PrefabHelper
    {
        public static bool IsInstanceOfPrefab(GameObject instance, GameObject prefab)
        {
            if (instance == prefab) return true;

            // Check if the gameObject is an instance of the prefab
            if (PrefabUtility.GetPrefabInstanceHandle(instance) == prefab)
            {
                return true;
            }

            // Check if the gameObject is the prefab itself
            if (PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(instance) == AssetDatabase.GetAssetPath(prefab))
            {
                return true;
            }

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();

            // Check if the gameObject is the prefab open in the prefab editor
            if (prefabStage != null
                && prefabStage.mode == PrefabStage.Mode.InIsolation
                && prefabStage.assetPath == AssetDatabase.GetAssetPath(prefab)
                && prefabStage.prefabContentsRoot == instance)
            {
                return true;
            }

            return false;
        }
    }
}
