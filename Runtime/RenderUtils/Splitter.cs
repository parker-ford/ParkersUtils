using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParkersUtils
{
    public static partial class RenderUtils
    {

        private static bool _splitterInitialized = InitilazeSplitter();
        private static ComputeShader _splitterComputeShader;

        private static int KERNEL_SPLIT_R;
        private static int KERNEL_SPLIT_G;
        private static int KERNEL_SPLIT_B;
        private static int KERNEL_SPLIT_RGB;

        private static bool InitilazeSplitter()
        {
            _splitterComputeShader = Resources.Load<ComputeShader>("ComputeShaders/Splitter");
            if (_splitterComputeShader == null)
            {
                Debug.LogError("Failed to load Splitter compute shader");
                return false;
            }

            KERNEL_SPLIT_R = _splitterComputeShader.FindKernel("CS_SplitR");
            KERNEL_SPLIT_G = _splitterComputeShader.FindKernel("CS_SplitG");
            KERNEL_SPLIT_B = _splitterComputeShader.FindKernel("CS_SplitB");
            KERNEL_SPLIT_RGB = _splitterComputeShader.FindKernel("CS_SplitRGB");

            return true;
        }

        public static void SplitRGB(RenderTexture source, RenderTexture red, RenderTexture green, RenderTexture blue)
        {
            int width = source.width;
            int height = source.height;
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_RGB, "_Source", source);
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_RGB, "_RedChannel", red);
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_RGB, "_GreenChannel", green);
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_RGB, "_BlueChannel", blue);
            _splitterComputeShader.Dispatch(KERNEL_SPLIT_RGB, width / 8, height / 8, 1);
        }

        public static void SplitR(RenderTexture source, RenderTexture red)
        {
            int width = source.width;
            int height = source.height;
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_R, "_Source", source);
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_R, "_RedChannel", red);
            _splitterComputeShader.Dispatch(KERNEL_SPLIT_R, width / 8, height / 8, 1);
        }

        public static void SplitG(RenderTexture source, RenderTexture green)
        {
            int width = source.width;
            int height = source.height;
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_G, "_Source", source);
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_G, "_GreenChannel", green);
            _splitterComputeShader.Dispatch(KERNEL_SPLIT_G, width / 8, height / 8, 1);
        }

        public static void SplitB(RenderTexture source, RenderTexture blue)
        {
            int width = source.width;
            int height = source.height;
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_B, "_Source", source);
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_B, "_BlueChannel", blue);
            _splitterComputeShader.Dispatch(KERNEL_SPLIT_B, width / 8, height / 8, 1);
        }
    }
}
