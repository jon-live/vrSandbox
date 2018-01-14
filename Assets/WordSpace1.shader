Shader "TerrainViz/Terrain_Simple"
{
	Properties
	{
		_WorldSpaceUVTrans("WorldSpace UV Transformation", Vector) = (1, 1, 0, 0)
		_TerrainSize("TerrainSize", int) = 1024

		// Base Terrain
		_TerrainField("Terrain Field", 2D) = "white" {}
	_TerrainNormalStrength("Terrain Normal Strength", Range(0, 4)) = 1
		_TerrainBaseColor("Terrain Base Color", Color) = (0.5, 0.45, 0.4)

		// Detail
		_DetailDiffuse("Detail Diffuse Texture", 2D) = "white" {}
	_DetailDiffuseColor("Detail Color", Color) = (1, 1, 1)
		_DetailDiffuseBrightness("Detail Brightness", Range(0, 5)) = 1
		_DetailNormal("Detail Normal Map", 2D) = "bump" {}
	_DetailNormalStrength("Detail Normal Strength", Range(0, 1)) = 1
		_DetailUVTrans("Detail Tiling/Offset", Vector) = (1, 1, 0, 0)
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" }

		CGPROGRAM

#pragma target 3.0

#include "UnityCG.cginc"

#pragma surface surf Lambert addshadow
#pragma vertex vert

#pragma shader_feature VERTEX_NORMALS

		uniform float4 _WorldSpaceUVTrans;
	uniform int _TerrainSize;

	uniform sampler2D_float _TerrainField;
	uniform float _TerrainNormalStrength;
	uniform float4 _TerrainBaseColor;

	uniform float4 _BaseColor, _DetailDiffuseColor;
	uniform sampler2D _DetailDiffuse, _DetailNormal;
	uniform float _DetailDiffuseBrightness, _DetailNormalStrength;
	uniform float4 _DetailUVTrans;

	struct Input
	{
		float2 terrainUV;
		float2 detailUV;
		float3 nrm;
	};

	// ---------

	float TerrainHeightLOD(float2 uv)
	{
		return tex2Dlod(_TerrainField, float4(uv, 0, 0)).x;
	}

	float3 HTNormal_Smooth(float3x3 ht, float scale)
	{
		float3 nrm;
		nrm.x = scale *  ((ht[2][0] - ht[2][2]) + 2 * (ht[1][0] - ht[1][2]) + (ht[0][0] - ht[0][2]));
		nrm.y = 1 / _TerrainNormalStrength;
		nrm.z = scale * -((ht[0][0] - ht[0][2]) + 2 * (ht[1][0] - ht[1][2]) + (ht[2][0] - ht[2][2]));
		return normalize(nrm);
	}

	float3 TerrainNormalLOD(float2 uv)
	{
		float3 offset = float3 (-1.0 / _TerrainSize, 0, 1.0 / _TerrainSize);
		float3x3 heights = { { TerrainHeightLOD(uv + offset.xx), TerrainHeightLOD(uv + offset.yx), TerrainHeightLOD(uv + offset.zx) },
		{ TerrainHeightLOD(uv + offset.xy), TerrainHeightLOD(uv + offset.yy), TerrainHeightLOD(uv + offset.zy) },
		{ TerrainHeightLOD(uv + offset.xz), TerrainHeightLOD(uv + offset.yz), TerrainHeightLOD(uv + offset.zz) } };
		return HTNormal_Smooth(heights, 1);
	}

	float3 combineNormal(float3 base, float3 detail)
	{
		base += float3 (0, 0, 1);
		detail *= float3 (-1, -1, 1);
		return base * dot(base, detail) / base.z - detail;
	}

	float3 scaleNormal(float3 nrm, float strenght)
	{
		nrm.z = nrm.z / strenght;
		return normalize(nrm);
	}


	// ---------

	void vert(inout appdata_full v, out Input o)
	{
		UNITY_INITIALIZE_OUTPUT(Input, o);
		// setup worldspace uvs
		o.terrainUV = mul(unity_ObjectToWorld, v.vertex).xz * _WorldSpaceUVTrans.xy + _WorldSpaceUVTrans.zw;
		o.detailUV = o.terrainUV * _DetailUVTrans.xy + _DetailUVTrans.zw;
		// calculate normals
		o.nrm = TerrainNormalLOD(o.terrainUV).xzy;
		// Offset terrain
		v.vertex.y += TerrainHeightLOD(o.terrainUV);
	}

	void surf(Input IN, inout SurfaceOutput SUR)
	{
		// Terrain and Detail normals
		float3 terrainNRM = IN.nrm;
		float3 detailNRM = UnpackNormal(tex2D(_DetailNormal, IN.detailUV));
		// Blend Normals
		float3 finalNRM = combineNormal(terrainNRM, scaleNormal(detailNRM, _DetailNormalStrength));

		SUR.Albedo = _TerrainBaseColor * _DetailDiffuseColor * tex2D(_DetailDiffuse, IN.detailUV).rgb * _DetailDiffuseBrightness;
		SUR.Normal = finalNRM;
	}

	ENDCG
	}
		FallBack "Diffuse"
}