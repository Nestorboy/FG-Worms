Shader "Custom/Liquid"
{
    Properties
    {
        [Header(Surface)]
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _GlossMap ("Smoothness Map", 2D) = "black" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        
        [Header(Fluid)]
        _FluidHueOffset ("Fluid Hue Offset", Range(0,1)) = 0.0
        _FluidHueRange ("Fluid Hue Range", Range(-1,1)) = 0.1
        _FluidNormal ("Fluid Normal", Vector) = (0,1,0,0)
        _FluidHeight ("Fluid Height", Range(0,1)) = 0.5
        _FluidMaxHeight ("Fluid Maximum Height", Float) = 0.5
        _FluidMinHeight ("Fluid Minimum Height", Float) = -0.5
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue" = "AlphaTest"
        }
        
        Pass
        {
            ZWrite On
            ColorMask 0
        }
        
        CGPROGRAM
        
        #pragma surface surf Standard fullforwardshadows alpha
        #pragma target 3.0
        
        sampler2D _MainTex, _GlossMap;
        
        struct Input
        {
            float2 uv_MainTex;
            float2 uv_GlossMap;
            
            float4 vertex;
            float3 worldPos;
            float3 worldNormal;
        };
        
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        float3 hue2rgb(float hue) {
            hue = frac(hue); //only use fractional part of hue, making it loop
            float r = abs(hue * 6 - 3) - 1; //red
            float g = 2 - abs(hue * 6 - 2); //green
            float b = 2 - abs(hue * 6 - 4); //blue
            float3 rgb = float3(r,g,b); //combine components
            rgb = saturate(rgb); //clamp between 0 and 1
            return rgb;
        }

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        
        float3 _FluidNormal;
        float _FluidMaxHeight, _FluidMinHeight, _FluidHeight, _FluidHueOffset, _FluidHueRange;
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            _FluidNormal = normalize(_FluidNormal);
            
            float3 centerPos = unity_ObjectToWorld._14_24_34;
            float3 incoming = normalize(IN.worldPos - _WorldSpaceCameraPos);
            
            float fragHeight = dot(IN.worldPos - centerPos, _FluidNormal);
            float viewHeight = dot(incoming, _FluidNormal);
            float camHeight = dot(_WorldSpaceCameraPos - centerPos, _FluidNormal);
            float rim = abs(dot(IN.worldNormal, -incoming));
            
            float planeDst = -(fragHeight - lerp(_FluidMinHeight, _FluidMaxHeight, _FluidHeight)) / viewHeight / rim * 2;

            bool camUnderFluid = camHeight < 0;
            bool viewUnderFluid = viewHeight < 0;
            
            float ellipse = camUnderFluid == viewUnderFluid ? 1-planeDst : planeDst;
            if (!camUnderFluid)
                ellipse = 1-ellipse;
            ellipse = saturate(ellipse);

            bool isFluid = ellipse > 0;
            
            float fluidGradient = saturate((fragHeight-_FluidMinHeight) / (_FluidHeight * (_FluidMaxHeight - _FluidMinHeight)));
            
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = isFluid ? 0 : c.rgb;
            o.Emission = isFluid ? hue2rgb(ellipse * fluidGradient * _FluidHueRange + _FluidHueOffset) : 0;
            //o.Emission = ellipse > 0 ? hue2rgb(ellipse * fluidGradient * 0.1) : 0;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = (1 - tex2D (_GlossMap, IN.uv_GlossMap)) * _Glossiness;
            o.Alpha = isFluid ? 1 : c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
