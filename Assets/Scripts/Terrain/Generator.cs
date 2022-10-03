using System;
using UnityEngine;
using Visuals;

namespace Terrain
{
    public class Generator : MonoBehaviour
    {
        [SerializeField] private ComputeShader _volumeCompute;
        [SerializeField] private ComputeShader _marchCompute;
        [SerializeField] private Vector3Int _dimensions = Vector3Int.one;
        
        private ComputeBuffer _volumeBuffer;
        private ComputeBuffer _triangleBuffer;
        private ComputeBuffer _counterBuffer;

        private int _voxelCount = 0;
        private int _marchMaxTris = 0;
        
        [SerializeField] private Vector3 _marchScale = Vector3.one;
        [SerializeField] private Vector3 _marchOffset = Vector3.zero;
        private float _marchIso = 0f;

        private struct Vertex
        {
            public Vector3 P;
            public Vector3 N;
        }
        
        private struct Triangle {
            public Vertex A;
            public Vertex B;
            public Vertex C;

            public Vertex this [int i] {
                get {
                    switch (i) {
                        case 0:
                            return A;
                        case 1:
                            return B;
                        default:
                            return C;
                    }
                }
            }
        }
        
        void Awake()
        {
            //GenerateMesh();
        }

        private void Update()
        {
            GenerateMesh();
        }

        private void OnDisable()
        {
            ReleaseBuffers();
        }

        private void AllocateBuffers()
        {
            ReleaseBuffers();
            
            _voxelCount = 64 * _dimensions.x * _dimensions.y * _dimensions.z;
            _marchMaxTris = _voxelCount * 5;
            
            _volumeBuffer = new ComputeBuffer(_voxelCount, sizeof(float));
            _triangleBuffer = new ComputeBuffer(_marchMaxTris, sizeof(float) * 3 * 3 * 2, ComputeBufferType.Append); // 3 vertices (position and normal) per buffer.
            _counterBuffer = new ComputeBuffer(1, sizeof(int), ComputeBufferType.Raw);
        }
        
        private void ReleaseBuffers()
        {
            _triangleBuffer?.Release();
            _counterBuffer?.Release();
            _volumeBuffer?.Release();
        }

        private void GenerateMesh()
        {
            _dimensions.x = Math.Max(_dimensions.x, 1);
            _dimensions.y = Math.Max(_dimensions.y, 1);
            _dimensions.z = Math.Max(_dimensions.z, 1);
            
            AllocateBuffers();

            MarchVolume();

            MarchCubes();

            ComputeBuffer.CopyCount(_triangleBuffer, _counterBuffer, 0);
            int[] countArray = { 0 };
            _counterBuffer.GetData(countArray);
            int triCount = countArray[0];
            
            Triangle[] tris = new Triangle[triCount];
            _triangleBuffer.GetData(tris, 0, 0, triCount);
            
            Vector3[] vertices = new Vector3[triCount * 3];
            Vector3[] normals = new Vector3[triCount * 3];
            int[] triangles = new int[triCount * 3];
            
            for (int i = 0; i < triCount; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    int id = i * 3 + j;
                    triangles[id] = id;
                    vertices[id] = tris[i][j].P;
                    normals[id] = tris[i][j].N;
                }
            }

            MeshFilter filter = GetComponent<MeshFilter>();
            MeshCollider collider = GetComponent<MeshCollider>();
            Mesh mesh = filter.mesh;
            mesh.Clear();

            mesh.vertices = vertices;
            mesh.normals = normals;
            mesh.triangles = triangles;
            if (triCount > 0)
            {
                //mesh.RecalculateNormals();
                collider.sharedMesh = mesh;
            }
        }

        public void MarchVolume()
        {
            _volumeCompute.SetBuffer(0, "VolumeBuffer", _volumeBuffer);
            _volumeCompute.GetKernelThreadGroupSizes(0, out uint volX, out uint volY, out uint volZ);
            _volumeCompute.SetInts(ShaderIDs.Dimensions, _dimensions.x * (int)volX, _dimensions.y * (int)volY, _dimensions.z * (int)volZ);
            _volumeCompute.SetVector(ShaderIDs.Scale, _marchScale);
            _volumeCompute.SetVector(ShaderIDs.Offset, _marchOffset);

            _volumeCompute.SetVector("_Time", Shader.GetGlobalVector("_Time"));

            _volumeCompute.Dispatch(0, _dimensions.x, _dimensions.y, _dimensions.z);
            
            //float[] volumes = new float[voxelCount];
            //_volumeBuffer.GetData(volumes);
            //string s = "Volumes:";
            //foreach (float v in volumes)
            //    s += "\n" + v;
            //print(s);
        }

        public void MarchCubes()
        {
            _triangleBuffer.SetCounterValue(0);
            _counterBuffer.SetCounterValue(0);
            _marchCompute.SetBuffer(0, "Voxels", _volumeBuffer);
            _marchCompute.SetBuffer(0, "TriangleBuffer", _triangleBuffer);
            _marchCompute.SetBuffer(0, "CounterBuffer", _counterBuffer);
            
            _marchCompute.GetKernelThreadGroupSizes(0, out uint marchX, out uint marchY, out uint marchZ);
            _marchCompute.SetInts(ShaderIDs.Dimensions, _dimensions.x * (int)marchX, _dimensions.y * (int)marchY, _dimensions.z * (int)marchZ);
            _marchCompute.SetFloat(ShaderIDs.IsoValue, _marchIso);
            _marchCompute.SetFloat(ShaderIDs.Scale, 1f);

            _marchCompute.Dispatch(0, _dimensions.x, _dimensions.y, _dimensions.z);
        }
    }
}
