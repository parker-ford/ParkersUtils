using UnityEngine;

namespace ParkersUtils
{
    public enum FourierTransformerScaling
    {
        None = 0,
        Inverse = 1,
        Symmetric = 2,
        Logarithmic = 3,
        Forward = 4
    }

    public enum FourierTransformerShift
    {
        None = 0,
        Centered = 1
    }

    public enum FourierTransformerAlgorithm
    {
        FFT = 0,
        DFT = 1,
        FFTSignal8 = 2
    }

    public enum FourierTransformerOutput
    {
        Complex = 0,
        Magnitude = 1,
        Phase = 2
    }

    public readonly struct FourierTransformerSettings
    {
        public FourierTransformerScaling Scaling { get; }
        public FourierTransformerShift Shift { get; }
        public FourierTransformerAlgorithm Algorithm { get; }
        public FourierTransformerOutput Output { get; }

        public FourierTransformerSettings(
            FourierTransformerScaling scaling = default,
            FourierTransformerShift shift = default,
            FourierTransformerAlgorithm algorithm = default,
            FourierTransformerOutput output = default)
        {
            Scaling = scaling;
            Shift = shift;
            Algorithm = algorithm;
            Output = output;
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
                    if (target.width > 1024 || target.height > 1024)
                    {
                        Debug.LogError("Fourtier Transformer Error: FFT algorithm only supports input dimensions up to 1024. Use FFTSignal8  for larger dimensions.");
                        return;
                    }
                    FourierTransformGPU.DFT(target, false);
                    break;
                case FourierTransformerAlgorithm.FFT:
                    if (target.width > 1024 || target.height > 1024)
                    {
                        Debug.LogError("Fourtier Transformer Error: FFT algorithm only supports input dimensions up to 1024. Use FFTSignal8  for larger dimensions.");
                        return;
                    }
                    FourierTransformGPU.FFT(target, false);
                    break;
                case FourierTransformerAlgorithm.FFTSignal8:
                    FourierTransformGPU.FFTRadix8(target, false);
                    break;
            }

            switch (_settings.Output)
            {
                case FourierTransformerOutput.Magnitude:
                    FourierTransformGPU.ConvertToMagnitude(target);
                    break;
                case FourierTransformerOutput.Phase:
                    FourierTransformGPU.ConvertToPhase(target);
                    break;
            }

            switch (_settings.Scaling)
            {
                case FourierTransformerScaling.Symmetric:
                    FourierTransformGPU.SymmetricScale(target);
                    break;
                case FourierTransformerScaling.Logarithmic:
                    FourierTransformGPU.LogarithmicScale(target);
                    break;
                case FourierTransformerScaling.Forward:
                    FourierTransformGPU.LinearScale(target);
                    break;
            }
        }

        public void Inverse(RenderTexture target)
        {
            switch (_settings.Algorithm)
            {
                case FourierTransformerAlgorithm.DFT:
                    if (target.width > 1024 || target.height > 1024)
                    {
                        Debug.LogError("Fourtier Transformer Error: FFT algorithm only supports input dimensions up to 1024. Use FFTSignal8  for larger dimensions.");
                        return;
                    }
                    FourierTransformGPU.DFT(target, true);
                    break;
                case FourierTransformerAlgorithm.FFT:
                    if (target.width > 1024 || target.height > 1024)
                    {
                        Debug.LogError("Fourtier Transformer Error: FFT algorithm only supports input dimensions up to 1024. Use FFTSignal8  for larger dimensions.");
                        return;
                    }
                    FourierTransformGPU.FFT(target, true);
                    break;
                case FourierTransformerAlgorithm.FFTSignal8:
                    FourierTransformGPU.FFTRadix8(target, true);
                    break;
            }

            if (_settings.Shift == FourierTransformerShift.Centered)
            {
                FourierTransformGPU.FrequencyShift(target);
            }

            switch (_settings.Scaling)
            {
                case FourierTransformerScaling.Inverse:
                    FourierTransformGPU.LinearScale(target);
                    break;
                case FourierTransformerScaling.Symmetric:
                    FourierTransformGPU.SymmetricScale(target);
                    break;
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
