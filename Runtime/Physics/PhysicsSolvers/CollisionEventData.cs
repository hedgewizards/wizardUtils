using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.PhysicsSolvers
{
    public class CollisionEventData
    {
        public Collider Other;
        public Vector3 ShapePosition;
        public Vector3 CollisionDirection;
        public float CollisionDistance;
    }
}
