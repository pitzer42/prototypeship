using UnityEngine;

namespace ProceduralGeometry
{
    public static class ProceduralGeometry
    {
        public static Mesh Plane(float width, float depth, int sqrtVertexCount)
        {
            float uvXStep = 1.0f / (sqrtVertexCount - 1);
            float uvZStep = 1.0f / (sqrtVertexCount - 1);
            float xStep = width * uvXStep;
            float zStep = depth * uvZStep;
            int vertexCount = sqrtVertexCount * sqrtVertexCount;


            // Create Vertices, normals and texure coordinates (UV)
            // from left to right then from closest to furtherst
            Vector3[] vertices = new Vector3[vertexCount];
            Vector3[] normals = new Vector3[vertexCount];
            Vector2[] uv = new Vector2[vertexCount];
            int vertexIndex = 0;

            for (int i = 0; i < sqrtVertexCount; i++)
            {
                for (int j = 0; j < sqrtVertexCount; j++)
                {
                    vertexIndex = i * sqrtVertexCount + j;
                    vertices[vertexIndex] = new Vector3(xStep * j, 0, zStep * i);
                    uv[vertexIndex] = new Vector2(uvXStep * i, uvZStep * j);
                    normals[vertexIndex] = Vector3.up;
                }
            }


            // Create Triangles
            // iterating on upper right vertices of quads
            int triangleCount = 2 * (sqrtVertexCount - 1) * (sqrtVertexCount - 1);
            int triangleVertexCount = 3 * triangleCount;
            int[] triangleVertices = new int[triangleVertexCount];
            int upperRight = sqrtVertexCount + 1;
            int lowerLeft = 0;
            int lowerRight = 0;
            int upperLeft = 0;
            int triangleVertexIndex = 0;

            while (upperRight <= vertexCount)
            {
                upperLeft = upperRight - 1;
                lowerLeft = upperLeft - sqrtVertexCount;
                lowerRight = upperRight - sqrtVertexCount;

                // lower triangle clockwise
                triangleVertices[triangleVertexIndex] = lowerLeft;
                triangleVertices[triangleVertexIndex + 1] = upperLeft;
                triangleVertices[triangleVertexIndex + 2] = lowerRight;

                // upper triangle clockwise
                triangleVertices[triangleVertexIndex + 3] = upperLeft;
                triangleVertices[triangleVertexIndex + 4] = upperRight;
                triangleVertices[triangleVertexIndex + 5] = lowerRight;

                triangleVertexIndex += 6; // next quad
                if (++upperRight % sqrtVertexCount == 0) // next upper right is outside?
                    upperRight++; // skip to the next upper right
            }

            return new Mesh
            {
                vertices = vertices,
                normals = normals,
                uv = uv,
                triangles = triangleVertices
            };
        }

        public static void SyncGeometry(MeshFilter filter, MeshCollider collider)
        {
            filter.mesh.RecalculateBounds();
            collider.sharedMesh = null;
            collider.sharedMesh = filter.mesh;
        }

        public static void SyncGeometry(Vector3[] vertices, Mesh mesh, MeshCollider collider)
        {
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
            collider.sharedMesh = null;
            collider.sharedMesh = mesh;
        }


    }
}
