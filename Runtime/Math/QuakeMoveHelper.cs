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

        /// <summary>
        /// move horizontal speed towards minimumSpeed
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="drag"></param>
        /// <returns></returns>
        public static Vector3 Decelerate(
            Vector3 velocity,
            float minimumSpeed,
            float deltaTime,
            float drag)
        {
            if (minimumSpeed > 0 && velocity.sqrMagnitude < minimumSpeed * minimumSpeed) return velocity;
            Vector3 desiredVelocity = velocity.Flatten().normalized * minimumSpeed;
            Vector3 result = Vector3.MoveTowards(velocity.Flatten(), desiredVelocity, drag * deltaTime);
            result.y = velocity.y;

            return result;
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

        public static Vector3 MoveVelocityTowards(
            Vector3 velocity,
            Vector3 direction,
            float maxSpeed,
            float maxDeltaVelocity
            )
        {
            return Vector3.MoveTowards(velocity, direction * maxSpeed, maxDeltaVelocity);
        }

        public static Vector3 MoveVelocityTowards(
            Vector3 velocity,
            Vector3 targetVelocity,
            float maxDeltaVelocity
            )
        {
            return Vector3.MoveTowards(velocity, targetVelocity, maxDeltaVelocity);
        }
    }
}
