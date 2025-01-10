using UnityEngine;

namespace ParkersUtils
{
    public static class FourierTransformGPU
    {
        private static ComputeShader _computeShader;

        private static readonly int KERNEL_FFT;
        private static readonly int KERNEL_DFT;
        private static readonly int KERNEL_INVERSE_SCALE;
        private static readonly int KERNEL_SYMMETRIC_SCALE;
        private static readonly int KERNEL_LOGARITHMIC_SCALE;
        private static readonly int KERNEL_FREQUENCY_SHIFT;
        private static readonly int KERNEL_CONVERT_TO_MAGNITUDE;
        private static readonly int KERNEL_CONVERT_TO_PHASE;

        static FourierTransformGPU()
        {
            _computeShader = Resources.Load<ComputeShader>("ComputeShaders/FourierTransform");

            KERNEL_FFT = _computeShader.FindKernel("CS_FFT");
            KERNEL_DFT = _computeShader.FindKernel("CS_DFT");
            KERNEL_INVERSE_SCALE = _computeShader.FindKernel("CS_InverseScale");
            KERNEL_SYMMETRIC_SCALE = _computeShader.FindKernel("CS_SymmetricScale");
            KERNEL_LOGARITHMIC_SCALE = _computeShader.FindKernel("CS_LogarithmicScale");
            KERNEL_FREQUENCY_SHIFT = _computeShader.FindKernel("CS_FrequencyShift");
            KERNEL_CONVERT_TO_MAGNITUDE = _computeShader.FindKernel("CS_ConvertToMagnitude");
            KERNEL_CONVERT_TO_PHASE = _computeShader.FindKernel("CS_ConvertToPhase");

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

        public static void InverseScale(RenderTexture target)
        {
            ClearComputeShaderVariables();

            if (!ValidateTexture(target)) return;

            int size = target.width;
            if (!SetTextureSize(size)) return;

            _computeShader.SetTexture(KERNEL_INVERSE_SCALE, "_Target", target);
            _computeShader.Dispatch(KERNEL_INVERSE_SCALE, size / 8, size / 8, 1);
        }

        public static void SymmetricScale(RenderTexture target)
        {
            ClearComputeShaderVariables();

            if (!ValidateTexture(target)) return;

            int size = target.width;
            if (!SetTextureSize(size)) return;

            _computeShader.SetTexture(KERNEL_SYMMETRIC_SCALE, "_Target", target);
            _computeShader.Dispatch(KERNEL_SYMMETRIC_SCALE, size / 8, size / 8, 1);
        }

        public static void LogarithmicScale(RenderTexture target)
        {
            ClearComputeShaderVariables();

            if (!ValidateTexture(target)) return;

            int size = target.width;
            if (!SetTextureSize(size)) return;

            _computeShader.SetTexture(KERNEL_LOGARITHMIC_SCALE, "_Target", target);
            _computeShader.Dispatch(KERNEL_LOGARITHMIC_SCALE, size / 8, size / 8, 1);
        }

        public static void FrequencyShift(RenderTexture target)
        {
            ClearComputeShaderVariables();

            if (!ValidateTexture(target)) return;

            int size = target.width;
            if (!SetTextureSize(size)) return;

            _computeShader.SetTexture(KERNEL_FREQUENCY_SHIFT, "_Target", target);
            _computeShader.Dispatch(KERNEL_FREQUENCY_SHIFT, size / 8, size / 8, 1);
        }

        public static void ConvertToMagnitude(RenderTexture target)
        {
            ClearComputeShaderVariables();

            if (!ValidateTexture(target)) return;

            int size = target.width;
            if (!SetTextureSize(size)) return;

            _computeShader.SetTexture(KERNEL_CONVERT_TO_MAGNITUDE, "_Target", target);
            _computeShader.Dispatch(KERNEL_CONVERT_TO_MAGNITUDE, size / 8, size / 8, 1);
        }

        public static void ConvertToPhase(RenderTexture target)
        {
            ClearComputeShaderVariables();

            if (!ValidateTexture(target)) return;

            int size = target.width;
            if (!SetTextureSize(size)) return;

            _computeShader.SetTexture(KERNEL_CONVERT_TO_PHASE, "_Target", target);
            _computeShader.Dispatch(KERNEL_CONVERT_TO_PHASE, size / 8, size / 8, 1);
        }
    }
}

