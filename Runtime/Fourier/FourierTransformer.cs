
using UnityEngine;

namespace ParkersUtils
{

    public enum FourierTransformerScaling
    {
        None = 0,
        Inverse = 1
    };

    public enum FourierTransformerShift
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
        public FourierTransformerShift Shift { get; }
        public FourierTransformerAlgorithm Algorithm { get; }

        public FourierTransformerSettings(
            FourierTransformerScaling scaling = default,
            FourierTransformerShift shift = default,
            FourierTransformerAlgorithm algorithm = default)
        {
            Scaling = scaling;
            Shift = shift;
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


        /*
        *   2D Fourier Transform (GPU)
        */
        public void Forward(RenderTexture target)
        {
            if (_settings.Shift == FourierTransformerShift.Centered)
            {
                FourierTransformGPU.FrequencyShift(target);
            }

            switch (_settings.Algorithm)
            {
                case FourierTransformerAlgorithm.DFT:
                    FourierTransformGPU.DFT(target, false);
                    break;
                case FourierTransformerAlgorithm.FFT:
                    FourierTransformGPU.FFT(target, false);
                    break;
            }
        }

        public void Inverse(RenderTexture target)
        {
            switch (_settings.Algorithm)
            {
                case FourierTransformerAlgorithm.DFT:
                    FourierTransformGPU.DFT(target, true);
                    break;
                case FourierTransformerAlgorithm.FFT:
                    FourierTransformGPU.FFT(target, true);
                    break;
            }

            if (_settings.Shift == FourierTransformerShift.Centered)
            {
                FourierTransformGPU.FrequencyShift(target);
            }

            if (_settings.Scaling == FourierTransformerScaling.Inverse)
            {
                FourierTransformGPU.InverseScale(target);
            }
        }

        /*
        *   1D Fourier Transform (CPU)
        */
        // TODO: Implement 1D fourier transform
        public void Forward(Complex[] target)
        {

        }

        public void Inverse(Complex[] target)
        {

        }

    }
}
