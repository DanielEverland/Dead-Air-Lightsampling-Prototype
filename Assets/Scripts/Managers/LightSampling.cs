﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LightSampling {

    public static float GetIntensity(Vector3 point)
    {
        float intensity = 0;

        foreach (Light light in LightSamplingManager.AllLights)
        {
            float currentIntensity = GetIntensityFromLight(light, point);
            
            if (currentIntensity > intensity)
            {
                intensity = currentIntensity;
            }
        }

        return intensity + GetAmbientIntensity();
    }
    private static float GetAmbientIntensity()
    {
        return RenderSettings.ambientLight.grayscale;
    }
    private static float GetIntensityFromLight(Light light, Vector3 point)
    {
        if (!IsVisible(light, point))
            return 0;

        switch (light.type)
        {
            case LightType.Point:
                return GetIntensityFromPointLight(light, point);
            case LightType.Directional:
                return GetIntensityFromDirectionalLight(light, point);
            case LightType.Spot:
                return GetIntensityFromSpotLight(light, point);
            default:
                throw new System.NotImplementedException();
        }
    }
    private static float GetIntensityFromSpotLight(Light light, Vector3 point)
    {
        if (light.type != LightType.Spot)
            throw new System.ArgumentException();

        float attenuation = QuadraticAttenuationFalloff(light.transform.position, point, light.range) * light.intensity;
        
        return attenuation * GetSpotlightFalloff(point, light);
    }
    private static float GetIntensityFromDirectionalLight(Light light, Vector3 point)
    {
        if (light.type != LightType.Directional)
            throw new System.ArgumentException();

        return light.intensity;
    }
    private static float GetIntensityFromPointLight(Light light, Vector3 point)
    {
        if (light.type != LightType.Point)
            throw new System.ArgumentException();
        
        return QuadraticAttenuationFalloff(light.transform.position, point, light.range) * light.intensity;
    }
    private static bool IsVisible(Light light, Vector3 point)
    {
        switch (light.type)
        {
            case LightType.Spot:
                return IsVisibleFromSpotlight(point, light);
            case LightType.Directional:
                return IsDirectionVisible(point, light.transform.forward);
            case LightType.Point:
                return IsPointVisible(light.transform.position, point);
            default:
                throw new System.NotImplementedException();
        }
    }
    private static bool IsVisibleFromSpotlight(Vector3 point, Light light)
    {
        if (!MathHelper.IsWithinCone(point, light.transform.position, light.transform.forward, light.range, light.spotAngle))
            return false;

        return IsDirectionVisible(point, light.transform.forward);
    }
    private static bool IsDirectionVisible(Vector3 from, Vector3 direction)
    {
        return !Physics.Raycast(from, -direction);
    }
    private static bool IsPointVisible(Vector3 from, Vector3 to)
    {
        Vector3 delta = from - to;

        return !Physics.Raycast(from, -delta.normalized, delta.magnitude);
    }
    private static float GetSpotlightFalloff(Vector3 point, Light light)
    {
        if(light.cookie != null)
        {
            return GetFalloffFromCookie(point, light, ((Texture2D)light.cookie).GetReadableTexture());
        }
        else if(LightSamplingManager.DefaultSpotCookie != null)
        {
            return GetFalloffFromCookie(point, light, LightSamplingManager.DefaultSpotCookie);
        }
        else
        {
            return ProceduralSpotCookie(point, light);
        }
    }
    private static float GetFalloffFromCookie(Vector3 point, Light light, Texture2D cookie)
    {
        Plane plane = new Plane(light.transform.forward, light.transform.position);
        Vector3 pointOnPlane = plane.ClosestPointOnPlane(point);
        Vector3 pointOnPlaneDelta = point - pointOnPlane;

        float distanceFromPlane = pointOnPlaneDelta.magnitude;
        float circleSizeAtDistance = distanceFromPlane * Mathf.Tan(Mathf.Deg2Rad * light.spotAngle / 2.0f);

        Vector3 pointAlongAxis = light.transform.position + pointOnPlaneDelta.normalized * distanceFromPlane;
        Vector3 farPointDelta = point - pointAlongAxis;

        float distanceToPointAlongAxis = Vector3.Distance(point, pointAlongAxis);
        float distanceInterpolator = Mathf.InverseLerp(0, circleSizeAtDistance, distanceToPointAlongAxis);

        Vector2Int halfSize = new Vector2Int((int)((float)cookie.width / 2), (int)((float)cookie.height / 2));

        Vector2Int textureIndex = new Vector2Int()
        {
            x = (int)(farPointDelta.normalized.x * distanceInterpolator * halfSize.x) + halfSize.x,
            y = (int)(farPointDelta.normalized.y * distanceInterpolator * halfSize.y) + halfSize.y,
        };

        Color color = cookie.GetPixel(textureIndex.x, textureIndex.y);
        
        return color.a;
    }
    private static float ProceduralSpotCookie(Vector3 point, Light light)
    {
        Plane plane = new Plane(light.transform.forward, light.transform.position);
        Vector3 pointOnPlane = plane.ClosestPointOnPlane(point);
        Vector3 pointOnPlaneDelta = point - pointOnPlane;

        float distanceFromPlane = pointOnPlaneDelta.magnitude;
        float circleSizeAtDistance = distanceFromPlane * Mathf.Tan(Mathf.Deg2Rad * light.spotAngle / 2.0f);

        Vector3 pointAlongAxis = light.transform.position + pointOnPlaneDelta.normalized * distanceFromPlane;

        float distanceToPointAlongAxis = Vector3.Distance(point, pointAlongAxis);
        
        float interpolator = 1 - Mathf.InverseLerp(0, circleSizeAtDistance * 0.9f, distanceToPointAlongAxis);
        interpolator = Mathf.Sqrt(interpolator) * 1.2f;
        
        return Mathf.Clamp(interpolator, 0, 1);
    }
    private static float QuadraticAttenuationFalloff(Vector3 a, Vector3 b, float range)
    {
        float distance = Vector3.Distance(a, b);
        float interpolant = Mathf.InverseLerp(0, range, distance);
        
        return 1.0f / (1.0f + 25.0f * interpolant * interpolant);
    }
}
