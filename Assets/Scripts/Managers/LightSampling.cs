using System.Linq;
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

        return intensity;
    }
    private static float GetIntensityFromLight(Light light, Vector3 point)
    {
        switch (light.type)
        {
            case LightType.Point:
                return GetIntensityFromPointLight(light, point);
            default:
                throw new System.NotImplementedException();
        }
    }
    private static float GetIntensityFromPointLight(Light light, Vector3 point)
    {
        if (light.type != LightType.Point)
            throw new System.ArgumentException();

        if (!IsVisible(light, point))
            return 0;

        return QuadraticAttenuationFalloff(light.transform.position, point, light.range) * light.intensity;
    }
    private static bool IsVisible(Light light, Vector3 point)
    {
        return !Physics.Raycast(point, light.transform.position - point);
    }
    private static float QuadraticAttenuationFalloff(Vector3 a, Vector3 b, float range)
    {
        float distance = Vector3.Distance(a, b);
        
        return 1.0f / (1.0f + 25.0f * distance * distance);
    }
}
