Shader "Custom/Terrain World Space Shader" {

	Properties{
		_Color("Basic Color", Color) = (1,1,1,1) // White colorr
		_Height0("Max Height for Texture 0", float) = 2.0
		_Textre0("Texture 0 (RGB)", 2D) = "surface" {}
		_Height1("Max Height for Texture 1", float) = 4.0
		_Textre1("Texture 1 (RGB)", 2D) = "surface" {}
		_Height2("Max Height for Texture 1", float) = 6.0
		_Texture2("Texture 2 (RGB)", 2D) = "surface" {}
		_Height3("Max Height for Texture 1", float) = 8.0
		_Texture3("Texture 3 (RGB)", 2D) = "surface" {}
		_Height4("Max Height for Texture 1", float) = 10.0
		_Texture4("Texture 4 (RGB)", 2D) = "surface" {}
		_Height5("Max Height for Texture 1", float) = 12.0
		_Texture5("Texture 5 (RGB)", 2D) = "surface" {}
		_Height6("Max Height for Texture 1", float) = 14.0
		_Texture6("Texture 6 (RGB)", 2D) = "surface" {}
		_Height7("Max Height for Texture 1", float) = 16.0
		_Texture7("Texture 7 (RGB)", 2D) = "surface" {}
		_Height8("Max Height for Texture 1", float) = 18.0
		_Texture8("Texture 8 (RGB)", 2D) = "surface" {}
		_Height9("Max Height for Texture 1", float) = 20.0
		_Texture9("Texture 9 (RGB)", 2D) = "surface" {}
		_Height10("Max Height for Texture 1", float) = 22.0
		_Texture10("Texture 10 (RGB)", 2D) = "surface" {}
		_Height11("Max Height for Texture 1", float) = 24.0
		_Texture11("Texture 11 (RGB)", 2D) = "surface" {}
		_Height12("Max Height for Texture 1", float) = 26.0
		_Texture12("Texture 12 (RGB)", 2D) = "surface" {}
		_Texture13("Texture 13 (RGB)", 2D) = "surface" {}
		_Scale("Texture Scale", Float) = 1.0
	}

	SubShader{
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM
		#pragma surface surf Lambert

		struct Input {
		float3 worldNormal;
		float3 worldPos; 
		};

		sampler2D _Texture0;
		sampler2D _Texture1;
		sampler2D _Texture2;
		sampler2D _Texture3;
		sampler2D _Texture4;
		sampler2D _Texture5;
		sampler2D _Texture6;
		sampler2D _Texture7;
		sampler2D _Texture8;
		sampler2D _Texture9;
		sampler2D _Texture10;
		sampler2D _Texture11;
		sampler2D _Texture12;
		sampler2D _Texture13;
		float _Height0;
		float _Height1;
		float _Height2;
		float _Height3;
		float _Height4;
		float _Height5;
		float _Height6;
		float _Height7;
		float _Height8;
		float _Height9;
		float _Height10;
		float _Height11;
		float _Height12;
		float4 _Color;
		float _Scale;

		void surf(Input IN, inout SurfaceOutput o) {
			float2 UV;
			fixed4 c;
			UV = IN.worldPos.xz;
			if (IN.worldPos.y < _Height0) {
				c = tex2D(_Texture0, UV* _Scale);
			}
			else if (IN.worldPos.y >= _Height0 && IN.worldPos.y < _Height1) {
				c = tex2D(_Texture1, UV* _Scale);
			}
			else if (IN.worldPos.y >= _Height1 && IN.worldPos.y < _Height2) {
				c = tex2D(_Texture2, UV* _Scale);
			}
			else if (IN.worldPos.y >= _Height2 && IN.worldPos.y < _Height3) {
				c = tex2D(_Texture3, UV* _Scale);
			}
			else if (IN.worldPos.y >= _Height3 && IN.worldPos.y < _Height4) {
				c = tex2D(_Texture4, UV* _Scale);
			}
			else if (IN.worldPos.y >= _Height4 && IN.worldPos.y < _Height5) {
				c = tex2D(_Texture5, UV* _Scale);
			}
			else if (IN.worldPos.y >= _Height5 && IN.worldPos.y < _Height6) {
				c = tex2D(_Texture6, UV* _Scale);
			}
			else if (IN.worldPos.y >= _Height6 && IN.worldPos.y < _Height7) {
				c = tex2D(_Texture7, UV* _Scale);
			}
			else if (IN.worldPos.y >= _Height7 && IN.worldPos.y < _Height8) {
				c = tex2D(_Texture8, UV* _Scale);
			}
			else if (IN.worldPos.y >= _Height8 && IN.worldPos.y < _Height9) {
				c = tex2D(_Texture9, UV* _Scale);
			}
			else if (IN.worldPos.y >= _Height9 && IN.worldPos.y < _Height10) {
				c = tex2D(_Texture10, UV* _Scale);
			}
			else if (IN.worldPos.y >= _Height10 && IN.worldPos.y < _Height11) {
				c = tex2D(_Texture11, UV* _Scale);
			}
			else if (IN.worldPos.y >= _Height11 && IN.worldPos.y < _Height12) {
				c = tex2D(_Texture12, UV* _Scale);
			}
			else if (IN.worldPos.y >= _Height12) {
				c = tex2D(_Texture13, UV* _Scale);
			}
			o.Albedo = c.rgb * _Color;
		}

		ENDCG
	}

	Fallback "VertexLit"
}