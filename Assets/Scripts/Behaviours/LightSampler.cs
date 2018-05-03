using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSampler : MonoBehaviour {

    [SerializeField]
    private Vector2 _center;
    [SerializeField]
    private Vector2 _size = new Vector2(10, 10);
    [SerializeField]
    private bool _overrideCenter = true;
}
