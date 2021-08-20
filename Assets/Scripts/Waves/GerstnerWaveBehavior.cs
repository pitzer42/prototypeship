using System;
using UnityEngine;

namespace Waves
{
    /// <summary>
    /// Edit Custom/Wave shader properties during runtime.
    /// Allow specific instance of GerstnerWave to interact with the scene.
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class GerstnerWaveBehavior : MonoBehaviour
    {
        #region Shader Property Names
        const string SHADER_NAME = "Custom/Wave";
        const string STEEPNESS_ID = "_Steepness";
        const string DIRECTION_ID = "_Direction";
        const string WAVE_NUMBER_ID = "_WaveNumber";
        const string PHASE_SPEED_ID = "_PhaseSpeed";
        const string AMPLITUDE_ID = "_Amplitude";
        const string WAVE_TIME_ID = "_WaveTime";
        #endregion

        [SerializeField] private GerstnerWave wave;

        private MeshRenderer meshRenderer;
        private Material material;

        public GerstnerWave Wave { get => wave; }

        public static float WaveTime { get => Time.timeSinceLevelLoad; }


        void Start()
        {
			Physics.IgnoreLayerCollision(0, gameObject.layer);

            meshRenderer = GetComponent<MeshRenderer>();
            material = meshRenderer.material;
            if (!material.shader.name.Equals(SHADER_NAME))
                Debug.LogError(SHADER_NAME + " shader not found in material " + material.name);
            else
                WriteShaderProperties();
        }


        #if UNITY_EDITOR
        //Write shader properties on the fly only when playing on Editor
        void Update()
        {
            WriteShaderProperties();
        }
        #endif

        public void WriteShaderProperties()
        {
            float waveNumber = wave.WaveNumber;
            float phaseSpeed = GerstnerWave.Math.PhaseSpeed(waveNumber);
            float amplitude = GerstnerWave.Math.Amplitude(wave.Steepness, waveNumber);
            
            var properties = new MaterialPropertyBlock();
            meshRenderer.GetPropertyBlock(properties);
            
            properties.SetVector(DIRECTION_ID, wave.Direction4);
            properties.SetFloat(STEEPNESS_ID, wave.Steepness);
            properties.SetFloat(WAVE_NUMBER_ID, waveNumber);
            properties.SetFloat(PHASE_SPEED_ID, phaseSpeed);
            properties.SetFloat(AMPLITUDE_ID, amplitude);
            
            meshRenderer.SetPropertyBlock(properties);
        }
    }
}
