using UnityEngine;

namespace ParkersUtils
{
    public static partial class RenderUtils
    {
        private static bool _colorCorrectionInitialized = InitializeColorCorrection();
        private static ComputeShader _gammaCorrectionShader;

        private static int KERNEL_GAMMA_CORRECTION;

        private static bool InitializeColorCorrection()
        {
            _gammaCorrectionShader = Resources.Load<ComputeShader>("ComputeShaders/GammaCorrect");
            if (_gammaCorrectionShader == null)
            {
                Debug.LogError("Failed to load Gamma Correction compute shader");
                return false;
            }

            KERNEL_GAMMA_CORRECTION = _gammaCorrectionShader.FindKernel("CS_GammaCorrect");

            return true;
        }
        public static void GammaCorrectRenderTexture(RenderTexture target)
        {
            int width = target.width;
            int height = target.height;

            _gammaCorrectionShader.SetTexture(KERNEL_GAMMA_CORRECTION, "_Target", target);
            _gammaCorrectionShader.Dispatch(KERNEL_GAMMA_CORRECTION, width / 8, height / 8, 1);
        }
    }
}
