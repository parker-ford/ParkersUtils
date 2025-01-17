using UnityEngine;

namespace ParkersUtils
{
    public static partial class RenderUtils
    {

        private static bool _cleanupInitialized = InitializeCleanup();

        public static bool InitializeCleanup()
        {
            Application.quitting += OnApplicationQuit;
            return true;
        }

        private static void OnApplicationQuit()
        {
            ReleaseResources();
        }


        private static void ReleaseResources()
        {
            if (_pixelBuffer != null)
            {
                _pixelBuffer.Release();
                _pixelBuffer = null;
            }

            if (_tempRenderTexture != null)
            {
                _tempRenderTexture.Release();
                _tempRenderTexture = null;
            }
        }
    }
}
