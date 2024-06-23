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
        /// <param name="OnCollide">A function for the consumer to implement, if FALSE, ignore the collision!</param>
        /// <returns></returns>
        public Vector3 Move(Vector3 position, Vector3 movement, Func<CollisionEventData, bool> OnCollide)
        {
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
                    CollisionEventData eventData = new CollisionEventData()
                    {
                        ShapePosition = position,
                        CollisionDirection = direction,
                        CollisionDistance = distance,
                    };
                    bool result = OnCollide.Invoke(eventData);
                    if (result)
                    {
                        position += direction * distance;
                    }
                }
            }

            return position;
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

        public bool TestNoSelfOverlap(Vector3 position)
        {
            int overlapCount = Shape.OverlapShapeNonAlloc(_OverlapCache, position, Quaternion.identity, LayerMask);
            if (overlapCount > 1)
            {
                return true;
            }

            return _OverlapCache[0] != Shape.Collider;
        }
    }
}
