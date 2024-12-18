// Made with Amplify Shader Editor v1.9.6.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SH_VFX_Veffects_AddDissolve"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		_EmissiveIntensity("Emissive Intensity", Float) = 1
		_NoisePower("Noise Power", Float) = 1
		_NoiseScale("Noise Scale", Vector) = (1,1,0,0)
		_NoiseSpeed("Noise Speed", Vector) = (0,0.2,0,0)
		[Space(33)][Header(AR)][Space(13)]_Cull("Cull", Float) = 0
		_Src("Src", Float) = 1
		_Dst("Dst", Float) = 1
		_ZWrite("ZWrite", Float) = 0
		_ZTest("ZTest", Float) = 2
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull [_Cull]
		ZWrite [_ZWrite]
		ZTest [_ZTest]
		Blend [_Src] [_Dst]
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 uv2_texcoord2;
		};

		uniform float _ZWrite;
		uniform float _ZTest;
		uniform float _Src;
		uniform float _Dst;
		uniform float _Cull;
		uniform sampler2D _Noise;
		uniform float2 _NoiseSpeed;
		uniform float2 _NoiseScale;
		uniform float _NoisePower;
		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform float _EmissiveIntensity;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 panner85 = ( 1.0 * _Time.y * _NoiseSpeed + ( i.uv_texcoord * _NoiseScale ));
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			o.Emission = ( ( (i.vertexColor).rgb * ( i.vertexColor.a * saturate( ( saturate( ( saturate( pow( tex2D( _Noise, panner85 ).g , _NoisePower ) ) * tex2D( _Texture, uv_Texture ).rgb ) ) + saturate( ( 1.0 - i.uv2_texcoord2.z ) ) ) ) ) * i.uv2_texcoord2.w ) * _EmissiveIntensity );
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19603
Node;AmplifyShaderEditor.TextureCoordinatesNode;68;-4224,-1536;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;67;-3840,-1408;Inherit;False;Property;_NoiseScale;Noise Scale;5;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-3840,-1536;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;70;-3584,-1408;Inherit;False;Property;_NoiseSpeed;Noise Speed;6;0;Create;True;0;0;0;False;0;False;0,0.2;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;85;-3584,-1536;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-2944,-1280;Inherit;False;Property;_NoisePower;Noise Power;4;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;64;-3328,-1536;Inherit;True;Property;_Noise;Noise;2;0;Create;True;0;0;0;False;0;False;-1;57e9e92fd03983b41bdbda66dc81e6b2;2c5f648e215789a4599c67843b5f7e9a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.PowerNode;72;-2944,-1536;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;24;-2304,-384;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;28;-2432,-1280;Inherit;True;Property;_Texture;Texture;1;0;Create;True;0;0;0;False;0;False;-1;d8a7e4b0bc507d54193801e43fe6b7d7;d8a7e4b0bc507d54193801e43fe6b7d7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SaturateNode;74;-2688,-1536;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;49;-2048,-768;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-2432,-1536;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;83;-1920,-768;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;75;-2048,-1536;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;-1792,-768;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;47;-1664,-768;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.VertexColorNode;27;-1792,-1024;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1280,-896;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;30;-1536,-1024;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;76;-337.4708,-1087.453;Inherit;False;1238;166;Lush was here! <3;5;81;80;79;78;77;Lush was here! <3;0.3880934,0.240566,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-1280,-1024;Inherit;False;3;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1024,-896;Inherit;False;Property;_EmissiveIntensity;Emissive Intensity;3;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;480.5292,-1037.453;Inherit;False;Property;_ZWrite;ZWrite;10;0;Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;736.5292,-1037.453;Inherit;False;Property;_ZTest;ZTest;11;0;Create;True;0;0;0;True;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-31.47076,-1037.453;Inherit;False;Property;_Src;Src;8;0;Create;True;0;0;0;True;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;80;224.5292,-1037.453;Inherit;False;Property;_Dst;Dst;9;0;Create;True;0;0;0;True;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-287.4708,-1037.453;Inherit;False;Property;_Cull;Cull;7;0;Create;True;0;0;0;True;3;Space(33);Header(AR);Space(13);False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-1024,-1024;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;6;-686,-1051;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;SH_VFX_Veffects_AddDissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;True;_ZWrite;0;True;_ZTest;False;0;False;;0;False;;False;0;Custom;0.5;True;False;0;True;Transparent;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;1;5;True;_Src;10;True;_Dst;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;True;_Cull;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;84;0;68;0
WireConnection;84;1;67;0
WireConnection;85;0;84;0
WireConnection;85;2;70;0
WireConnection;64;1;85;0
WireConnection;72;0;64;2
WireConnection;72;1;73;0
WireConnection;74;0;72;0
WireConnection;49;0;24;3
WireConnection;66;0;74;0
WireConnection;66;1;28;5
WireConnection;83;0;49;0
WireConnection;75;0;66;0
WireConnection;48;0;75;0
WireConnection;48;1;83;0
WireConnection;47;0;48;0
WireConnection;43;0;27;4
WireConnection;43;1;47;0
WireConnection;30;0;27;0
WireConnection;31;0;30;0
WireConnection;31;1;43;0
WireConnection;31;2;24;4
WireConnection;82;0;31;0
WireConnection;82;1;29;0
WireConnection;6;2;82;0
ASEEND*/
//CHKSM=47037371A74621D5FB5BA8B1B0FBB3411A89BF6B