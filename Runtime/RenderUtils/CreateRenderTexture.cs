using UnityEngine;

namespace ParkersUtils
{
    public static partial class RenderUtils
    {
        public static RenderTextureDescriptor CreateRenderTextureDescriptor(
            int width,
            int height,
            bool sRGB = false,
            UnityEngine.Rendering.TextureDimension dimension = UnityEngine.Rendering.TextureDimension.Tex2D,
            RenderTextureFormat colorFormat = RenderTextureFormat.ARGBFloat,
            int volumeDepth = 1,
            int msaaSamples = 1,
            bool enableRandomWrite = true)
        {
            return new RenderTextureDescriptor
            {
                sRGB = sRGB,
                dimension = dimension,
                colorFormat = colorFormat,
                width = width,
                height = height,
                volumeDepth = volumeDepth,
                msaaSamples = msaaSamples,
                enableRandomWrite = enableRandomWrite
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
