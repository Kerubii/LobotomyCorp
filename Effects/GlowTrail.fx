sampler uImage0 : register(s0);
sampler uImage1 : register(s1); // Automatically Images/Misc/Perlin via Force Shader testing option
sampler uImage2 : register(s2); // Automatically Images/Misc/noise via Force Shader testing option
sampler uImage3 : register(s3);
float3 uColor;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uDirection;
float uOpacity;
float uTime;
float uIntensity;
float uProgress;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 GlowingShader(float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color2 = tex2D(uImage1, coords);
    float4 result = color;

    if (1 - color2.r > uOpacity)
    {   
        result.rgba *= 0;
    }

    float4 color3 = tex2D(uImage2, coords);
    color3.rgb *= uOpacity;
    if (color3.r < 0.1)
        color3.rgb *= 0;

    result.rgb += color3.rgb;

	return result;
}

technique Technique1
{
    pass GlowTrail
    {
        PixelShader = compile ps_2_0 GlowingShader();
    }
}