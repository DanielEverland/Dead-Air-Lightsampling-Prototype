using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSamplingManager : MonoBehaviour {

    public static event System.Action OnBeforeSample;
    public static event System.Action OnSample;
    public static event System.Action OnAfterSample;

    /// <summary>
    /// Time in seconds between every sampling update
    /// </summary>
    public const float UPDATE_INTERVAL = 0.1f;

    private float _time;

    private void Update()
    {
        _time += Time.deltaTime;
        
        if(_time >= UPDATE_INTERVAL)
        {
            OnBeforeSample();
            OnSample();
            OnAfterSample();
        }
    }
}