using System;
using UnityEngine;

namespace ParkersUtils
{
    [Serializable]
    public class ImageOverlayEffect : PostProcessingEffect
    {
        [SerializeField] private Texture2D overlayImage;
        public override void ApplyEffect(RenderTexture src, RenderTexture dest)
        {
            if (overlayImage == null) return;

            Graphics.Blit(overlayImage, dest);
        }
    }
}
