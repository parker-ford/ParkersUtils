using System;
using UnityEngine;

namespace ParkersUtils
{
    public static partial class RenderUtils
    {
        private static bool _renderUtilsInitialized = InitializeRenderUtils();
        private static ComputeShader _clearComputeShader;
        private static ComputeShader _blitComputeShader;
        private static ComputeShader _readPixelComputeShader;
        private static ComputeBuffer _pixelBuffer;

        private static int KERNEL_CLEAR;
        private static int KERNEL_COPY;
        private static int KERNEL_READ_PIXEL;

        private static bool InitializeRenderUtils()
        {
            _clearComputeShader = Resources.Load<ComputeShader>("ComputeShaders/Clear");
            if (_clearComputeShader == null)
            {
                Debug.LogError("Failed to load Clear compute shader");
                return false;
            }
            KERNEL_CLEAR = _clearComputeShader.FindKernel("CS_Clear");


            _blitComputeShader = Resources.Load<ComputeShader>("ComputeShaders/Blit");
            if (_blitComputeShader == null)
            {
                Debug.LogError("Failed to load Blit compute shader");
                return false;
            }
            KERNEL_COPY = _blitComputeShader.FindKernel("CS_Blit");

            _readPixelComputeShader = Resources.Load<ComputeShader>("ComputeShaders/ReadPixel");
            if (_readPixelComputeShader == null)
            {
                Debug.LogError("Failed to load Read Pixel compute shader");
                return false;
            }
            KERNEL_READ_PIXEL = _readPixelComputeShader.FindKernel("CS_ReadPixel");

            _pixelBuffer = new ComputeBuffer(1, sizeof(float) * 4);

            return true;
        }

        public static void ClearRenderTexture(RenderTexture target)
        {
            int width = target.width;
            int height = target.height;
            _clearComputeShader.SetTexture(KERNEL_CLEAR, "_Target", target);
            _clearComputeShader.Dispatch(KERNEL_CLEAR, (int)Math.Ceiling((float)width / 8.0f), (int)Math.Ceiling((float)height / 8.0f), 1);
        }

        public static void CopyRenderTexture(RenderTexture target, RenderTexture source)
        {
            int width = target.width;
            int height = target.height;
            _blitComputeShader.SetTexture(KERNEL_COPY, "_Target", target);
            _blitComputeShader.SetTexture(KERNEL_COPY, "_Source", source);
            _blitComputeShader.Dispatch(KERNEL_COPY, (int)Math.Ceiling((float)width / 8.0f), (int)Math.Ceiling((float)height / 8.0f), 1);

        }

        public static Color ReadRenderTexturePixel(RenderTexture source, int x, int y)
        {
            _readPixelComputeShader.SetBuffer(KERNEL_READ_PIXEL, "_PixelBuffer", _pixelBuffer);
            _readPixelComputeShader.SetTexture(KERNEL_READ_PIXEL, "_Source", source);
            _readPixelComputeShader.SetInt("_X", x);
            _readPixelComputeShader.SetInt("_Y", y);
            _readPixelComputeShader.SetInt("_Width", source.width);
            _readPixelComputeShader.SetInt("_Height", source.height);
            _readPixelComputeShader.Dispatch(KERNEL_READ_PIXEL, 1, 1, 1);

            float[] pixelArray = new float[4];
            _pixelBuffer.GetData(pixelArray);

            Color pixel = new Color(pixelArray[0], pixelArray[1], pixelArray[2], pixelArray[3]);
            return pixel;
        }
    }
}
