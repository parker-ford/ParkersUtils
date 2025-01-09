using System;

namespace ParkersUtils
{
    public static class CommonUtils
    {
        public static int NextPowerOfTwo(int num)
        {
            return (int)Math.Pow(2, Math.Ceiling(Math.Log10(num) / Math.Log10(2)));
        }
    }
}
