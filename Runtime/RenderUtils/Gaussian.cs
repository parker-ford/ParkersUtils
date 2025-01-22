using UnityEngine;

namespace ParkersUtils
{
    public static partial class RenderUtils
    {

        private static float GetGaussianValue(int x, int y, float sigma)
        {
            float result = 1.0f / (2.0f * Mathf.PI * sigma * sigma);
            float exponent = -((float)((x * x) + (y * y)) / (float)(2.0f * sigma * sigma));
            result *= Mathf.Exp(exponent);
            return result;
        }
        public static ComputeBuffer CreateGaussianKernel(int kernelSize)
        {
            float[] data = new float[kernelSize * kernelSize];
            for (int x = -kernelSize / 2, index = 0; x <= kernelSize / 2; x++)
            {
                for (int y = -kernelSize / 2; y <= kernelSize / 2; y++, index++)
                {
                    data[index] = GetGaussianValue(x, y, kernelSize / 6.0f);
                }
            }

            ComputeBuffer computeBuffer = new ComputeBuffer(data.Length, sizeof(float));
            computeBuffer.SetData(data);

            return computeBuffer;
        }
    }
}
