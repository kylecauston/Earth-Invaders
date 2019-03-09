Shader "Custom/DrawSquare"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_Center("Center", Vector) = (0, 0, 0, 0)
		_Width("Width", Float) = 10
		_Rotation("Rotation", Float) = 0
		_BandColor("Band Color", Color) = (1, 0, 0, 1)
		_BandWidth("Band Width", Float) = 2
		_Shape("Shape", Int) = 0 // 0 - Square, 1 - Circle
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
		float3 _Center;
		float _Width;
		float _Rotation;
		fixed4 _BandColor;
		float _BandWidth;
		int _Shape;
		float Deg2Rad;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			Deg2Rad = (UNITY_PI * 2.0) / 360.0;
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;

			float3 toVec = _Center - IN.worldPos;
			float rotRadians = _Rotation * Deg2Rad;
			float cr = cos(rotRadians); //1
			float sr = sin(rotRadians); //0
			float3 rotToVec = float3(toVec.x * cr - toVec.z * sr, toVec.y, toVec.x * sr + toVec.z * cr); 
			if (_Shape == 0)
			{
				if (abs(rotToVec.x) < _Width / 2 && abs(rotToVec.z) < _Width / 2  // outside square
					&& (abs(rotToVec.x) > _Width / 2 - _BandWidth || abs(rotToVec.z) > _Width / 2 - _BandWidth)) // band
				{
					o.Albedo = _BandColor;
				}
			}
			else {

				float dist = distance(_Center, IN.worldPos);
				if (dist < _Width + _BandWidth && dist > _Width - _BandWidth)
				{
					o.Albedo = _BandColor;
				}
			}
        }
        ENDCG
    }
    FallBack "Diffuse"
}
