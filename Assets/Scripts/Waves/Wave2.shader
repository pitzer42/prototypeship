Shader "Custom/Wave2"
{
    Properties {
		[Header(Gerstner Wave)]
        _Steepness ("Steepness", Float) = .5
        _Direction ("Direction", Vector) = (1.0, .0, .0, .0) 
        _WaveNumber ("WaveNumber", Float) = .5
        _PhaseSpeed ("PhaseSpeed", Float) = .5
        _Amplitude ("Amplitude", Float) = .5
        _WaveTime ("WaveTime", Float) = 0

		[Header(CG)]
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        [Header(Foam)]
		_DistortStrength("Distort Strength", float) = 1.0
		_DepthRampTex("Depth Ramp", 2D) = "white" {}
    }

    SubShader
    {
        Tags {
            "Queue" = "Transparent"
        }        

        Pass
        {

            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM

            #include "UnityCG.cginc"
            #include "HLSLSupport.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _CameraDepthTexture;

			float _Steepness, _WaveNumber, _PhaseSpeed, _Amplitude, _WaveTime;
	    	float4 _Direction;
            sampler2D _MainTex;
            fixed4 _Color;

            struct VertInput
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct FragInput
            {
                //float4 vertex : SV_POSITION; 
				float4 vertex : POSITION; 
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            FragInput vert (VertInput v)
            {
				float4 worldPoint = mul(unity_ObjectToWorld, v.vertex);
				float f = _WaveNumber * (dot(_Direction.xz, worldPoint.xz) - _PhaseSpeed * _Time.y);
				float cosf = cos(f);
				float sinf = sin(f);
				float amp_cosf = _Amplitude * cosf;

				worldPoint.x += _Direction.x * amp_cosf;
				worldPoint.y += _Amplitude * sinf;
				worldPoint.z += _Direction.z * amp_cosf;

				FragInput o;
				o.vertex = mul(unity_WorldToObject, worldPoint);
				o.vertex = UnityObjectToClipPos(o.vertex);
                o.screenPos = ComputeScreenPos(UnityObjectToClipPos(v.vertex));
                o.uv = v.uv;
                return o;
            }
            
			float  _DepthFactor;
			sampler2D _DepthRampTex;

            float4 frag(FragInput input) : COLOR
			{
				return tex2D(_MainTex, input.screenPos.xy) * _Color;
			}
            ENDHLSL
        }
    }
}
