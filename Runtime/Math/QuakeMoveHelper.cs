using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.Math
{
    public static class QuakeMoveHelper
    {
        public static Vector3 AirAccelerate(
            Vector3 velocity,
            Vector3 direction,
            float maxSpeed,
            float accel,
            float deltaTime)
        {
            return Accelerate(velocity, direction, maxSpeed, accel * deltaTime);
        }

        public static Vector3 Accelerate(
            Vector3 velocity,
            Vector3 direction,
            float maxSpeed,
            float maxDeltaVelocity
            )
        {
            var currentSpeed = Vector3.Dot(velocity, direction);
            var addSpeed = maxSpeed - currentSpeed;

            if (addSpeed < 0) return velocity;

            var accelSpeed = Mathf.Min(maxDeltaVelocity, addSpeed);
            return velocity + accelSpeed * direction;
        }
    }
}
