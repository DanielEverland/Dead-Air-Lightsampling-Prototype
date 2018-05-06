using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LightSamplingManager : MonoBehaviour {

    public static event System.Action OnBeforeSample;
    public static event System.Action OnSample;
    public static event System.Action OnAfterSample;
    
    public static Texture2D DefaultSpotCookie { get { return _instance._defaultSpotCookie; } }
    public static IEnumerable<Light> AllLights { get { return new List<Light>(FindObjectsOfType<Light>()); } }

    private static List<Light> _allLights;
    private static LightSamplingManager _instance;

    /// <summary>
    /// Time in seconds between every sampling update
    /// </summary>
    public const float UPDATE_INTERVAL = 0.1f;

    [SerializeField]
    private Texture2D _defaultSpotCookie;
    [SerializeField]
    private bool _hasAssignedSpotCookie = false;

    private float _time;

    private void OnEnable()
    {
        _instance = this;
    }
    private void Awake()
    {
        _instance = this;
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

        OnBeforeSample?.Invoke();
    }
    private void Sample()
    {
        OnSample?.Invoke();
    }
    private void AfterSample()
    {
        OnAfterSample?.Invoke();
    }
    private void GetAllLightSources()
    {
        _allLights = new List<Light>(FindObjectsOfType<Light>());
    }
    [ContextMenu("Assign Built-In Cookie")]
    private void AssignDefaultSpotCookie()
    {
        _defaultSpotCookie = ((Texture2D)Resources.FindObjectsOfTypeAll(typeof(Texture)).FirstOrDefault(x => x.name == "Soft")).GetReadableTexture();
    }
}