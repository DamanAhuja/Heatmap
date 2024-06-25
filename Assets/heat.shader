Shader "Custom/HeatmapShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HeatMap ("Heatmap Texture", 2D) = "white" {}
    }
    
    SubShader
    {
        Tags { "Queue"="Overlay" "IgnoreProjector"="True" "RenderType"="Opaque" }
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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Intensity;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed heat = tex2D(_MainTex, i.uv).r; // Read red channel (heatmap value)

                // Assign discrete colors based on heatmap value ranges
                fixed4 color = fixed4(0, 0, 0, 1); // Default color (black)

                if (heat < 0.2) color = fixed4(0, 0, 1, 1);     // Blue
                else if (heat < 0.4) color = fixed4(0, 1, 1, 1); // Cyan
                else if (heat < 0.6) color = fixed4(0, 1, 0, 1); // Green
                else if (heat < 0.8) color = fixed4(1, 1, 0, 1); // Yellow
                else color = fixed4(1, 0, 0, 1);                // Red

                return color;
            }
            ENDCG
        }
    }
}
