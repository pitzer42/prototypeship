using System;
using UnityEngine;

namespace Waves
{
    [Serializable]
    public class GerstnerWave
    {
        [SerializeField]
        private float waveLength;

        [SerializeField]
        [Range(0.0f, 1.0f)] 
        private float steepness;

        [SerializeField]
        [Range(0.0f, 360.0f)] 
        private float angle;

        public float WaveLength { get => waveLength; set => waveLength = value; }

        public float Steepness { get => steepness; set => steepness = value; }

        public float Angle { get => angle; set => angle = value; }

        public Vector2 Direction
        {
            get { return Math.Direction(angle); }
        }

        public Vector3 Direction3
        {
            get { return Math.Direction3(angle); }
        }

        public Vector4 Direction4
        {
            get { return Math.Direction4(angle); }
        }

        public float WaveNumber
        {
            get { return Math.WaveNumber(waveLength); }
        }

        public float PhaseSpeed
        {
            get { return Math.PhaseSpeed(WaveNumber); }
        }

        public float Amplitude
        {
            get { return Math.Amplitude(steepness, WaveNumber); }
        }

        public Vector3 WaveDisplacement(Vector3 pointXZ, float time)
        {
            float waveNumber = Math.WaveNumber(waveLength);
            float phaseSpeed = Math.PhaseSpeed(waveNumber);
            float amplitude = Math.Amplitude(steepness, waveNumber);
            return Math.WaveDisplacement(pointXZ, waveNumber, phaseSpeed, amplitude, Direction, time);
        }

        public float ApproximateSurfaceHeight(Vector3 pointXZ, float time)
        {
            float waveNumber = Math.WaveNumber(waveLength);
            float phaseSpeed = Math.PhaseSpeed(waveNumber);
            float amplitude = Math.Amplitude(steepness, waveNumber);
    		return Math.ApproximateSurfaceHeight(pointXZ, waveNumber, phaseSpeed, amplitude, Direction, time);
        }

        public static class Math
        {
            public static Vector2 Direction(float angle)
            {
                Vector2 dir2 = Vector2.zero;
                dir2.x = Mathf.Cos(angle * Mathf.Deg2Rad);
                dir2.y = Mathf.Sin(angle * Mathf.Deg2Rad);
                return dir2;
            }

            public static Vector3 Direction3(float angle)
            {
                Vector3 dir3 = Vector3.zero;
                dir3.x = Mathf.Cos(angle * Mathf.Deg2Rad);
                dir3.z = Mathf.Sin(angle * Mathf.Deg2Rad);
                return dir3;
            }

            public static Vector4 Direction4(float angle)
            {
                Vector4 dir4 = Vector4.zero;
                dir4.x = Mathf.Cos(angle * Mathf.Deg2Rad);
                dir4.z = Mathf.Sin(angle * Mathf.Deg2Rad);
                return dir4;
            }

            public static float WaveNumber(float waveLength)
            {
                return 2 * Mathf.PI / waveLength;
            }

            public static float PhaseSpeed(float waveNumber)
            {
                return Mathf.Sqrt(Physics.gravity.magnitude / waveNumber);
            }

            public static float Amplitude(float steepness, float waveNumber)
            {
                return steepness / waveNumber;
            }

            public static Vector3 WaveDisplacement(Vector3 point, float waveNumber, float phaseSpeed, float amplitude, Vector2 direction, float time)
            {
                Vector2 pointXZ = new Vector2(point.x, point.z);
                float f = waveNumber * (Vector2.Dot(direction, pointXZ) - phaseSpeed * time);
                return new Vector3(
                    direction.x * amplitude * Mathf.Cos(f),
                    amplitude * Mathf.Sin(f),
                    direction.y * amplitude * Mathf.Cos(f));
            }

            public static float ApproximateSurfaceHeight(Vector3 pointXZ, float waveNumber, float phaseSpeed, float amplitude, Vector2 direction, float time, int iterations = 10)
            {
		        float height = 0;
		        Vector3 delta = Vector3.zero;
		        Vector3 candidate = Vector3.zero;
		        for(int i = 0; i < iterations; i++)
		        {
		            candidate = pointXZ + delta;
			        candidate += WaveDisplacement(candidate, waveNumber, phaseSpeed, amplitude, direction, time);
			        height = candidate.y;
			        candidate.y = pointXZ.y;
			        delta += (pointXZ - candidate);
		        }
		        return height;
            }
        }
    }
}
