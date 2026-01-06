using System;
using UnityEngine;

namespace ParkersUtils
{
    [Serializable]
    public class FourierTransformEffect : PostProcessingEffect
    {
        [SerializeField] private FourierTransformerScaling editorScaling;
        [SerializeField] private FourierTransformerAlgorithm editorAlgorithm;
        [SerializeField] private FourierTransformerOutput editorOutput;
        [SerializeField] private FourierTransformerShift editorShift;
        [SerializeField] private bool editorInverse;

        private FourierTransformerScaling scaling;
        private FourierTransformerAlgorithm algorithm;
        private FourierTransformerOutput output;
        private FourierTransformerShift shift;
        private bool inverse;

        private FourierTransformer fourierTransformer;
        private RenderTexture paddedRt;
        private RenderTexture temp;
        public override void ApplyEffect(RenderTexture src, RenderTexture dest)
        {
            if (CheckForReset())
            {
                scaling = editorScaling;
                algorithm = editorAlgorithm;
                output = editorOutput;
                shift = editorShift;
                inverse = editorInverse;

                FourierTransformerSettings fourierSettings = new FourierTransformerSettings(scaling, shift, algorithm, output);

                fourierTransformer = new FourierTransformer(fourierSettings);
            }

            if (paddedRt == null)
            {
                paddedRt = RenderUtils.CreatePaddedRenderTexture(src);
            }

            if (temp == null)
            {
                temp = RenderUtils.CreateRenderTexture(
                    RenderUtils.CreateRenderTextureDescriptor(512, 512, colorFormat: RenderTextureFormat.RGFloat)
                );
            }

            if (!inverse)
            {
                RenderUtils.SplitR(src, paddedRt);
            }
            else
            {
                //RenderUtils.SplitR
                Graphics.Blit(src, paddedRt);
            }
            

            if (inverse)
            {
                fourierTransformer.Inverse(paddedRt);
            }
            else
            {
                fourierTransformer.Forward(paddedRt);
                //fourierTransformer.Inverse(paddedRt);
            }

            Graphics.Blit(paddedRt, dest);
        }

        private bool CheckForReset()
        {
            if (fourierTransformer == null)
            {
                return true;
            }

            if (
                scaling != editorScaling ||
                algorithm != editorAlgorithm ||
                output != editorOutput ||
                shift != editorShift ||
                inverse != editorInverse
            )
            {
                return true;
            }


            return false;
        }
    }
}
