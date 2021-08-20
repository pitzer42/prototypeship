using UnityEngine;

namespace Waves
{ 
    [RequireComponent(typeof(Rigidbody))]
    public class Buoyancy : MonoBehaviour
    {
        public LayerMask surfaceLayer = 16;
        public float maxDepth = 10;
        public float maxBuoyancy = 10;
        public Transform effectorParent;
        private Rigidbody body;
        private Transform[] effectors;
        private float gravity = Physics.gravity.magnitude;
        
        public float Contact{get; private set;}

        void Start()
        {
            body = GetComponent<Rigidbody>();

            if (Mathf.Abs(body.drag) < float.Epsilon)
                Debug.LogWarning("close to zero drag may prevent buoyancy stabilization");

            if (effectorParent == null)
                effectorParent = transform;
            effectors = new Transform[effectorParent.childCount];
            for (int i = 0; i < effectors.Length; i++)
                    effectors[i] = effectorParent.GetChild(i);
        }

        void FixedUpdate()
        {
            Contact = 0;
            Vector3 surface;
            foreach (var effector in effectors)
            {
                surface = FindSurfaceProjection(effector.position);
                float depth = surface.y - effector.position.y;
                if (depth > 0)
                {
                    Vector3 buoyancy = Vector3.up * gravity * depth * depth;
                    buoyancy = Vector3.ClampMagnitude(buoyancy, maxBuoyancy);
                    body.AddForceAtPosition(buoyancy, effector.position, ForceMode.Acceleration);
                    Contact += 1.0f / effectors.Length;
                }
            }
        }

        public Vector3 FindSurfaceProjection(Vector3 point)
		{
			RaycastHit hit;
			Vector3 start = point;
            start.y += maxDepth;
			if (Physics.Raycast(start, Vector3.down, out hit, maxDepth * 1.1f, surfaceLayer))
	        {
                Vector3 surfacePoint = hit.point;
	            GerstnerWaveBehavior wave = hit.transform.gameObject.GetComponent<GerstnerWaveBehavior>();
                surfacePoint.y = wave.Wave.ApproximateSurfaceHeight(surfacePoint, GerstnerWaveBehavior.WaveTime);
                DebugArrow(surfacePoint, Color.green);
                DebugArrow(hit.point, Color.red);
				return surfacePoint;
			}
			return point;
		}
		
		private void DebugArrow(Vector3 point, Color color)
		{
		    Debug.DrawLine(point, point + Vector3.forward * 2, color);
		    Debug.DrawLine(point, point + Vector3.right * 2, color);
		}
		
		
		
		
		
    }
}
