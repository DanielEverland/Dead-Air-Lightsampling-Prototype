using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[ExecuteInEditMode]
public class LightSamplingRenderer : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private float _pixelPerUnit = 1;
    [SerializeField]
    private Vector2 _size = Vector2.one;
    
    private void Update()
    {
        DoRender();
    }
    private void DoRender()
    {
        Texture2D texture = new Texture2D((int)(_size.x * _pixelPerUnit), (int)(_size.y * _pixelPerUnit), TextureFormat.RGBA32, false, true);
        Color[] color = new Color[texture.width * texture.height];

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                Vector3 worldPosition = transform.TransformPoint(GetLocalPosition(x, y, texture.width, texture.height));

                float intensity = LightSampling.GetIntensity(worldPosition);

                int index = y * texture.width + x;

                color[index] = new Color(intensity, intensity, intensity, 1);
            }
        }

        texture.SetPixels(color);
        texture.Apply();
        
        _spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), _pixelPerUnit);
    }
    private Vector2 GetLocalPosition(float x, float y, float width, float height)
    {
        float xInterpolant = Mathf.InverseLerp(0, width, x);
        float yInterpolant = Mathf.InverseLerp(0, height, y);

        return new Vector2(xInterpolant - 0.5f, yInterpolant - 0.5f);
    }
    private void OnValidate()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
}