

using UnityEngine;

namespace ParkersUtils
{
    public static class RenderTextureExtensions
    {
        public static RenderTexture CopyData(this RenderTexture target, Texture2D source)
        {
            RenderTexture.active = target;
            Graphics.Blit(source, target, new Vector2(1.0f, -1.0f), new Vector2(0.0f, 1.0f)); // Prevents vertical flip
            RenderTexture.active = null;
            return target;
        }
    }
}
