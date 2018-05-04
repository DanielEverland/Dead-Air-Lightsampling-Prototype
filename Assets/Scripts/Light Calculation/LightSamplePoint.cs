using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSamplePoint {

    public Vector3 Point { get; set; }
    public float Intensity { get; private set; }

    public void Sample()
    {
        Intensity = 0;

        foreach (Light light in LightSamplingManager.AllLights)
        {
            float currentIntensity = GetIntensityFromLight(light);

            if(currentIntensity > Intensity)
            {
                Intensity = currentIntensity;
            }
        }
    }
    private float GetIntensityFromLight(Light light)
    {
        switch (light.type)
        {
            case LightType.Spot:
                return GetIntensityFromPointLight(light);
            default:
                throw new System.NotImplementedException();
        }
    }
    private float GetIntensityFromPointLight(Light light)
    {
        if (light.type != LightType.Point)
            throw new System.ArgumentException();

        if (!IsVisible(light))
            return 0;

        return QuadraticAttenuationFalloff(light.transform.position, Point, light.range) * light.intensity;
    }
    private bool IsVisible(Light light)
    {
        return Physics.Raycast(Point, light.transform.position - Point);
    }
    private float QuadraticAttenuationFalloff(Vector2 a, Vector2 b, float range)
    {
        float distance = Vector2.Distance(a, b);
        
        return 1.0f / (1.0f + 25.0f * distance * distance);
    }
}
