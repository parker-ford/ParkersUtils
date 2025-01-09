
using UnityEngine;

namespace ParkersUtils
{

    public enum FourierTransformerScaling
    {
        None = 0,
    };

    public enum FourierTransformerShifting
    {
        None = 0,
        Centered = 1
    };

    public enum FourierTransformerAlgorithm
    {
        FFT = 0,
        DFT = 1
    };

    public readonly struct FourierTransformerSettings
    {
        public FourierTransformerScaling Scaling { get; }
        public FourierTransformerShifting Shifting { get; }
        public FourierTransformerAlgorithm Algorithm { get; }

        public FourierTransformerSettings(
            FourierTransformerScaling scaling = default,
            FourierTransformerShifting shifting = default,
            FourierTransformerAlgorithm algorithm = default)
        {
            Scaling = scaling;
            Shifting = shifting;
            Algorithm = algorithm;
        }
    }

    public class FourierTransformer
    {
        private readonly FourierTransformerSettings _settings;


        public FourierTransformer()
        {
            _settings = new FourierTransformerSettings();
        }

        public FourierTransformer(FourierTransformerSettings settings)
        {
            _settings = settings;
        }

        public void Forward(RenderTexture target)
        {
            FourierTransformGPU.DFT(target, false);
        }

        public void Inverse(RenderTexture target)
        {

        }
    }
}
