using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSamplingManager : MonoBehaviour {

    public static event System.Action OnBeforeSample;
    public static event System.Action OnSample;
    public static event System.Action OnAfterSample;

    public static IEnumerable<Light> AllLights { get { return _allLights; } }

    private static List<Light> _allLights;

    /// <summary>
    /// Time in seconds between every sampling update
    /// </summary>
    public const float UPDATE_INTERVAL = 0.1f;

    private float _time;

    private void Awake()
    {
        _allLights = new List<Light>();
    }
    private void Update()
    {
        _time += Time.deltaTime;
        
        if(_time >= UPDATE_INTERVAL)
        {
            Tick();
        }
    }
    private void Tick()
    {
        BeforeSample();
        Sample();
        AfterSample();
    }
    private void BeforeSample()
    {
        GetAllLightSources();

        OnBeforeSample();
    }
    private void Sample()
    {
        OnSample();
    }
    private void AfterSample()
    {
        OnAfterSample();
    }
    private void GetAllLightSources()
    {
        _allLights = new List<Light>(FindObjectsOfType<Light>());
    }
}