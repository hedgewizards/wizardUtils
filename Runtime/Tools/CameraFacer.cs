using System.Collections;
using UnityEngine;

namespace WizardUtils
{
    public class CameraFacer : MonoBehaviour
    {
        public Camera OverrideCamera;
        public bool YawOnly;

        Camera targetCamera => OverrideCamera != null ? OverrideCamera : Camera.main;

        private void LateUpdate()
        {
            if (targetCamera == null) return;
            if (YawOnly)
            {
                transform.rotation = Quaternion.LookRotation((targetCamera.transform.position - transform.position).Flatten(), Vector3.up);
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(targetCamera.transform.position - transform.position, Vector3.up);
            }
        }
    }
}