Shader "Custom/Teleporting"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_Bottom("Bottom", Vector) = (0, 0, 0, 0)
		_TimePassed("TimePassed", Float) = 0.0
		_GridSize("Grid size", Float) = 8.0
		_DentIntensity("Dent Intensity", Float) = 0.5
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard fullforwardshadows alpha:fade

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input
			{
				float2 uv_MainTex;
				float3 worldPos;
				float3 viewDir;
				float3 worldNormal;
			};

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			float3 _Bottom;
			float _TimePassed;
			float _GridSize;
			float _DentIntensity;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			float rand(float3 co)
			{
				return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 45.5432))) * 43758.5453);
			}

			// Gradients for 3D noise
			static float gradient[48] = {
				1, 1, 0,    -1,  1, 0,     1, -1,  0,    -1, -1,  0,
				1, 0, 1,    -1,  0, 1,     1,  0, -1,    -1,  0, -1,
				0, 1, 1,     0, -1, 1,     0,  1, -1,     0, -1, -1,
				1, 1, 0,     0, -1, 1,    -1,  1,  0,     0, -1, -1
			};

			float permute(float x) {
				// This makes more sense to me
				float index = fmod(x, 289.0);
				return (index*34.0 + 1.0)*index;
				//return fmod((x*34.0 + 1.0)*x, 289.0);
			}

			float3 fade(float3 t) {

				return t * t * t * (t * (t * 6 - 15) + 10); // new curve
				//  return t * t * (3 - 2 * t); // old curve
			}

			float grad(float x, float3 p) {
				int index = fmod(x, 16.0);
				float3 g = float3(gradient[index * 3], gradient[index * 3 + 1], gradient[index * 3 + 2]);
				return dot(g, p);
			}

			float mlerp(float a, float b, float t) {
				return a + t * (b - a);
			}

			// 3D version of noise function
			float inoise(float3 p) {

				float3 P = fmod(floor(p), 256.0);
				p -= floor(p);
				float3 f = fade(p);

				// HASH COORDINATES FOR 6 OF THE 8 CUBE CORNERS
				float A = permute(P.x) + P.y;
				float AA = permute(A) + P.z;
				float AB = permute(A + 1) + P.z;
				float B = permute(P.x + 1) + P.y;
				float BA = permute(B) + P.z;
				float BB = permute(B + 1) + P.z;

				// AND ADD BLENDED RESULTS FROM 8 CORNERS OF CUBE
				return mlerp(
					mlerp(mlerp(grad(permute(AA), p),
						grad(permute(BA), p + float3(-1, 0, 0)), f.x),
						mlerp(grad(permute(AB), p + float3(0, -1, 0)),
							grad(permute(BB), p + float3(-1, -1, 0)), f.x), f.y),
					mlerp(mlerp(grad(permute(AA + 1), p + float3(0, 0, -1)),
						grad(permute(BA + 1), p + float3(-1, 0, -1)), f.x),
						mlerp(grad(permute(AB + 1), p + float3(0, -1, -1)),
							grad(permute(BB + 1), p + float3(-1, -1, -1)), f.x), f.y), f.z);
			}

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				// Metallic and smoothness come from slider variables
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;

				float nse = inoise(float3((IN.worldPos - _Bottom) * _GridSize));
				nse *= _DentIntensity;

				float a = max(0, frac(nse) + 5.0 + (IN.worldPos - _Bottom).y / 4 - (_TimePassed));

				if (a > 0.5)
				{
					a = 0.0;
				}
				else if (a > 0.4)
				{
					a = 1.0;
					o.Albedo = float3(0, 1.0f, 1.0f);
				}
				else
				{
					a = 1.0;
				}
				o.Alpha = a;

			}
			ENDCG
		}
	FallBack "Diffuse"
}
