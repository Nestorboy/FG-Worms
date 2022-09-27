using UnityEngine;

namespace Visuals
{
    static class ShaderIDs
    {
        internal static readonly int color = Shader.PropertyToID("_Color");
        internal static readonly int fluidHeight = Shader.PropertyToID("_FluidHeight");
    }
}
