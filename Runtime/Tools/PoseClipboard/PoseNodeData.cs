using Unity.Plastic.Newtonsoft.Json;
using UnityEngine;

namespace PoseClipboard
{
    [System.Serializable]
    public struct PoseNodeData
    {
        [JsonConverter(typeof(FlatVector3Converter))]
        public Vector3 localPosition, localScale, localRotation;
        public PoseNodeData[] children;
    }
}