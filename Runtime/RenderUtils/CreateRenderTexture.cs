using UnityEngine;

namespace ParkersUtils
{
    public static partial class RenderUtils
    {
        public static RenderTextureDescriptor CreateDefaultRenderTextureDescriptor(int width, int height)
        {
            return new RenderTextureDescriptor
            {
                sRGB = false,
                dimension = UnityEngine.Rendering.TextureDimension.Tex2D,
                colorFormat = RenderTextureFormat.ARGBFloat,
                width = width,
                height = height,
                volumeDepth = 1,
                msaaSamples = 1,
                enableRandomWrite = true
            };
        }

        public static RenderTexture CreateRenderTexture(
            RenderTextureDescriptor descriptor,
            TextureWrapMode wrapMode = TextureWrapMode.Repeat,
            FilterMode filterMode = FilterMode.Trilinear,
            int anisoLevel = 1)
        {
            var rt = new RenderTexture(descriptor)
            {
                anisoLevel = filterMode == FilterMode.Trilinear ? anisoLevel : 0,
                wrapMode = wrapMode,
                filterMode = filterMode
            };
            rt.Create();
            return rt;
        }
    }
}
