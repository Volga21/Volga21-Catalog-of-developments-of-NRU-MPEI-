Shader "Custom/a"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _ColorInt("Internal color", Color) = (1.0, 1.0, 1.0, 1.0)
        _Metallic("Metallic", Range(0,1)) = 0.0
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
		_OcclusionMap("Occlusion", 2D) = "white" {}
        [HideInInspector]_PlaneCenter("Plane Center", vector) = (0, 0, 0, 1)
		[HideInInspector]_PlaneNormal("Plane Normal", vector) = (0, 0, -1, 0)
    }

    CGINCLUDE

	float4 _PlaneCenter;
	float4 _PlaneNormal;

	float Distance2Plane(float3 pt) {

		float3 n = _PlaneNormal.xyz;
		float3 pt2 = _PlaneCenter.xyz;

		float d = (n.x*(pt.x - pt2.x)) + (n.y*(pt.y - pt2.y)) + (n.z*(pt.z - pt2.z)) / sqrt(n.x*n.x + n.y*n.y + n.z*n.z);

		return d;
	}

	ENDCG

    SubShader
    {
        Pass
        {
            Tags {"LightMode"="ForwardBase"}
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"

            struct v2f
            {
                float4 uv : TEXCOORD0;
                fixed4 diff : COLOR0;
                float4 vertex : SV_POSITION;
            };

            half _Metallic;

            v2f vert (appdata_base v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = mul(unity_ObjectToWorld, v.vertex);
                half3 worldNormal = UnityObjectToWorldNormal(v.normal);
                half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
                o.diff = nl * _Metallic;

                // the only difference from previous shader:
                // in addition to the diffuse lighting from the main light,
                // add illumination from ambient or light probes
                // ShadeSH9 function from UnityCG.cginc evaluates it,
                // using world space normal
                o.diff.rgb += ShadeSH9(half4(worldNormal,1));
                return o;
            }
            
            sampler2D _MainTex;
            fixed4 _Color;
			sampler2D _OcclusionMap;

            fixed4 frag (v2f i) : SV_Target
            {
                clip(-Distance2Plane(i.uv.xyz));
                fixed4 col = tex2D(_MainTex, i.uv);
				fixed occlusion = tex2D(_OcclusionMap, i.uv).r;
                col *= i.diff;
				col *= occlusion;
                return _Color* i.diff *occlusion;
            }
            ENDCG
        }
        Pass{

			Cull Front

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				float4 worldPos : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			fixed4 _ColorInt;

			fixed4 frag(v2f i) : SV_Target{
				clip(-Distance2Plane(i.worldPos.xyz));

				return _ColorInt;
			}
			ENDCG
		}

		//  PASS 3
		Pass{

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct v2f {
				float4 pos : SV_POSITION;
				float4 worldPos : TEXCOORD0;
			};

			fixed4 _ColorInt;
			float _EdgeWidth;

			v2f vert(appdata_base v)
			{
				v2f o;
				v.vertex.xyz *= _EdgeWidth;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}		

			fixed4 frag(v2f i) : SV_Target{
				clip(-Distance2Plane(i.worldPos.xyz));

				return _ColorInt;
			}
			ENDCG
		}
    }
}