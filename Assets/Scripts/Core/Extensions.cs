using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions {

    private static Dictionary<Texture2D, Texture2D> _cachedReadableTextures = new Dictionary<Texture2D, Texture2D>();

    public static Texture2D GetReadableTexture(this Texture2D nonReadable)
    {
        if (_cachedReadableTextures.ContainsKey(nonReadable))
            return _cachedReadableTextures[nonReadable];

        RenderTexture temporaryRenderTexture = RenderTexture.GetTemporary(nonReadable.width, nonReadable.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);

        Graphics.Blit(nonReadable, temporaryRenderTexture);

        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = temporaryRenderTexture;

        Texture2D newTexture = new Texture2D(nonReadable.width, nonReadable.height);
        newTexture.ReadPixels(new Rect(0, 0, temporaryRenderTexture.width, temporaryRenderTexture.height), 0, 0);
        newTexture.Apply();

        newTexture.name = nonReadable.name;

        RenderTexture.active = previous;

        RenderTexture.ReleaseTemporary(temporaryRenderTexture);

        _cachedReadableTextures.Add(nonReadable, newTexture);

        return newTexture;
    }
}