Shader "Lexikus/Sprites/Default"
{

    CGINCLUDE
        #include "UnityCG.cginc"
        #include "UnityLightingCommon.cginc"
    ENDCG

    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint", Color) =(1,1,1,1)
        [MaterialToggle] PixelSnap("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor("RendererColor", Color) =(1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) =(1,1,1,1)
        [PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0
        _DirectionalRotation("Directional Rotation", Vector) = (0,0,0)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
            "DisableBatching"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // bring variable into scope
            fixed4 _RendererColor;
            fixed2 _Flip;
            fixed4 _Color;
            float _EnableExternalAlpha;
            sampler2D _MainTex;
            sampler2D _AlphaTex;

            struct appdata
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip)
            {
                return float4(pos.xy * flip, pos.z, 1.0);
            }

            v2f vert(appdata IN)
            {
                v2f OUT;

                float4 vertex = IN.vertex;

                OUT.vertex = UnityFlipSprite(vertex, _Flip);
                OUT.vertex = UnityObjectToClipPos(OUT.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color * _RendererColor;

                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
                c.rgb *= c.a;
                return c;
            }
            ENDCG
        }

        Pass {
            Tags {"LightMode"="ShadowCaster"}

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_shadowcaster
            #pragma fragmentoption ARB_precision_hint_fastest

            struct appdata
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                V2F_SHADOW_CASTER;
            };

            inline float4 RotateAroundYInDegrees(float4 vertex, float degrees)
            {
                float alpha = degrees * UNITY_PI / 180.0;
                float sina, cosa;
                sincos(alpha, sina, cosa);
                float2x2 m = float2x2(cosa, -sina, sina, cosa);
                return float4(mul(m, vertex.xz), vertex.yw).xzyw;
            }

            inline float4 RotateAroundXInDegrees(float4 vertex, float degrees)
            {
                float alpha = degrees * UNITY_PI / 180.0;
                float sina, cosa;
                sincos(alpha, sina, cosa);
                float3x3 m = float3x3(1.0, 0.0, 0.0, 0.0, cosa, -sina, 0.0, sina, cosa);
                return float4(mul(m, vertex.xyz), vertex.w).xyzw;
            }

            inline float4 RotateAroundZInDegrees(float4 vertex, float degrees)
            {
                float alpha = degrees * UNITY_PI / 180.0;
                float sina, cosa;
                sincos(alpha, sina, cosa);
                float2x2 m = float2x2(cosa, -sina, sina, cosa);
                return float4(mul(m, vertex.xy), vertex.zw).xyzw;
            }

            v2f vert(appdata IN)
            {
                v2f OUT;

                TRANSFER_SHADOW_CASTER_NORMALOFFSET(OUT)

                return OUT;
            }

            fixed4 frag(v2f IN) : COLOR
            {
                 SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}