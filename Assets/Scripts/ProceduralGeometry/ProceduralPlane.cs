using UnityEngine;

namespace ProceduralGeometry
{ 
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ProceduralPlane : MonoBehaviour
    {
        public int sqrtVertexCount;
        private float width;
        private float depth;
        private MeshFilter filter;

        void Start()
        {
            filter = GetComponent<MeshFilter>();

            width = transform.localScale.x;
            depth = transform.localScale.z;
            transform.localScale = Vector3.one;

            if(sqrtVertexCount < 2)
            {
                sqrtVertexCount = 2;
                Debug.LogWarning("sqrtVertexCount must be greater or equal to 2");
            }

            filter.mesh = ProceduralGeometry.Plane(width, depth, sqrtVertexCount);
            var _collider = GetComponent<MeshCollider>();
            if (_collider)
                ProceduralGeometry.SyncGeometry(filter, _collider);
        }

        void OnDrawGizmos()
        {
            Vector3 size = Vector3.zero;
            size.x = Mathf.Max(width, transform.localScale.x);
            size.y = Mathf.Max(1, transform.localScale.y);
            size.z = Mathf.Max(depth, transform.localScale.z);

            Vector3 center = size / 2;

            Vector3 backup = transform.localScale;
            transform.localScale = Vector3.one;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawWireCube(center, size);
            transform.localScale = backup;
        }
    }
}
