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

            float4x4 _viewMatrix;

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 depth : DEPTH;
            };

            v2f vert (appdata v)
            {
                v2f o;
                float4 vertex = v.vertex;
                float4x4 mv = mul(_viewMatrix, unity_ObjectToWorld);
                float4x4 mvp = mul(UNITY_MATRIX_P, mv);
                vertex = mul(mvp, vertex);
                o.vertex = vertex;
                o.depth = vertex.zw;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half depth = i.depth.x / i.depth.y;
                return fixed4(depth,depth,depth,1);
            }
            ENDCG
        }
    }
}
