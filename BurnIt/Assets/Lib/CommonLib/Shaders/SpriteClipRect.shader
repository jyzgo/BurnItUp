// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SpriteClipRect" {
	Properties {
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
		_TextureX ("Texture X", Range (0, 1)) = 0
		_TextureY ("Texture Y", Range (0, 1)) = 0
		_ClipRectX ("Clip Rect X", Range (0, 1)) = 0
		_ClipRectY ("Clip Rect Y", Range (0, 1)) = 0
		_ClipRectW ("Clip Rect W", Range (0, 1)) = 1
		_ClipRectH ("Clip Rect H", Range (0, 1)) = 1
	}

	SubShader {
		Tags {
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		LOD 200

		Pass {
			Cull Off
			Lighting Off
			ZWrite Off
			Offset -1, -1
			Fog { Mode Off }
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float _TextureX;
			float _TextureY;
			float _ClipRectX;
			float _ClipRectY;
			float _ClipRectW;
			float _ClipRectH;

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0; 
				fixed4 color : COLOR;
			};

			struct v2f {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};

			v2f vert(appdata_t v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = v.texcoord;
				o.color = v.color;
				return o;
			}

			half4 frag(v2f IN) : COLOR {
				float left = _TextureX + _ClipRectX;
				if (IN.texcoord.x < left) {
					return half4(0, 0, 0, 0);
				}
				float right = left + _ClipRectW;
				if (IN.texcoord.x >= right) {
					return half4(0, 0, 0, 0);
				}
				float bottom = _TextureY + _ClipRectY;
				if (IN.texcoord.y < bottom) {
					return half4(0, 0, 0, 0);
				}
				float top = bottom + _ClipRectH;
				if (IN.texcoord.y >= top) {
					return half4(0, 0, 0, 0);
				}
				return tex2D(_MainTex, IN.texcoord) * IN.color;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
