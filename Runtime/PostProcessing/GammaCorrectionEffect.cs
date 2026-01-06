using System;
using UnityEngine;

namespace ParkersUtils
{
    [Serializable]
    public class GammaCorrectionEffect : PostProcessingEffect
    {
        [SerializeField] private float GammaExponent = 2.2f;
        [SerializeField] private bool Inverse = false;
        public override void ApplyEffect(RenderTexture src, RenderTexture dest)
        {
            RenderUtils.GammaCorrectRenderTexture(src, GammaExponent, Inverse);
            Graphics.Blit(src, dest);
        }
    }
}
