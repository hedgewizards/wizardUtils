using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Plastic.Newtonsoft.Json;

namespace PoseClipboard
{
    [CustomEditor(typeof(PoseClipboard))]
    public class PoseClipboardEditor : Editor
    {
        private PoseClipboard self;
        public override VisualElement CreateInspectorGUI()
        {
            self = target as PoseClipboard;
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Copy Pose"))
            {
                Copy();
            }
            
            if (GUILayout.Button("Paste Pose"))
            {
                Paste();
            }

            base.OnInspectorGUI();
        }

        public void Copy()
        {
            GUIUtility.systemCopyBuffer = JsonConvert.SerializeObject(GetNodeData());
        }

        public void Paste()
        {
            PoseNodeData nodeData = JsonConvert.DeserializeObject<PoseNodeData>(GUIUtility.systemCopyBuffer);
            SetNodeData(nodeData);
        }

        public PoseNodeData GetNodeData()
        {
            return transformToNode(self.transform);
        }

        private PoseNodeData transformToNode(Transform targetTransform)
        {
            List<PoseNodeData> children = new List<PoseNodeData>();
            for (int n = 0; n < targetTransform.childCount; n++)
            {
                Transform child = targetTransform.GetChild(n);
                children.Add(transformToNode(child));
            }

            return new PoseNodeData()
            {
                localPosition = targetTransform.localPosition,
                localRotation = targetTransform.eulerAngles,
                localScale = targetTransform.localScale,
                children = children.ToArray()
            };
        }

        public void SetNodeData(PoseNodeData data)
        {
            Undo.SetCurrentGroupName("Set Bones Pose");
            int undoId = Undo.GetCurrentGroup();
            RecursivelySetNodeData(self.transform, data);
            Undo.CollapseUndoOperations(undoId);
        }

        private void RecursivelySetNodeData(Transform targetTransform, PoseNodeData data)
        {
            Undo.RecordObject(targetTransform.gameObject, "Set Bone Pose");

            targetTransform.localPosition = data.localPosition;
            targetTransform.localScale = data.localScale;
            targetTransform.eulerAngles = data.localRotation;

            for (int n = 0; n < data.children.Length; n++)
            {
                RecursivelySetNodeData(targetTransform.GetChild(n), data.children[n]);
            }

            PrefabUtility.RecordPrefabInstancePropertyModifications(targetTransform.gameObject);
        }
    }
}
