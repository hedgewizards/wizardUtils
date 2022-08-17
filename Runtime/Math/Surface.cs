using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WizardUtils.Math
{
    public struct Surface
    {
        public Vector3 position {get; private set;}
        public Vector3 normal { get; private set; }

        public Surface(Vector3 _position, Vector3 _normal)
        {
            position = _position;
            normal = _normal;
        }

        public Vector3 GetPosition()
        {
            return position;
        }
        public Vector3 GetNormal()
        {
            return normal;
        }
    }

}
