using System;
using UnityEngine;

namespace ParkersUtils
{
    public static partial class RenderUtils
    {
        private static bool _clearInitialized = InitializeClear();
        private static ComputeShader _clearComputeShader;

        private static int KERNEL_CLEAR;

        private static bool InitializeClear()
        {
            _clearComputeShader = Resources.Load<ComputeShader>("ComputeShaders/Clear");
            if (_clearComputeShader == null)
            {
                Debug.LogError("Failed to load Splitter compute shader");
                return false;
            }

            KERNEL_CLEAR = _clearComputeShader.FindKernel("CS_Clear");
            return true;
        }

        public static void ClearRenderTexture(RenderTexture target)
        {
            int width = target.width;
            int height = target.height;
            _clearComputeShader.SetTexture(KERNEL_CLEAR, "_Target", target);
            _clearComputeShader.Dispatch(KERNEL_CLEAR, (int)Math.Ceiling((float)width / 8.0f), (int)Math.Ceiling((float)height / 8.0f), 1);
        }
    }
}
