Shader "Nessie/Debug/Face"
{
    Properties
    {

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (bool isFrontFacing : SV_IsFrontFace) : SV_Target
            {

                return isFrontFacing ? float4(0,1,1,1) : float4(1,0,0,1);
            }
            ENDCG
        }
    }
}
