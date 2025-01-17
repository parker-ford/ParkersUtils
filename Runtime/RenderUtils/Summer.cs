using System;
using UnityEngine;

namespace ParkersUtils
{
    public static partial class RenderUtils
    {
        private static bool _summerInitialized = InitializeSummer();
        private static ComputeShader _summerComputeShader;
        private static int KERNEL_SUM_COLOR;

        private static Texture2D _tempTexture;
        private static RenderTexture _tempRenderTexture;
        private static readonly int MAX_SIZE = 2048;

        private static bool InitializeSummer()
        {
            _summerComputeShader = Resources.Load<ComputeShader>("ComputeShaders/Sum");
            if (_summerComputeShader == null)
            {
                Debug.LogError("Failed to load Summer compute shader");
                return false;
            }

            KERNEL_SUM_COLOR = _summerComputeShader.FindKernel("CS_SumColor");

            _tempTexture = new Texture2D(1, 1, TextureFormat.RGBAFloat, false);

            return true;
        }

        public static Color SumRenderTextureColor(RenderTexture target)
        {
            if (_tempRenderTexture == null)
            {
                RenderTextureDescriptor desc = CreateRenderTextureDescriptor(MAX_SIZE, MAX_SIZE, colorFormat: RenderTextureFormat.ARGBFloat);
                _tempRenderTexture = CreateRenderTexture(desc);
            }
            if (target.width > MAX_SIZE || target.height > MAX_SIZE)
            {
                Debug.LogWarning("Render Utils Warning: Sum target exceeds maximum size of: " + MAX_SIZE + ". Results will not be accurate");
            }
            _tempRenderTexture.CopyData(target);

            int level = 2;
            int span = 1;
            int width = target.width;
            int height = target.height;
            int limit = CommonUtils.NextPowerOfTwo(Mathf.Max(width, height));

            _summerComputeShader.SetTexture(KERNEL_SUM_COLOR, "_Target", _tempRenderTexture);

            while (level <= limit)
            {
                _summerComputeShader.SetInt("_Level", level);
                _summerComputeShader.SetInt("_Span", span);
                _summerComputeShader.SetInt("_Width", width);
                _summerComputeShader.SetInt("_Height", height);
                _summerComputeShader.Dispatch(KERNEL_SUM_COLOR, width / 8, height / 8, 1);

                level *= 2;
                span *= 2;
            }

            return _tempRenderTexture.ReadPixel(0, 0);
        }

    }
}
