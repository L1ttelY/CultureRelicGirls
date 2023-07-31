Shader "Unlit/Blendmod"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlendTex ("Texture", 2D) = "white" {}
    }
    CGINCLUDE
    #include "UnityCG.cginc"
    struct appdata
    {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
    };
    struct v2f
    {   
        float2 uv : TEXCOORD0;
        UNITY_FOG_COORDS(1)
        float4 vertex : SV_POSITION;
    };
    sampler2D _MainTex;
    float4 _MainTex_ST;
    sampler2D _BlendTex;
    v2f vert (appdata v)
    {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv = TRANSFORM_TEX(v.uv, _MainTex);
        UNITY_TRANSFER_FOG(o,o.vertex);
         return o;
    }
    fixed4 frag (v2f i) : SV_Target
    {
        fixed4 A = tex2D(_MainTex, i.uv);
        fixed4 B = tex2D(_BlendTex,i.uv);

        //B7Nârgba ÊO

        fixed4 C=abs(A-B); //正常透明度混合

        return C;
    }

    ENDCG
    SubShader
    {
        Tags { "Queue"="Transparent" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            ENDCG
        }   
    }
}