using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.PhysicsSolvers
{
    public class PhysicsSolver
    {
        public IPhysicsSolverShape Shape {get; private set;}
        private int LayerMask;
        private static Collider[] _OverlapCache;

        static PhysicsSolver()
        {
            _OverlapCache = new Collider[64];
        }

        public PhysicsSolver(IPhysicsSolverShape shape, int layerMask)
        {
            Shape = shape;
            LayerMask = layerMask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="movement"></param>
        /// <param name="OnCollide">A function for the consumer to implement, if FALSE, the consumer handles changing position (including ignoring the collision)</param>
        /// <returns></returns>
        public Vector3 Move(Vector3 position, Vector3 movement, Func<CollisionEventData, bool> OnCollide)
        {
            CollisionEventData eventData = new CollisionEventData()
            {
                Position = position,
            };
            position += movement;

            int overlapCount = Shape.OverlapShapeNonAlloc(_OverlapCache, position, Quaternion.identity, LayerMask);

            for (int n = 0; n < overlapCount; n++)
            {
                Collider other = _OverlapCache[n];
                if (!Shape.CanCollide(other))
                {
                    continue;
                }

                if (Shape.ComputePenetration(position, Quaternion.identity, other, other.transform.position, other.transform.rotation, out Vector3 direction, out float distance))
                {
                    eventData.Other = other;
                    eventData.Position = position;
                    eventData.CollisionDirection = direction;
                    eventData.CollisionDistance = distance;

                    bool result = OnCollide.Invoke(eventData);
                    position = eventData.Position;
                    if (result)
                    {
                        position += direction * distance;
                    }
                }
            }

            return position;
        }

        /// <summary>
        /// Like <see cref="Shape"/>'s ShapeCastSingle, but ignores its own collider<br/>
        /// Relies on Layer 2 (Stock unity "Ignore Raycast") Being excluded from the layermask!
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool ShapeCastSingleIgnoreSelf(
            out RaycastHit hitInfo,
            Vector3 worldPosition,
            Vector3 direction,
            Quaternion orientation,
            float maxDistance,
            float scale = 1,
            int layermask = -1,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            var initialLayer = Shape.Collider.gameObject.layer;
            Shape.Collider.gameObject.layer = 2; // unity's stock Ignore Raycast layer

            bool result = Shape.ShapeCastSingle(
                out hitInfo,
                worldPosition,
                direction,
                orientation,
                maxDistance,
                scale,
                layermask,
                queryTriggerInteraction);

            Shape.Collider.gameObject.layer = initialLayer;
            return result;
        }

        public bool Raycast(
            out RaycastHit hitInfo,
            Vector3 position,
            Vector3 direction,
            float maxDistance,
            int layermask = -1,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
        {
            return Physics.Raycast(
                position,
                direction,
                out hitInfo,
                maxDistance,
                layermask,
                queryTriggerInteraction
                );
        }

        /// <summary>
        /// Returns true if it isn't overlapping anything at target position. Ignores its own collider
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool OverlapAny(Vector3 position)
        {
            int overlapCount = Shape.OverlapShapeNonAlloc(_OverlapCache, position, Quaternion.identity, LayerMask);
            if (overlapCount > 1)
            {
                return true;
            }

            return overlapCount == 1 && _OverlapCache[0] != Shape.Collider;
        }
    }
}
