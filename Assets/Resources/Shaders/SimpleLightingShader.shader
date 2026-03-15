Shader "Custom/3D/SimpleLightingShader"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _LightIntensity ("Light Intensity", Range(0, 1)) = 1
        _AmbientIntensity ("Ambient Intensity", Range(0, 1)) = 0.275
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : POSITION;
                half3 normal : TEXCOORD0;
            };

            float4 _Color;
            half _LightIntensity;
            half _AmbientIntensity;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.normal = normalize(mul((float3x3)unity_ObjectToWorld, v.normal));
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                half diff = max(0, dot(i.normal, lightDir)) * _LightIntensity;
                half ambient = _AmbientIntensity;
                half3 finalColor = _Color.rgb * (diff + ambient);

                return half4(finalColor, 1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
