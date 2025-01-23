

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

        public static RenderTexture CopyData(this RenderTexture target, RenderTexture source)
        {
            RenderUtils.CopyRenderTexture(target, source);
            return target;
        }

        public static RenderTexture Clear(this RenderTexture target)
        {
            RenderUtils.ClearRenderTexture(target);
            return target;
        }

        public static Color ReadPixel(this RenderTexture target, int x, int y)
        {

            return RenderUtils.ReadRenderTexturePixel(target, x, y);
        }

        public static RenderTexture GammaCorrect(this RenderTexture target)
        {
            RenderUtils.GammaCorrectRenderTexture(target);
            return target;
        }
    }
}
