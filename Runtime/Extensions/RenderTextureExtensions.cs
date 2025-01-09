

using UnityEngine;

namespace ParkersUtils
{
    public static class RenderTextureExtensions
    {
        public static RenderTexture FillData(this RenderTexture target, Texture2D source)
        {
            RenderTexture.active = target;
            Graphics.Blit(source, target);
            RenderTexture.active = null;
            return target;
        }
    }
}
