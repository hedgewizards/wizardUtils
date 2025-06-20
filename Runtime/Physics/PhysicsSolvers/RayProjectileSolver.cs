using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.PhysicsSolvers
{
    public class RayProjectileSolver
    {
        private int LayerMask;
        private static Collider[] _OverlapCache;
        private static RaycastHit[] _RaycastCache;

        static RayProjectileSolver()
        {
            _OverlapCache = new Collider[64];
            _RaycastCache = new RaycastHit[64];
        }

        public RayProjectileSolver(int layerMask)
        {
            LayerMask = layerMask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="movement"></param>
        /// <param name="OnCollide">A function for the consumer to implement, set event data for special behavior</param>
        /// <returns></returns>
        public Vector3 Move(Vector3 position, Vector3 movement, Action<RayProjectileCollisionEventData> OnCollide)
        {
            static bool IgnoreCollider(List<KeyValuePair<Collider, int>> cachedColliders, Collider collider)
            {
                if (collider.gameObject.layer == 2)
                {
                    return false;
                }
                cachedColliders.Add(new KeyValuePair<Collider, int>(collider, collider.gameObject.layer));
                collider.gameObject.layer = 2; // this is the Ignore Raycast layer
                return true;
            }

            static void CleanUpColliders(List<KeyValuePair<Collider, int>> cachedColliders)
            {
                foreach(var kv in cachedColliders)
                {
                    kv.Key.gameObject.layer = kv.Value;
                }
            }

            RayProjectileCollisionEventData eventData = new RayProjectileCollisionEventData();

            bool badCollider = false;
            Vector3 direction = movement.normalized;
            float distance = movement.magnitude;
            List<KeyValuePair<Collider, int>> cachedColliders = new List<KeyValuePair<Collider, int>>();
            while(true)
            {
                if (!Raycast(
                    out RaycastHit hitInfo,
                    position,
                    direction,
                    distance,
                    LayerMask))
                {
                    position += direction * distance;
                    break;
                }

                eventData.HitInfo = hitInfo;
                eventData.Position = position + direction * hitInfo.distance;
                eventData.ShouldContinueMoving = false;

                OnCollide?.Invoke(eventData);

                if (eventData.ShouldContinueMoving)
                {
                    distance -= (position - eventData.Position).magnitude;
                    position = eventData.Position;

                    badCollider = !IgnoreCollider(cachedColliders, hitInfo.collider);
                    if (badCollider) break;
                    continue;
                }

                position = eventData.Position;
                break;
            }

            CleanUpColliders(cachedColliders);
            if (badCollider)
            {
                Debug.LogError($"{nameof(RayProjectileSolver)} failed to resolve Move. Collided with something on Unity reserved layer 2 (Ignore Raycast Layer)");
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
    }
}
