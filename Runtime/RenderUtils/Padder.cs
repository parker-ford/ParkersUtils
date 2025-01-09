using UnityEngine;

namespace ParkersUtils
{
    public static partial class RenderUtils
    {
        private static bool _padderInitialized = InitilazePadder();
        private static ComputeShader _padderComputeShader;

        private static int KERNEL_PADDER_BLIT;

        private static bool InitilazePadder()
        {
            _padderComputeShader = Resources.Load<ComputeShader>("ComputeShaders/Blit");
            if (_padderComputeShader == null)
            {
                Debug.LogError("Failed to load Padder Blit compute shader");
                return false;
            }

            KERNEL_PADDER_BLIT = _padderComputeShader.FindKernel("CS_Blit");

            return true;
        }

        public static RenderTexture CreatePaddedRenderTexture(RenderTexture source)
        {
            if (source.width == source.height)
            {
                return source;
            }

            int maxDimension = Mathf.Max(source.width, source.height);
            int targetSize = CommonUtils.NextPowerOfTwo(maxDimension);

            RenderTextureDescriptor desc = source.descriptor;
            desc.width = targetSize;
            desc.height = targetSize;
            RenderTexture paddedTexture = CreateRenderTexture(desc);

            _padderComputeShader.SetTexture(KERNEL_PADDER_BLIT, "_Source", source);
            _padderComputeShader.SetTexture(KERNEL_PADDER_BLIT, "_Target", paddedTexture);
            _padderComputeShader.Dispatch(KERNEL_PADDER_BLIT, (int)Mathf.Ceil(source.width / 8.0f), (int)Mathf.Ceil(source.height / 8.0f), 1);

            return paddedTexture;
        }

        public static void UnpadTexture(RenderTexture paddedTexture, RenderTexture target)
        {
            _padderComputeShader.SetTexture(KERNEL_PADDER_BLIT, "_Source", paddedTexture);
            _padderComputeShader.SetTexture(KERNEL_PADDER_BLIT, "_Target", target);
            _padderComputeShader.Dispatch(KERNEL_PADDER_BLIT, (int)Mathf.Ceil(target.width / 8.0f), (int)Mathf.Ceil(target.height / 8.0f), 1);
        }
    }
}
