using UnityEngine;

namespace ParkersUtils
{

    // TODO: This code is in a sorry state. I will need to revist this later

    public class FourierTransformCPU
    {
        public static void DFT(Complex[] samples)
        {
            int N = samples.Length;
            Complex[] frequencies = new Complex[N];

            for (int freqBin = 0; freqBin < N; freqBin++)
            {
                Complex sum = new Complex(0, 0);
                for (int i = 0; i < N; i++)
                {
                    float b = -(2.0f * Mathf.PI * freqBin * i) / N;
                    Complex c = new Complex(Mathf.Cos(b), Mathf.Sin(b));
                    c *= samples[i];
                    sum += c;
                }
                frequencies[freqBin] = sum;
            }

            samples = frequencies;
        }

        public static Complex[] FastFourierTransformDFS(Complex[] samples, bool postProcess = true)
        {
            Complex[] FFT(Complex[] samples)
            {
                int N = samples.Length;
                if (N == 1)
                {
                    return samples;
                }

                int M = N / 2;
                Complex[] evenSamples = new Complex[M];
                Complex[] oddSamples = new Complex[M];

                for (int i = 0; i < M; i++)
                {
                    evenSamples[i] = samples[i * 2];
                    oddSamples[i] = samples[i * 2 + 1];
                }

                Complex[] evenFourier = FFT(evenSamples);
                Complex[] oddFourier = FFT(oddSamples);
                Complex[] frequencies = new Complex[N];

                for (int i = 0; i < M; i++)
                {
                    Complex exponential = Complex.Polar(1.0f, -2.0f * Mathf.PI * i / N);
                    frequencies[i] = evenFourier[i] + exponential * oddFourier[i];
                    frequencies[i + M] = evenFourier[i] - exponential * oddFourier[i];
                }

                return frequencies;
            }

            int N = samples.Length;
            Complex[] frequencies = FFT(samples);
            return frequencies;
        }
    }
}
