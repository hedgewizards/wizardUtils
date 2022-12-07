using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardUtils.Animations
{
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class SkinnedMeshRendererRootUpdater : MonoBehaviour
    {
        [ContextMenu("test")]
        public void Test()
        {
            SkinnedMeshRenderer renderer = GetComponent<SkinnedMeshRenderer>();
            foreach(var bone in renderer.bones)
            {
                Debug.Log($"Tap to select bone {bone.name}", bone);
            }
        }
        public void UpdateRoot(Transform newRoot)
        {
            int[] pathBuffer = new int[64];

            SkinnedMeshRenderer renderer = GetComponent<SkinnedMeshRenderer>();
            renderer.enabled = false;
            Transform oldRoot = renderer.rootBone;
            renderer.rootBone = newRoot;

            for (int boneIndex = 0; boneIndex < renderer.bones.Length; boneIndex++)
            {
                int count = GetHierarchyPathNoAlloc(pathBuffer, oldRoot, renderer.bones[boneIndex]);

                Transform newBone = TraverseHierarchyPath(pathBuffer, count, newRoot);

                //Debug.Log($"{boneIndex}: {renderer.bones[boneIndex].name} -> {newBone.name}\npath: {PathToString(pathBuffer, count)}");
                Transform oldBone = renderer.bones[boneIndex];
                renderer.bones[boneIndex] = newBone;
                if (renderer.bones[boneIndex] != newBone)
                {
                    Debug.LogError("Setting the bone literally did not work.");
                }

                if (boneIndex == 1)
                {
                    Debug.Log("Tap to select OLD bone", oldBone);
                    Debug.Log("Tap to select NEW bone", renderer.bones[boneIndex]);
                }
            }

            renderer.enabled = true;
        }

        Transform TraverseHierarchyPath(int[] pathBuffer, int pathCount, Transform rootBone)
        {
            Transform currentBone = rootBone;
            for (int n = pathCount - 1; n >= 0; n--)
            {
                currentBone = currentBone.GetChild(pathBuffer[n]);
            }
            return currentBone;
        }

        int GetHierarchyPathNoAlloc(int[] pathBuffer, Transform rootBone, Transform leafBone)
        {
            Transform currentBone = leafBone;
            int count = 0;
            while(currentBone != rootBone)
            {
                Transform parent = currentBone.parent;
                if (parent == null) throw new InvalidOperationException($"Provided leaf bone was not a child of the root. leaf: {leafBone.name}. root: {rootBone.name}");

                pathBuffer[count++] = GetChildIndex(parent, currentBone);
                currentBone = parent;
            }

            return count;
        }

        int GetChildIndex(Transform parent, Transform child)
        {
            for(int n = 0; n < parent.childCount; n++)
            {
                if (parent.GetChild(n) == child) return n;
            }
            return -1;
        }

        #region Debug
        private string PathToString(int[] pathBuffer, int count)
        {
            string txt = "";
            for (int n = count - 1; n >= 0; n--)
            {
                txt += pathBuffer[n] + " ";
            }

            return txt;
        }

        private string BoneHierarchyToString(Transform rootBone, Transform leafBone)
        {
            string str = "";

            Transform currentBone = leafBone;
            int count = 0;
            while (currentBone != rootBone)
            {
                Transform parent = currentBone.parent;
                if (parent == null) throw new InvalidOperationException($"Provided leaf bone was not a child of the root. leaf: {leafBone.name}. root: {rootBone.name}");

                str += parent.name + "\n";

                currentBone = parent;
            }

            return str;
        }
        #endregion
    }
}