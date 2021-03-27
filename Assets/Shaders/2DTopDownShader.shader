Shader "Lexikus/Shadow/2DTopDown"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
        _HorizontalSkew ("Horizontal Skew", Float) = 0
        [HideInInspector] _VerticalSkew ("Vertical Skew", Float) = 0
        _OffsetX ("Offset X", Float) = 0
        _OffsetY ("Offset Y", Float) = 0
        _ScaleX ("Scale X", Float) = 1
        _ScaleY ("Scale Y", Float) = 1
        _RotationRad ("Rotation Radians", Float) = 0
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
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
        }

        Cull Off
        Lighting Off
        ZWrite Off

        Pass
        {
        Blend One OneMinusSrcAlpha
        CGPROGRAM
            #pragma vertex vertex
            #pragma fragment fragment
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float4 _RendererColor;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            fixed2 _Flip;
            float _HorizontalSkew;
            float _VerticalSkew;
            float _OffsetX;
            float _OffsetY;
            float _ScaleX;
            float _ScaleY;
            float _RotationRad;
            float4 _ShadowColor;

            struct appdata
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip)
            {
                return float4(pos.xy * flip, pos.z, 1.0);
            }

            fixed4 SampleSpriteTexture(float2 uv)
            {
                fixed4 color = tex2D (_MainTex, uv);
                color = color.a > 0.1 ? fixed4(1,1,1,1) : fixed4(1,1,1,0);

            #if ETC1_EXTERNAL_ALPHA
                fixed4 alpha = tex2D (_AlphaTex, uv);
                color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
            #endif

                return color;
            }

            inline float4 skew(float4 vertex, float horizontalSkew, float verticalSkew) {
                float h = horizontalSkew;
                float v = verticalSkew;
                float4x4 transformMatrix = float4x4(
                    1,h,0,0,
                    v,1,0,0,
                    0,0,1,0,
                    0,0,0,1
                );
                return mul(transformMatrix, vertex);
            }

             inline float4 rotate(float4 vertex, float rad) {
                float4x4 transformMatrix = float4x4(
                    cos(rad),sin(rad),0,0,
                    -sin(rad),cos(rad),0,0,
                    0,0,1,0,
                    0,0,0,1
                );
                return mul(transformMatrix, vertex);
            }

            inline float4 transform(float4 vertex, float x, float y) {
                float4x4 transformMatrix = float4x4(
                    1,0,0,x,
                    0,1,0,y,
                    0,0,1,0,
                    0,0,0,1
                );
                return mul(transformMatrix, vertex);
            }

            inline float4 scale(float4 vertex, float x, float y) {
                float4x4 transformMatrix = float4x4(
                    1 * x,0,0,0,
                    0,1 * y,0,0,
                    0,0,1,0,
                    0,0,0,1
                );
                return mul(transformMatrix, vertex);
            }

            v2f vertex (appdata IN)
            {
                v2f OUT;

                UNITY_SETUP_INSTANCE_ID (IN);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
                OUT.texcoord = IN.texcoord;

                OUT.vertex = scale(OUT.vertex, _ScaleX, _ScaleY);
                OUT.vertex = skew(OUT.vertex, _HorizontalSkew, _VerticalSkew);
                OUT.vertex = rotate(OUT.vertex,_RotationRad);
                OUT.vertex = transform(OUT.vertex, _OffsetX, _OffsetY);

                // Shadow color
                OUT.color = _ShadowColor;

                OUT.vertex = UnityObjectToClipPos(OUT.vertex);
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 fragment (v2f IN): SV_Target
            {
                fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
                c.rgb *= c.a;
                return c;
            }
        ENDCG
        }

        // Sprite Default Pass
        Pass
        {
        Blend One OneMinusSrcAlpha
        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment SpriteFrag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma multi_compile_local _ PIXELSNAP_ON
            #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
            #include "UnitySprites.cginc"
        ENDCG
        }
    }
}