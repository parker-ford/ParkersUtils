using System;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace ParkersUtils
{
    public class PostProcessor : MonoBehaviour
    {
        [SerializeField] private bool flipY = true;
        [SerializeReference, SubclassSelector] public List<PostProcessingEffect> EffectsChain;
        private RenderTexture intermediateA, intermediateB;
        private bool pingPong = true;

        public void OnEnable()
        {
            if (EffectsChain == null) EffectsChain = new List<PostProcessingEffect>();
        }

        private RenderTexture GetSrcRT()
        {
            if (pingPong)
            {
                return intermediateA;
            }
            else
            {
                return intermediateB;
            }
        }

        private RenderTexture GetDestRT()
        {
            if (pingPong)
            {
                return intermediateB;
            }
            else
            {
                return intermediateA;
            }
        }

        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (intermediateA == null || intermediateA.width != src.width || intermediateA.height != src.height)
            {
                intermediateA = RenderUtils.CreateRenderTexture(RenderUtils.CreateRenderTextureDescriptor(src.width, src.height));
            }

            if (intermediateB == null || intermediateB.width != src.width || intermediateB.height != src.height)
            {
                intermediateB = RenderUtils.CreateRenderTexture(RenderUtils.CreateRenderTextureDescriptor(src.width, src.height));
            }

            Graphics.Blit(src, GetSrcRT());


            foreach (var postProcessingEffect in EffectsChain)
            {
                if (postProcessingEffect == null || postProcessingEffect.Active == false)
                {
                    continue;
                }

                postProcessingEffect.ApplyEffect(GetSrcRT(), GetDestRT());
                GetSrcRT().Clear();
                pingPong = !pingPong;
            }

            if (flipY)
            {
                Graphics.Blit(GetSrcRT(), dest, new Vector2(1.0f, -1.0f), new Vector2(0.0f, 1.0f));
            }
            else
            {
                Graphics.Blit(GetSrcRT(), dest);
            }
                       
        }
    }
}
