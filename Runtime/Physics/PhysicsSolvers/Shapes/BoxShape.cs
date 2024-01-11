using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.PhysicsSolvers.Shapes
{
    public struct BoxShape : IPhysicsSolverShape
    {
        private BoxCollider collider;

        public BoxShape(BoxCollider collider)
        {
            this.collider = collider;
        }

        public Collider Collider => collider;

        public bool CanCollide(Collider other) => collider != other;

        public bool ComputePenetration(
            Vector3 worldPosition,
            Quaternion orientation,
            Collider other,
            Vector3 otherPosition,
            Quaternion otherOrientation,
            out Vector3 direction,
            out float distance)
        {
            return Physics.ComputePenetration(
                collider,
                worldPosition,
                orientation,
                other,
                otherPosition,
                otherOrientation,
                out direction,
                out distance);
        }

        public Collider[] OverlapShape(
            Vector3 worldPosition,
            Quaternion orientation,
            int layermask = -1,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            return Physics.OverlapBox(
                worldPosition + collider.center,
                collider.size / 2,
                orientation,
                layermask,
                queryTriggerInteraction);
        }

        public int OverlapShapeNonAlloc(
            Collider[] results,
            Vector3 worldPosition,
            Quaternion orientation,
            int layermask = -1,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            return Physics.OverlapBoxNonAlloc(
                worldPosition + collider.center,
                collider.size / 2,
                results, 
                orientation,
                layermask,
                queryTriggerInteraction);
        }
    }
}
