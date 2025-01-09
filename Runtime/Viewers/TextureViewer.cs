using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureViewer : MonoBehaviour
{
    public Texture texture;
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (texture)
        {
            Graphics.Blit(texture, dest);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
