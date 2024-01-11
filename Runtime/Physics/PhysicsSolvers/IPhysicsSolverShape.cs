using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.PhysicsSolvers
{
    public interface IPhysicsSolverShape
    {
        public Collider Collider { get; }
        public bool CanCollide(Collider other);

        public Collider[] OverlapShape(
            Vector3 worldPosition,
            Quaternion orientation,
            int layermask = ~0,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);

        public int OverlapShapeNonAlloc(
            Collider[] results,
            Vector3 worldPosition,
            Quaternion orientation,
            int layermask = ~0,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);

        public bool ComputePenetration(
            Vector3 worldPosition,
            Quaternion orientation,
            Collider other,
            Vector3 otherPosition,
            Quaternion otherOrientation,
            out Vector3 direction,
            out float distance);
    }
}
