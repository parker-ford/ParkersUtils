using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ParkersUtils
{
    public class ImageEffectViewer : MonoBehaviour
    {
        public Shader shader;
        private Material material;

        void Start()
        {
            SetShader(shader);
        }

        public void SetShader(Shader _shader)
        {
            if (_shader)
            {
                shader = _shader;
                material = new Material(shader);
            }
        }
        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            if (shader)
            {
                Graphics.Blit(src, dest, material);
            }
            else
            {
                Graphics.Blit(src, dest);
            }
        }
    }
}
