// Source: https://github.com/keijiro/ComputeMarchingCubes/blob/main/Assets/MarchingCubes/MarchingCubes.compute

#include "Assets/Scripts/Terrain/Marching Volume.cginc"

#define SIZEOF_UINT 4
#define SIZEOF_FLOAT3 12

uint3 _Dimensions;
uint1 _MaxTriangle;
float _IsoValue;
float _Scale;

// Weights/volumes.
StructuredBuffer<float> Voxels;

uint3 CubeVertex(uint index)
{
    bool x = index & 1;
    bool y = index & 2;
    bool z = index & 4;
    return uint3(x ^ y, y, z);
}

float VoxelValue(uint x, uint y, uint z)
{
    //return Voxels[x + _Dimensions.x * (y + _Dimensions.y * z)];
    return noise(float3(x,y,z) / _Dimensions);
}

float4 VoxelValueWithGradient(uint3 i)
{
    uint3 i_n = max(i, 1) - 1;
    uint3 i_p = min(i + 1, _Dimensions - 1);
    float v = VoxelValue(i.x, i.y, i.z);
    float v_nx = VoxelValue(i_n.x, i.y, i.z);
    float v_px = VoxelValue(i_p.x, i.y, i.z);
    float v_ny = VoxelValue(i.x, i_n.y, i.z);
    float v_py = VoxelValue(i.x, i_p.y, i.z);
    float v_nz = VoxelValue(i.x, i.y, i_n.z);
    float v_pz = VoxelValue(i.x, i.y, i_p.z);
    return float4(v_px - v_nx, v_py - v_ny, v_pz - v_nz, v);
}

uint2 EdgeVertexPair(uint index)
{
    // (0, 1) (1, 2) (2, 3) (3, 0)
    // (4, 5) (5, 6) (6, 7) (7, 4)
    // (0, 4) (1, 5) (2, 6) (3, 7)
    uint v1 = index & 7;
    uint v2 = index < 8 ? ((index + 1) & 3) | (index & 4) : v1 + 4;
    return uint2(v1, v2);
}

float3 TransformPoint(float3 p)
{
    return (p + 0.5 - _Dimensions / 2) * _Scale;
}

uint EdgeIndexFromTriangleTable(uint2 data, uint index)
{
    return 0xfu & (index < 8 ? data.x >> ((index + 0) * 4) :
                               data.y >> ((index - 8) * 4));
}
