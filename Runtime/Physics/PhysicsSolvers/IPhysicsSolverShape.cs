﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WizardUtils.PhysicsSolvers
{
    public interface IPhysicsSolverShape
    {
        public Collider Collider { get; }
        public bool CanCollide(Collider other);
        public bool ComputePenetration(
            Vector3 worldPosition,
            Quaternion orientation,
            Collider other,
            Vector3 otherPosition,
            Quaternion otherOrientation,
            out Vector3 direction,
            out float distance);

        public Collider[] OverlapShape(
            Vector3 worldPosition,
            Quaternion orientation,
            int layermask = ~0,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);

        public int OverlapShapeNonAlloc(
            Collider[] results,
            Vector3 worldPosition,
            Quaternion orientation,
            int layermask = ~0,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);

        public int ShapeCastNonAlloc(
            RaycastHit[] results,
            Vector3 worldPosition,
            Vector3 direction,
            Quaternion orientation,
            float maxDistance,
            int layermask = ~0,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);

        public bool ShapeCastSingle(
            out RaycastHit hitInfo,
            Vector3 worldPosition,
            Vector3 direction,
            Quaternion orientation,
            float maxDistance,
            float scale = 1f,
            int layermask = ~0,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);

        public bool TestShape(
            Vector3 worldPosition,
            Quaternion orientation,
            int layermask = ~0,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal);

        public void DebugDrawShape(
            Vector3 worldPosition,
            Quaternion orientation,
            float scale,
            Color color,
            float duration = 0);
    }
}
