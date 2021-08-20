Shader "Custom/Wave" {

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
    }

    SubShader {
    
		Tags {
			"Queue" = "Transparent"
			"RenderType"="Transparent" 
		}

	    CGPROGRAM
 
	    #pragma surface surf Standard fullforwardshadows alpha:fade addshadow
		#pragma vertex vert
	    #pragma target 3.0

	    float _Steepness, _WaveNumber, _PhaseSpeed, _Amplitude, _WaveTime;
	    float4 _Direction;

		sampler2D _MainTex;
		half _Glossiness;
	    half _Metallic;
	    fixed4 _Color;

	    struct Input {
	        float2 uv_MainTex;
	    };

	    void vert(inout appdata_full data) {
	        float3 p = mul(unity_ObjectToWorld, data.vertex);
	        float f = _WaveNumber * (dot(_Direction.xz, p.xz) - _PhaseSpeed * _Time.y);
	        float cosf = cos(f);
	        float sinf = sin(f);
	        
	        float _Amplitude_cosf = _Amplitude * cosf;
	        data.vertex.xyz = float3(
	            p.x + _Direction.x * _Amplitude_cosf,
	            p.y + _Amplitude * sinf,
	            p.z + _Direction.z * _Amplitude_cosf
	        );
			data.vertex = mul(unity_WorldToObject, data.vertex);
	        
	        float _Steepness_sinf = _Steepness * sinf;
	        float _Steepness_cosf = _Steepness * cosf;
	        
	        float3 tangent = float3(
	            1 - _Direction.x * _Direction.x * _Steepness_sinf,
	            _Direction.x * _Steepness_cosf,
	            -_Direction.x * _Direction.y * _Steepness_sinf);
	        
	        float3 binormal = float3(
	            tangent.z,
	            _Direction.y * _Steepness_cosf,
	            1 - _Direction.y * _Direction.y * _Steepness_sinf);
	        
	        data.normal = normalize(cross(binormal, tangent));
	    }

	    void surf (Input IN, inout SurfaceOutputStandard o) {
	        fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
	        o.Albedo = c.rgb;
	        o.Metallic = _Metallic;
	        o.Smoothness = _Glossiness;
	        o.Alpha = c.a;
	    }

	    ENDCG
    }
}

