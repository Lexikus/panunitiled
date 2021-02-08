Shader "Lexikus/Shadow/2DRealTopDownShader"
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

        _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
        _ShadowMapTex ("Shadow Texture", 2D) = "white" {}
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

                float4x4 _LightSpace;
                sampler2D _MainTex;
                sampler2D _ShadowMapTex;
                fixed4 _ShadowColor;
                fixed4 _Color;
                fixed4 _RendererColor;

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    fixed4 color : COLOR;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    fixed4 color : COLOR;
                    float2 uv : TEXCOORD0;
                    float4 fragPos : TEXCOORD1;
                    float4 fragPosLightSpace : TEXCOORD2;
                };

                fixed4 SampleSpriteTexture(sampler2D tex, float2 uv)
                {
                    fixed4 color = tex2D(tex, uv);
                    return color;
                }

                float ShadowCalculation(float4 fragPosLightSpace)
                {
                    float3 projCoords = fragPosLightSpace.xyz / fragPosLightSpace.w;
                    projCoords = projCoords * 0.5 + 0.5;

                    // get closest depth value from light's perspective (using [0,1] range fragPosLight as coords)
                    float closestDepth = tex2D(_ShadowMapTex, projCoords.xy).r;
                    // get depth of current fragment from light's perspective
                    float currentDepth = projCoords.z;
                    // check whether current frag pos is in shadow
                    float shadow = currentDepth > closestDepth ? 1.0 : 0.0;
                    return shadow;
                }

                v2f vertex (appdata IN)
                {
                    v2f OUT;

                    OUT.fragPos = mul(unity_ObjectToWorld, IN.vertex);
                    float4x4 mv = mul(_LightSpace, unity_ObjectToWorld);
                    OUT.fragPosLightSpace = mul(mv, IN.vertex);

                    OUT.color = IN.color * _Color * _RendererColor;
                    OUT.uv = IN.uv;
                    OUT.vertex = UnityObjectToClipPos(IN.vertex);
                    return OUT;
                }

                fixed4 fragment (v2f IN) : SV_Target
                {
                    float shadow = ShadowCalculation(IN.fragPosLightSpace);
                    fixed4 c = SampleSpriteTexture(_MainTex, IN.uv) * IN.color;
                    c.rgb *= c.a;
                    c *= _Color;
                    c *= shadow > 0.0 ? _ShadowColor : 1.0;
                    return c;
                }
            ENDCG
        }
    }
}
