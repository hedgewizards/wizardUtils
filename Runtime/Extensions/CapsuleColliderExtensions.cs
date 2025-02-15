using UnityEngine;

namespace WizardUtils.Extensions
{
    public static class CapsuleColliderExtensions
    {
        public static void SetPosition(this CapsuleCollider capsule, Vector3 top, Vector3 bottom)
        {
            Vector3 midPoint = (top + bottom) / 2f;
            Vector3 direction = (bottom - top).normalized;
            float distance = Vector3.Distance(top, bottom);

            capsule.transform.SetPositionAndRotation(midPoint, Quaternion.FromToRotation(Vector3.up, direction));

            // Set collider height
            capsule.height = distance + capsule.radius * 2;
        }
    }
}