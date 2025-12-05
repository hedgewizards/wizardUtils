using UnityEngine;
using WizardUtils.Extensions;

namespace WizardUtils.PhysicsSolvers.Shapes
{
    public struct CapsuleShape : IPhysicsSolverShape
    {
        private CapsuleCollider collider;

        public CapsuleShape(CapsuleCollider collider)
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
            (var p1, var p2) = GetCapsulePoints(worldPosition, orientation);
            return Physics.CheckCapsule(
                p1,
                p2,
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
            (var p1, var p2) = GetCapsulePoints(worldPosition, orientation);
            return Physics.OverlapCapsule(
                p1,
                p2,
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
            (var p1, var p2) = GetCapsulePoints(worldPosition, orientation);
            return Physics.OverlapCapsuleNonAlloc(
                p1,
                p2,
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
            (var p1, var p2) = GetCapsulePoints(worldPosition, orientation);
            return Physics.CapsuleCastNonAlloc(
                p1,
                p2,
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
            (var p1, var p2) = GetCapsulePoints(worldPosition, orientation);
            return Physics.CapsuleCast(
                p1,
                p2,
                collider.radius * scale,
                direction,
                out hitInfo,
                maxDistance,
                layermask,
                queryTriggerInteraction);
        }

        private (Vector3 p1, Vector3 p2) GetCapsulePoints(Vector3 worldPosition, Quaternion orientation)
        {
            float offset = Mathf.Max(0, collider.height * 0.5f - collider.radius);
            Vector3 center = worldPosition + orientation * collider.center;
            Vector3 orientatedOffset = (orientation * Vector3.up) * offset;
            return (center + orientatedOffset, center - orientatedOffset);
        }

        public void DebugDrawShape(Vector3 worldPosition, Quaternion orientation, float scale, Color color, float duration = 0)
        {
            (var p1, var p2) = GetCapsulePoints(worldPosition, orientation);
            DebugDrawHelper.DrawCapsule(p1, p2, collider.radius * scale, color, 16, duration);
        }
    }
}
