using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ParkersUtils
{
    public static partial class RenderUtils
    {

        private static bool _splitterInitialized = InitilazeSplitter();
        private static ComputeShader _splitterComputeShader;

        public enum ColorChannel { R = 0, G = 1, B = 2, A = 3}

        private static int KERNEL_SPLIT_R;
        private static int KERNEL_SPLIT_G;
        private static int KERNEL_SPLIT_B;
        private static int KERNEL_SPLIT_RGB;
        private static int KERNEL_SINGLE_CHANNEL;
        private static int KERNEL_DOUBLE_CHANNEL;
        private static int KERNEL_TRIPLE_CHANNEL;
        private static int KERNEL_QUAD_CHANNEL;

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
            KERNEL_SINGLE_CHANNEL = _splitterComputeShader.FindKernel("CS_SplitSingleChannel");
            KERNEL_DOUBLE_CHANNEL = _splitterComputeShader.FindKernel("CS_SplitDoubleChannel");
            KERNEL_TRIPLE_CHANNEL = _splitterComputeShader.FindKernel("CS_SplitTripleChannel");
            KERNEL_QUAD_CHANNEL = _splitterComputeShader.FindKernel("CS_SplitQuadChannel");

            return true;
        }

        public static void SplitChannels(RenderTexture src, IReadOnlyList<ColorChannel> channels, IReadOnlyList<RenderTexture> outputs)
        {
            if(outputs == null || outputs.Count == 0 || channels == null || channels.Count == 0 || channels.Count > 4)
            {
                return;
            }

            var tempChannels = new List<ColorChannel>(channels);
            for (int i = 0; i < outputs.Count; i++)
            {
                if (tempChannels.Count == 0)
                {
                    continue;
                }

                var output = outputs[i];

                if (i == outputs.Count - 1)
                {
                    if (tempChannels.Count == 1)
                    {
                        SplitSingleChannel(src, output, tempChannels[0]);
                    }
                    else if (tempChannels.Count == 2)
                    {
                        SplitDoubleChannel(src, output, tempChannels[0], tempChannels[1]);
                    }
                    else if (tempChannels.Count == 3)
                    {
                        SplitTripleChannel(src, output, tempChannels[0], tempChannels[1], tempChannels[2]);
                    }
                    else if (tempChannels.Count == 4)
                    {
                        SplitQuadChannel(src, output, tempChannels[0], tempChannels[1], tempChannels[2], tempChannels[3]);
                    }

                        tempChannels.Clear();
                }
                else
                {
                    ColorChannel channel = tempChannels[0];
                    tempChannels.RemoveAt(0);
                    SplitSingleChannel(src, output, channel);
                }


            }
        }

        public static void SplitSingleChannel(RenderTexture src, RenderTexture target, ColorChannel channel)
        {
            int width = target.width;
            int height = target.height;
            _splitterComputeShader.SetFloat("_Width", width);
            _splitterComputeShader.SetFloat("_Height", height);
            _splitterComputeShader.SetInt("_Channel0", (int)channel);
            _splitterComputeShader.SetTexture(KERNEL_SINGLE_CHANNEL, "_Source", src);
            _splitterComputeShader.SetTexture(KERNEL_SINGLE_CHANNEL, "_Target", target);
            //_splitterComputeShader.Dispatch(KERNEL_SINGLE_CHANNEL, GetDispatchSize(width, 8), GetDispatchSize(height, 8), 1);
            _splitterComputeShader.Dispatch(KERNEL_SINGLE_CHANNEL, width, height, 1);
        }

        public static void SplitDoubleChannel(RenderTexture src, RenderTexture target, ColorChannel channel0, ColorChannel channel1)
        {
            int width = target.width;
            int height = target.height;
            _splitterComputeShader.SetFloat("_Width", width);
            _splitterComputeShader.SetFloat("_Height", height);
            _splitterComputeShader.SetInt("_Channel0", (int)channel0);
            _splitterComputeShader.SetInt("_Channel1", (int)channel1);
            _splitterComputeShader.SetTexture(KERNEL_DOUBLE_CHANNEL, "_Source", src);
            _splitterComputeShader.SetTexture(KERNEL_DOUBLE_CHANNEL, "_Target", target);
            _splitterComputeShader.Dispatch(KERNEL_DOUBLE_CHANNEL, width / 8, height / 8, 1);
        }

        public static void SplitTripleChannel(RenderTexture src, RenderTexture target, ColorChannel channel0, ColorChannel channel1, ColorChannel channel2)
        {
            int width = target.width;
            int height = target.height;
            _splitterComputeShader.SetFloat("_Width", width);
            _splitterComputeShader.SetFloat("_Height", height);
            _splitterComputeShader.SetInt("_Channel0", (int)channel0);
            _splitterComputeShader.SetInt("_Channel1", (int)channel1);
            _splitterComputeShader.SetInt("_Channel2", (int)channel2);
            _splitterComputeShader.SetTexture(KERNEL_TRIPLE_CHANNEL, "_Source", src);
            _splitterComputeShader.SetTexture(KERNEL_TRIPLE_CHANNEL, "_Target", target);
            _splitterComputeShader.Dispatch(KERNEL_TRIPLE_CHANNEL, width / 8, height / 8, 1);
        }

        public static void SplitQuadChannel(RenderTexture src, RenderTexture target, ColorChannel channel0, ColorChannel channel1, ColorChannel channel2, ColorChannel channel3)
        {
            int width = target.width;
            int height = target.height;
            _splitterComputeShader.SetFloat("_Width", width);
            _splitterComputeShader.SetFloat("_Height", height);
            _splitterComputeShader.SetInt("_Channel0", (int)channel0);
            _splitterComputeShader.SetInt("_Channel1", (int)channel1);
            _splitterComputeShader.SetInt("_Channel2", (int)channel2);
            _splitterComputeShader.SetInt("_Channel3", (int)channel3);
            _splitterComputeShader.SetTexture(KERNEL_QUAD_CHANNEL, "_Source", src);
            _splitterComputeShader.SetTexture(KERNEL_QUAD_CHANNEL, "_Target", target);
            _splitterComputeShader.Dispatch(KERNEL_QUAD_CHANNEL, width / 8, height / 8, 1);
        }

        public static void SplitRGB(Texture source, RenderTexture red, RenderTexture green, RenderTexture blue)
        {
            int width = red.width;
            int height = red.height;
            _splitterComputeShader.SetFloat("_Width", width);
            _splitterComputeShader.SetFloat("_Height", height);

            _splitterComputeShader.SetTexture(KERNEL_SPLIT_RGB, "_Source", source);
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_RGB, "_RedChannel", red);
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_RGB, "_GreenChannel", green);
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_RGB, "_BlueChannel", blue);
            _splitterComputeShader.Dispatch(KERNEL_SPLIT_RGB, width / 8, height / 8, 1);
        }

        public static void SplitR(Texture source, RenderTexture red)
        {
            int width = red.width;
            int height = red.height;
            _splitterComputeShader.SetFloat("_Width", width);
            _splitterComputeShader.SetFloat("_Height", height);

            _splitterComputeShader.SetTexture(KERNEL_SPLIT_R, "_Source", source);
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_R, "_RedChannel", red);
            _splitterComputeShader.Dispatch(KERNEL_SPLIT_R, width / 8, height / 8, 1);
        }

        public static void SplitG(Texture source, RenderTexture green)
        {
            int width = green.width;
            int height = green.height;
            _splitterComputeShader.SetFloat("_Width", width);
            _splitterComputeShader.SetFloat("_Height", height);

            _splitterComputeShader.SetTexture(KERNEL_SPLIT_G, "_Source", source);
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_G, "_GreenChannel", green);
            _splitterComputeShader.Dispatch(KERNEL_SPLIT_G, width / 8, height / 8, 1);
        }

        public static void SplitB(Texture source, RenderTexture blue)
        {
            int width = blue.width;
            int height = blue.height;
            _splitterComputeShader.SetFloat("_Width", width);
            _splitterComputeShader.SetFloat("_Height", height);

            _splitterComputeShader.SetTexture(KERNEL_SPLIT_B, "_Source", source);
            _splitterComputeShader.SetTexture(KERNEL_SPLIT_B, "_BlueChannel", blue);
            _splitterComputeShader.Dispatch(KERNEL_SPLIT_B, width / 8, height / 8, 1);
        }
    }
}
