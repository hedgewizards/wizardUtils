using Unity.Plastic.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoseClipboard
{
    public class FlatVector3Converter : JsonConverter<Vector3>
    {
        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            FlatVector3 value = serializer.Deserialize<FlatVector3>(reader);

            return value.ToVector3();
        }

        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, new FlatVector3(value));
        }

        private struct FlatVector3
        {
            public float x, y, z;

            public FlatVector3(Vector3 value) : this()
            {
                x = value.x;
                y = value.y;
                z = value.z;
            }

            public Vector3 ToVector3()
            {
                return new Vector3(x, y, z);
            }
        }
    }
}
