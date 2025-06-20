using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.PhysicsSolvers
{
    public class RayProjectileCollisionEventData
    {
        public RaycastHit HitInfo;
        public Vector3 Position;
        public bool ShouldContinueMoving;
    }
}
