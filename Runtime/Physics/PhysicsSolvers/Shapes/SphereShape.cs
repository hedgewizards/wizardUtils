using UnityEngine;
using WizardUtils.Extensions;

namespace WizardUtils.PhysicsSolvers.Shapes
{
    public struct SphereShape : IPhysicsSolverShape
    {
        private SphereCollider collider;

        public SphereShape(SphereCollider collider)
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

        public bool TestShape(
            Vector3 worldPosition,
            Quaternion orientation,
            int layermask = -1,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            return Physics.CheckSphere(
                worldPosition + collider.center,
                collider.radius,
                layermask,
                queryTriggerInteraction
                );
        }

        public Collider[] OverlapShape(
            Vector3 worldPosition,
            Quaternion orientation,
            int layermask = -1,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            return Physics.OverlapSphere(
                worldPosition + collider.center,
                collider.radius,
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
            return Physics.OverlapSphereNonAlloc(
                worldPosition + collider.center,
                collider.radius,
                results, 
                layermask,
                queryTriggerInteraction);
        }

        public int ShapeCastNonAlloc(
            RaycastHit[] results,
            Vector3 worldPosition,
            Vector3 direction,
            Quaternion orientation,
            float maxDistance,
            int layermask = -1,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            return Physics.SphereCastNonAlloc(
                worldPosition + collider.center,
                collider.radius,
                direction,
                results,
                maxDistance,
                layermask,
                queryTriggerInteraction);
        }

        public bool ShapeCastSingle(
            out RaycastHit hitInfo,
            Vector3 worldPosition,
            Vector3 direction,
            Quaternion orientation,
            float maxDistance,
            float scale = 1,
            int layermask = -1,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            return Physics.SphereCast(
                worldPosition + collider.center,
                collider.radius * scale,
                direction,
                out hitInfo,
                maxDistance,
                layermask,
                queryTriggerInteraction);
        }

        public void DebugDrawShape(Vector3 worldPosition, Quaternion orientation, float scale, Color color, float duration = 0)
        {
            DebugDrawHelper.DrawSphere(worldPosition + collider.center, collider.radius * scale, color, 8, duration);
        }
    }
}
