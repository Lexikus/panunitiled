Shader "Lexikus/Shadow/2DRealTopDownShadowMapShader"
{
    Properties
    {
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4x4 _lightViewMatrix;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 lightDepth : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;

                float4x4 lightMV = mul(_lightViewMatrix, unity_ObjectToWorld);
                float4x4 lightMVP = mul(UNITY_MATRIX_P, lightMV);
                float4 lightVertex = mul(lightMVP, v.vertex);

                o.lightDepth = lightVertex.zw;
                o.vertex = lightVertex;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half lightDepth = i.lightDepth.x / i.lightDepth.y;
                lightDepth = 1 - lightDepth;
                return fixed4(lightDepth, lightDepth, lightDepth, 1);
            }
            ENDCG
        }
    }
}
