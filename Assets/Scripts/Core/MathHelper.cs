using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathHelper {

    public static bool IsWithinCone(Vector3 point, Vector3 coneStartPoint, Vector3 direction, float distance, float coneAngle)
    {
        Plane plane = new Plane(direction, coneStartPoint);
        Vector3 pointOnPlane = plane.ClosestPointOnPlane(point);
        Vector3 pointOnPlaneDelta = point - pointOnPlane;
        
        float distanceFromPlane = pointOnPlaneDelta.magnitude;
        float circleSizeAtDistance = distanceFromPlane * Mathf.Tan(Mathf.Deg2Rad * coneAngle / 2.0f);

        Vector3 pointAlongAxis = coneStartPoint + pointOnPlaneDelta.normalized * distanceFromPlane;
        
        float distanceToPointAlongAxis = Vector3.Distance(point, pointAlongAxis);

        return distanceToPointAlongAxis < circleSizeAtDistance;
    }
}
