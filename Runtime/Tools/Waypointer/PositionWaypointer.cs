﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils
{
    public class PositionWaypointer : Waypointer<Vector3>
    {
        Vector3 origin;

        public override void Awake()
        {
            origin = transform.position;
            base.Awake();
        }

        protected override Vector3 GetCurrentValue()
        {
            return transform.position - origin;
        }

        protected override void InterpolateAndApply(Vector3 startValue, Vector3 endValue, float i)
        {
            Vector3 newPositionFromOrigin = Vector3.Lerp(startValue, endValue, i);

            transform.position = origin + newPositionFromOrigin;
        }

        private void OnDrawGizmosSelected()
        {
            if (Waypoints == null) return;
            if (Application.isPlaying)
            {

                foreach (Vector3 worldOffset in Waypoints)
                {
                    Gizmos.DrawIcon(origin + worldOffset, "sp_flag.tiff", false, Color.blue);
                }
            }
            else
            {
                foreach (Vector3 worldOffset in Waypoints)
                {
                    Gizmos.DrawIcon(transform.position + worldOffset, "sp_flag.tiff", false, Color.blue);
                }
            }
        }
    }
}
