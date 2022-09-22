using System;
using UnityEngine;

namespace Terrain
{
    public class Generator : MonoBehaviour
    {
        [SerializeField]
        private ComputeShader compute;
        [SerializeField]
        private Vector3Int dimensions;
        
        private ComputeBuffer _triangleBuffer;
        private ComputeBuffer _counterBuffer;

        private int marchMaxTris = 65535;
        private int marchIso = 0;
        private int marchScale = 1;
        
        private struct Triangle {
#pragma warning disable 649
            public Vector3 a;
            public Vector3 b;
            public Vector3 c;

            public Vector3 this [int i] {
                get {
                    switch (i) {
                        case 0:
                            return a;
                        case 1:
                            return b;
                        default:
                            return c;
                    }
                }
            }
        }
        
        void Awake()
        {
            GenerateMesh();
        }

        private void Update()
        {
            //GenerateMesh();
        }

        private void AllocateBuffers()
        {
            ReleaseBuffers();
            
            int voxelCount = 64 * dimensions.x * dimensions.y * dimensions.z;
            marchMaxTris = voxelCount * 5;
            
            _triangleBuffer = new ComputeBuffer(marchMaxTris, sizeof (float) * 3 * 3, ComputeBufferType.Append); // 3 vertices per buffer.
            _counterBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
        }
        
        private void ReleaseBuffers()
        {
            _triangleBuffer?.Release();
            _counterBuffer?.Release();
        }

        private void GenerateMesh()
        {
            dimensions.x = Math.Max(dimensions.x, 1);
            dimensions.y = Math.Max(dimensions.y, 1);
            dimensions.z = Math.Max(dimensions.z, 1);
            
            AllocateBuffers();
            
            // Prepare buffers.
            _triangleBuffer.SetCounterValue(0);
            _counterBuffer.SetCounterValue(0);
            compute.SetBuffer(0, "TriangleBuffer", _triangleBuffer);
            compute.SetBuffer(0, "CounterBuffer", _counterBuffer);
            
            compute.SetInts("_Dimensions", dimensions.x, dimensions.y, dimensions.z);
            compute.SetInt("_MaxTriangle", marchMaxTris);
            compute.SetInt("_IsoValue", marchIso);
            compute.SetInt("_Scale", marchScale);
            compute.SetVector("_Time", Shader.GetGlobalVector ("_Time"));
            
            compute.Dispatch(0, dimensions.x, dimensions.y, dimensions.z);
            
            ComputeBuffer.CopyCount(_triangleBuffer, _counterBuffer, 0);
            int[] countArray = { 0 };
            _counterBuffer.GetData(countArray);
            int triCount = countArray[0];
            
            Triangle[] tris = new Triangle[triCount];
            _triangleBuffer.GetData(tris, 0, 0, triCount);
            
            Vector3[] vertices = new Vector3[triCount * 3];
            int[] triangles = new int[triCount * 3];

            string s = $"Verts [{triCount}]:";
            for (int i = 0; i < triCount; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    triangles[i * 3 + j] = i * 3 + j;
                    vertices[i * 3 + j] = tris[i][j];
                    s += "\n" + tris[i][j];
                }
            }

            print(s);
            
            MeshFilter filter = GetComponent<MeshFilter>();
            Mesh mesh = filter.mesh;
            mesh.Clear();

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();

            ReleaseBuffers();
        }
    }
}
