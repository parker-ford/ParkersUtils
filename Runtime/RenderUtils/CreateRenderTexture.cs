using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParkersUtils
{
    public static partial class RenderUtils
    {

        public static RenderTextureDescriptor CreateDefaultRenderTextureDescriptor(int width, int height)
        {
            RenderTextureDescriptor rtDesc = new RenderTextureDescriptor();
            rtDesc.sRGB = false;
            rtDesc.dimension = UnityEngine.Rendering.TextureDimension.Tex2D;
            rtDesc.colorFormat = RenderTextureFormat.ARGBFloat;
            rtDesc.width = width;
            rtDesc.height = height;
            rtDesc.volumeDepth = 1;
            rtDesc.msaaSamples = 1;
            rtDesc.enableRandomWrite = true;
            return rtDesc;
        }

        public static RenderTexture CreateRenderTexture(RenderTextureDescriptor descriptor,
        TextureWrapMode wrapMode = TextureWrapMode.Repeat,
        FilterMode filterMode = FilterMode.Trilinear,
        int anisoLevel = 1)
        {
            RenderTexture rt = new RenderTexture(descriptor)
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
