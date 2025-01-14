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

        public static Texture2D CreateTexture2DFromRenderTexture(RenderTexture source)
        {
            var format = source.descriptor.colorFormat switch
            {
                RenderTextureFormat.ARGBFloat => TextureFormat.RGBAFloat,
                RenderTextureFormat.ARGBHalf => TextureFormat.RGBAHalf,
                RenderTextureFormat.ARGB32 => TextureFormat.RGBA32,
                _ => TextureFormat.RGBA32
            };

            var tex2D = new Texture2D(
                source.descriptor.width,
                source.descriptor.height,
                format,
                source.descriptor.msaaSamples > 1,
                source.descriptor.sRGB);

            tex2D.wrapMode = source.wrapMode;
            tex2D.filterMode = source.filterMode;
            tex2D.anisoLevel = source.anisoLevel;

            var prevActive = RenderTexture.active;
            RenderTexture.active = source;
            tex2D.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0);
            tex2D.Apply();
            RenderTexture.active = prevActive;

            return tex2D;
        }
    }
}
