using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParkersUtils
{
    [Serializable]
    public class SplitColorChannelEffect : PostProcessingEffect
    {
        public enum NumSplitChannels { SingleChannel, DoubleChannel, TripleChannel }

        [SerializeField] NumSplitChannels numChannels;
        [SerializeField] RenderUtils.ColorChannel channel0;
        [SerializeField] RenderUtils.ColorChannel channel1;
        [SerializeField] RenderUtils.ColorChannel channel2;

        RenderTexture splitTex;
        public override void ApplyEffect(RenderTexture src, RenderTexture dest)
        {
            if (numChannels == NumSplitChannels.SingleChannel)
            {
                RenderUtils.SplitChannels(src, new List<RenderUtils.ColorChannel> { channel0 }, new List<RenderTexture> { dest });
            }
            else if (numChannels == NumSplitChannels.DoubleChannel)
            {
                RenderUtils.SplitChannels(src, new List<RenderUtils.ColorChannel> { channel0, channel1 }, new List<RenderTexture> { dest });
            }
            if (numChannels == NumSplitChannels.TripleChannel)
            {
                RenderUtils.SplitChannels(src, new List<RenderUtils.ColorChannel> { channel0, channel1, channel2 }, new List<RenderTexture> { dest });
            }
        }
    }
}
