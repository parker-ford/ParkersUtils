

using UnityEngine;

namespace ParkersUtils
{
    public static class FourierTransformGPU
    {
        private static ComputeShader _computeShader;

        // private static readonly int KERNEL_SPLIT_RGB;
        private static readonly int KERNEL_FFT;
        private static readonly int KERNEL_DFT;
        // private static readonly int KERNEL_CENTER_SHIFT;
        // private static readonly int KERNEL_MAGNITUDE_RGB;
        // private static readonly int KERNEL_LOG_SCALE;
        // private static readonly int KERNEL_LINEAR_SCALE;

        static FourierTransformGPU()
        {
            _computeShader = Resources.Load<ComputeShader>("FourierTransform");

            KERNEL_FFT = _computeShader.FindKernel("CS_FFT");
            KERNEL_DFT = _computeShader.FindKernel("CS_DFT");

            // KERNEL_SPLIT_RGB = _computeShader.FindKernel("CS_SplitRGB");
            // KERNEL_CENTER_SHIFT = _computeShader.FindKernel("CS_ShiftToCenter");
            // KERNEL_MAGNITUDE_RGB = _computeShader.FindKernel("CS_RGBToMagnitude");
            // KERNEL_LOG_SCALE = _computeShader.FindKernel("CS_LogScale");
            // KERNEL_LINEAR_SCALE = _computeShader.FindKernel("CS_LinearScale");
        }

        private static void ClearComputeShaderVariables()
        {
            if (_computeShader != null)
            {
                _computeShader.SetBool("_Direction", false);
                _computeShader.SetBool("_Inverse", false);

                _computeShader.DisableKeyword("FFT_SIZE_16");
                _computeShader.DisableKeyword("FFT_SIZE_64");
                _computeShader.DisableKeyword("FFT_SIZE_128");
                _computeShader.DisableKeyword("FFT_SIZE_256");
                _computeShader.DisableKeyword("FFT_SIZE_512");
                _computeShader.DisableKeyword("FFT_SIZE_1024");
            }
        }

        private static bool ValidateTexture(Texture source)
        {
            if (source.width != source.height)
            {
                Debug.LogError("Source texture must be square");
                return false;
            }

            if ((source.width & (source.width - 1)) != 0)
            {
                Debug.LogError("Texture dimensions must be power of 2");
                return false;
            }

            return true;
        }

        private static bool SetTextureSize(int textureSize)
        {
            switch (textureSize)
            {
                case 16:
                    _computeShader.EnableKeyword("FFT_SIZE_16");
                    break;
                case 64:
                    _computeShader.EnableKeyword("FFT_SIZE_64");
                    break;
                case 128:
                    _computeShader.EnableKeyword("FFT_SIZE_128");
                    break;
                case 256:
                    _computeShader.EnableKeyword("FFT_SIZE_256");
                    break;
                case 512:
                    _computeShader.EnableKeyword("FFT_SIZE_512");
                    break;
                case 1024:
                    _computeShader.EnableKeyword("FFT_SIZE_1024");
                    break;
                default:
                    Debug.LogError("Unsupported texture size. Must be 16, 64, 128, 256, 512 or 1024.");
                    return false;
            }
            return true;
        }

        private static void ProcessFFT(RenderTexture target, bool direction, bool inverse)
        {
            int size = target.width;
            _computeShader.SetTexture(KERNEL_FFT, "_Target", target);
            _computeShader.SetBool("_Direction", direction);
            _computeShader.SetBool("_Inverse", inverse);
            _computeShader.Dispatch(KERNEL_FFT, 1, size, 1);
        }

        private static void ProcessDFT(RenderTexture target, bool direction, bool inverse)
        {
            int size = target.width;
            _computeShader.SetTexture(KERNEL_DFT, "_Target", target);
            _computeShader.SetBool("_Direction", direction);
            _computeShader.SetBool("_Inverse", inverse);
            _computeShader.Dispatch(KERNEL_DFT, 1, size, 1);
        }

        public static void FFT(RenderTexture target, bool inverse)
        {
            ClearComputeShaderVariables();

            if (!ValidateTexture(target)) return;

            int size = target.width;
            if (!SetTextureSize(size)) return;

            // Order of direction dependent on if inverse
            ProcessFFT(target, inverse ? false : true, inverse);
            ProcessFFT(target, inverse ? true : false, inverse);
        }

        public static void DFT(RenderTexture target, bool inverse)
        {
            ClearComputeShaderVariables();

            if (!ValidateTexture(target)) return;

            int size = target.width;
            if (!SetTextureSize(size)) return;

            // Order of direction dependent on if inverse
            ProcessDFT(target, inverse ? false : true, inverse);
            ProcessDFT(target, inverse ? true : false, inverse);
        }
    }
}

