// Made with Amplify Shader Editor v1.9.6.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SH_VFX_Veffects_AddDissolveEmissive"
{
	Properties
	{
		_Texture("Texture", 2D) = "white" {}
		_EmissiveIntensity("Emissive Intensity", Float) = 1
		_Noise("Noise", 2D) = "white" {}
		_NoisePower("Noise Power", Float) = 1
		_NoiseScale("Noise Scale", Vector) = (1,1,0,0)
		_NoiseSpeed("Noise Speed", Vector) = (1,1,0,0)
		[Space(33)][Header(AR)][Space(13)]_Cull("Cull", Float) = 0
		_Src("Src", Float) = 5
		_Dst("Dst", Float) = 10
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
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 uv2_texcoord2;
		};

		uniform float _ZWrite;
		uniform float _ZTest;
		uniform float _Cull;
		uniform float _Dst;
		uniform float _Src;
		uniform sampler2D _Noise;
		uniform float2 _NoiseScale;
		uniform float2 _NoiseSpeed;
		uniform float _NoisePower;
		uniform sampler2D _Texture;
		uniform float4 _Texture_ST;
		uniform float _EmissiveIntensity;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord58 = i.uv_texcoord * _NoiseScale + ( _Time.y * _NoiseSpeed );
			float2 uv_Texture = i.uv_texcoord * _Texture_ST.xy + _Texture_ST.zw;
			float4 tex2DNode28 = tex2D( _Texture, uv_Texture );
			float temp_output_54_0 = ( saturate( pow( tex2D( _Noise, uv_TexCoord58 ).r , _NoisePower ) ) * tex2DNode28.r );
			float4 color69 = IsGammaSpace() ? float4(0.2,0.1105263,0.05263157,1) : float4(0.03310476,0.01173478,0.004176853,1);
			o.Albedo = saturate( ( saturate( ( saturate( temp_output_54_0 ) * 2.0 ) ) * (color69).rgb ) );
			float3 temp_output_87_0 = (i.vertexColor).rgb;
			float temp_output_63_0 = ( tex2DNode28.b * i.uv2_texcoord2.w );
			o.Emission = ( ( ( temp_output_87_0 + ( temp_output_87_0 * temp_output_63_0 ) ) * saturate( ( ( 1.0 - i.uv2_texcoord2.z ) + ( temp_output_54_0 + tex2DNode28.g ) ) ) ) * _EmissiveIntensity );
			o.Alpha = saturate( temp_output_63_0 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack2.xyzw = customInputData.uv2_texcoord2;
				o.customPack2.xyzw = v.texcoord1;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				surfIN.uv2_texcoord2 = IN.customPack2.xyzw;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=19603
Node;AmplifyShaderEditor.SimpleTimeNode;56;-4224,-1152;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;80;-4224,-1024;Inherit;False;Property;_NoiseSpeed;Noise Speed;6;0;Create;True;0;0;0;False;0;False;1,1;0.1,0.03;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;57;-4224,-1408;Inherit;False;Property;_NoiseScale;Noise Scale;5;0;Create;True;0;0;0;False;0;False;1,1;0.7,0.7;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-3968,-1152;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;58;-3840,-1408;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;60;-3584,-1408;Inherit;True;Property;_Noise;Noise;3;0;Create;True;0;0;0;False;0;False;-1;57e9e92fd03983b41bdbda66dc81e6b2;57e9e92fd03983b41bdbda66dc81e6b2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;59;-3200,-1152;Inherit;False;Property;_NoisePower;Noise Power;4;0;Create;True;0;0;0;False;0;False;1;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;55;-3200,-1408;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;53;-2944,-1408;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;28;-3072,-896;Inherit;True;Property;_Texture;Texture;1;0;Create;True;0;0;0;False;0;False;-1;47749437a63f791418bfbd3af347f0d9;47749437a63f791418bfbd3af347f0d9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;54;-2688,-1408;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;24;-3072,-128;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;27;-2176,-512;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;84;-2432,-1408;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;63;-2688,-128;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;49;-2688,-384;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-2688,-896;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;87;-1920,-512;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;69;-2146.168,-1179.958;Inherit;False;Constant;_Color0;Color 0;6;0;Create;True;0;0;0;False;0;False;0.2,0.1105263,0.05263157,1;0,0,0,0;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;72;-2048,-1408;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;-2304,-896;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;-1664,-512;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;70;-1891.297,-1183.022;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;85;-1792,-1408;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;47;-2048,-896;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-1408,-384;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;73;-304,-1072;Inherit;False;1238;166;Lush was here! <3;5;78;77;76;75;74;Lush was here! <3;0.3880934,0.240566,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-1536,-1408;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-1280,-384;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-1024,-256;Inherit;False;Property;_EmissiveIntensity;Emissive Intensity;2;0;Create;True;0;0;0;False;0;False;1;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;512,-1024;Inherit;False;Property;_ZWrite;ZWrite;10;0;Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;768,-1024;Inherit;False;Property;_ZTest;ZTest;11;0;Create;True;0;0;0;True;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-256,-1024;Inherit;False;Property;_Cull;Cull;7;0;Create;True;0;0;0;True;3;Space(33);Header(AR);Space(13);False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;256,-1026.338;Inherit;False;Property;_Dst;Dst;9;0;Create;True;0;0;0;True;0;False;10;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;0,-1024;Inherit;False;Property;_Src;Src;8;0;Create;True;0;0;0;True;0;False;5;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;86;-1280,-1408;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-1024,-384;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;88;-1024,-768;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;6;-686,-1051;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SH_VFX_Veffects_AddDissolveEmissive;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;True;_ZWrite;0;True;_ZTest;False;0;False;;0;False;;False;0;Custom;0.5;True;True;0;True;Transparent;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;2;5;True;_Src;10;True;_Dst;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;True;_Cull;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;62;0;56;0
WireConnection;62;1;80;0
WireConnection;58;0;57;0
WireConnection;58;1;62;0
WireConnection;60;1;58;0
WireConnection;55;0;60;1
WireConnection;55;1;59;0
WireConnection;53;0;55;0
WireConnection;54;0;53;0
WireConnection;54;1;28;1
WireConnection;84;0;54;0
WireConnection;63;0;28;3
WireConnection;63;1;24;4
WireConnection;49;0;24;3
WireConnection;51;0;54;0
WireConnection;51;1;28;2
WireConnection;87;0;27;0
WireConnection;72;0;84;0
WireConnection;48;0;49;0
WireConnection;48;1;51;0
WireConnection;66;0;87;0
WireConnection;66;1;63;0
WireConnection;70;0;69;0
WireConnection;85;0;72;0
WireConnection;47;0;48;0
WireConnection;82;0;87;0
WireConnection;82;1;66;0
WireConnection;68;0;85;0
WireConnection;68;1;70;0
WireConnection;43;0;82;0
WireConnection;43;1;47;0
WireConnection;86;0;68;0
WireConnection;83;0;43;0
WireConnection;83;1;29;0
WireConnection;88;0;63;0
WireConnection;6;0;86;0
WireConnection;6;2;83;0
WireConnection;6;9;88;0
ASEEND*/
//CHKSM=D4EA2AB8C9AFBA370F9027F95B7064322423FDC7