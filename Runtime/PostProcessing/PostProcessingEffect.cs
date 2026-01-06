using System;
using UnityEngine;

namespace ParkersUtils
{
    [Serializable]
    public abstract class PostProcessingEffect
    {
        [SerializeField] public bool Active = true;
        public abstract void ApplyEffect(RenderTexture src, RenderTexture dest);
    }
}
