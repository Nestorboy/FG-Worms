using System;
using UnityEngine;

namespace Terrain
{
    public class Generator : MonoBehaviour
    {
        [SerializeField] private ComputeShader volumeCompute;
        [SerializeField] private ComputeShader marchCompute;
        [SerializeField] private Vector3Int dimensions;
        
        private ComputeBuffer _volumeBuffer;
        private ComputeBuffer _triangleBuffer;
        private ComputeBuffer _counterBuffer;

        private int voxelCount = 0;
        private int marchMaxTris = 65535;
        private float marchIso = 0f;
        [SerializeField] private Vector3 marchScale = Vector3.one;
        [SerializeField] private Vector3 marchOffset = Vector3.zero;

        private struct Triangle {
#pragma warning disable 649
            public Vector3 a;
            public Vector3 b;
            public Vector3 c;

            public Vector3 n1;
            public Vector3 n2;
            public Vector3 n3;
            
            //public Vector3 this [int i] {
            //    get {
            //        switch (i) {
            //            case 0:
            //                return a;
            //            case 1:
            //                return b;
            //            default:
            //                return c;
            //        }
            //    }
            //}

            public Vector3 vertex(int i)
            {
                switch (i)
                {
                    case 0:
                        return a;
                    case 1:
                        return b;
                    default:
                        return c;
                }
            }
            
            public Vector3 normal(int i)
            {
                switch (i)
                {
                    case 0:
                        return n1;
                    case 1:
                        return n2;
                    default:
                        return n3;
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
            
            voxelCount = 64 * dimensions.x * dimensions.y * dimensions.z;
            marchMaxTris = voxelCount * 5;
            
            _volumeBuffer = new ComputeBuffer(voxelCount, sizeof(float));
            _triangleBuffer = new ComputeBuffer(marchMaxTris, sizeof(float) * 3 * 3 * 2, ComputeBufferType.Append); // 3 vertices and normals  per buffer.
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
            dimensions.x = Math.Max(dimensions.x, 1);
            dimensions.y = Math.Max(dimensions.y, 1);
            dimensions.z = Math.Max(dimensions.z, 1);
            
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
                    triangles[i * 3 + j] = i * 3 + j;
                    vertices[i * 3 + j] = tris[i].vertex(j);
                    normals[i * 3 + j] = tris[i].normal(j);
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
            volumeCompute.SetBuffer(0, "VolumeBuffer", _volumeBuffer);
            volumeCompute.GetKernelThreadGroupSizes(0, out uint volX, out uint volY, out uint volZ);
            volumeCompute.SetInts("_Dimensions", dimensions.x * (int)volX, dimensions.y * (int)volY, dimensions.z * (int)volZ);
            volumeCompute.SetVector("_Scale", marchScale);
            volumeCompute.SetVector("_Offset", marchOffset);

            volumeCompute.SetVector("_Time", Shader.GetGlobalVector("_Time"));

            volumeCompute.Dispatch(0, dimensions.x, dimensions.y, dimensions.z);
            
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
            marchCompute.SetBuffer(0, "Voxels", _volumeBuffer);
            marchCompute.SetBuffer(0, "TriangleBuffer", _triangleBuffer);
            marchCompute.SetBuffer(0, "CounterBuffer", _counterBuffer);
            
            marchCompute.GetKernelThreadGroupSizes(0, out uint marchX, out uint marchY, out uint marchZ);
            marchCompute.SetInts("_Dimensions", dimensions.x * (int)marchX, dimensions.y * (int)marchY, dimensions.z * (int)marchZ);
            marchCompute.SetInt("_MaxTriangle", marchMaxTris);
            marchCompute.SetFloat("_IsoValue", marchIso);
            marchCompute.SetFloat("_Scale", 1f);

            marchCompute.Dispatch(0, dimensions.x, dimensions.y, dimensions.z);
        }
    }
}
