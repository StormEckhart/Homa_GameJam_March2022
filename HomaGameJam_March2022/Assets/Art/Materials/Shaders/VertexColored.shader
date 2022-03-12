Shader "Unlit/VertexColored"
{
    Properties
    {
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                // Read vertex color from mesh.
                fixed4 color : COLOR0;    
            };

            struct v2f
            {
                // Pass interpolated vertex color to fragment.
                fixed4 color : COLOR0;    
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.color = v.color; // Pass the color through.
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Output the color straight.
                return i.color;
            }
            ENDCG
        }
    }
}