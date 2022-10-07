using UnityEngine;

namespace Visuals
{
    static class ShaderIDs
    {
        internal static readonly int Color = Shader.PropertyToID("_Color");
        internal static readonly int FluidHueOffset = Shader.PropertyToID("_FluidHueOffset");
        internal static readonly int FluidHeight = Shader.PropertyToID("_FluidHeight");
        
        internal static readonly int Dimensions = Shader.PropertyToID("_Dimensions");
        internal static readonly int Scale = Shader.PropertyToID("_Scale");
        internal static readonly int Offset = Shader.PropertyToID("_Offset");
        internal static readonly int IsoValue = Shader.PropertyToID("_IsoValue");
        internal static readonly int MaxTris = Shader.PropertyToID("_MaxTris");
    }
}
