using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Waves { 

    public class DebugWave : MonoBehaviour
    {
        public GerstnerWaveBehavior wave;


	void OnDrawGizmos()
	{
		Vector3 surface = transform.position;
		surface.y = wave.Wave.ApproximateSurfaceHeight(transform.position, GerstnerWaveBehavior.WaveTime);
		Gizmos.DrawSphere(surface, .1f);
	}        
    }
}
