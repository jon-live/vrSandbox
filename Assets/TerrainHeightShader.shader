Shader "Custom/TerrainHeightShader" {
//	Properties{
//	}
//	SubShader{
//		Tags { "RenderType" = "Opaque" }
//		LOD 200
//
//		CGPROGRAM
//		// Physically based Standard lighting model, and enable shadows on all light types
//		#pragma surface surf Standard fullforwardshadows
//
//		// Use shader model 3.0 target, to get nicer looking lighting
//		#pragma target 3.0
//
//		const static int maxColourCount = 8;
//	
//		int baseColourCount = 2;
//		float3 baseColours[0.2, 0.4];
//		float baseStartHeight[0, 2];
//
//		float minHeight;
//		float MaxHeight;
//
//		float inverseLerp(float a, float b, float value) {
//			return saturate((value - a) / (b - a));
//		}
//
//		struct Input {
//			float3 worldPos;
//		};
//
//		void surf (Input IN, inout SurfaceOutputStandard o) {
//			float heightPercent = inverseLerp(0, 20, IN.worldPos.y);
//			for (int = 0; i < baseColourCount; i++) {
//				float drawStrength = saturate(sign(heightPercent - baseStartHeights[i]));
//				o.Albedo = o.Albedo * (1 - drawStrength) + baseColours[i] * drawStrength;
//			}
//		}
//		ENDCG
//	}
//	FallBack "Diffuse"
}