Shader "Custom/HeatmapGradient"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _HeatMap ("Heatmap Texture", 2D) = "white" {}
        _Alpha ("Alpha", Range(0,1)) = 1.0 // Add alpha property
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" } // Change render type to Transparent
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade // Enable alpha blending

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _HeatMap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_HeatMap;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        half _Alpha; // Alpha variable

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        fixed4 Gradient(float heat)
        {
            // Define the gradient colors
            fixed4 colors[5];
            colors[0] = fixed4(0, 0, 1, _Alpha); // Blue with alpha
            colors[1] = fixed4(0, 1, 1, _Alpha); // Cyan with alpha
            colors[2] = fixed4(0, 1, 0, _Alpha); // Green with alpha
            colors[3] = fixed4(1, 1, 0, _Alpha); // Yellow with alpha
            colors[4] = fixed4(1, 0, 0, _Alpha); // Red with alpha

            // Scale heat to 0-4 range for the gradient
            heat = heat * 4.0;

            // Get the indices
            int idx1 = (int)floor(heat);
            int idx2 = min(idx1 + 1, 4);

            // Interpolate between the two nearest colors
            float t = heat - idx1;
            return lerp(colors[idx1], colors[idx2], t);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 albedoColor = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            float heat = tex2D(_HeatMap, IN.uv_HeatMap).r; // Assuming the heat value is stored in the red channel

            // Get the color from the gradient based on the heat value
            fixed4 heatColor = Gradient(heat);

            o.Albedo = heatColor.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = heatColor.a; // Use the alpha from the gradient
        }
        ENDCG
    }
    FallBack "Diffuse"
}

