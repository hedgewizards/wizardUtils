using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace WizardUtils.PhysicsSolvers
{
    public class PhysicsSolver<TShape> where TShape : IPhysicsSolverShape
    {
        private TShape Shape;
        private int LayerMask;
        private Collider[] _OverlapCache;

        public PhysicsSolver(TShape shape, int layerMask, int overlapCacheSize = 16)
        {
            Shape = shape;
            LayerMask = layerMask;
            _OverlapCache = new Collider[overlapCacheSize];
        }

        public Vector3 Move(Vector3 position, Vector3 movement, Action<CollisionEventData> OnCollide)
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
                        Position = position,
                    };
                    OnCollide.Invoke(eventData);
                    position = eventData.Position;
                }
            }

            return position;
        }
    }
}
