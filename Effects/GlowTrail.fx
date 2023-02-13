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
float4 uCustomData;

float4 GlowingShader(float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color2 = tex2D(uImage1, coords);
    float4 result = color;

    float top = (1 - uOpacity) * 1.1;
    if (color2.r < top - 0.1)
    {
        result *= 0;
    }
    else if (color2.r < top)
    {
        result *= ((color2.r - (top - 0.1)) / 0.1);
    }

    float4 color3 = tex2D(uImage2, coords);
    if (color3.r < top)
    {
        color3 *= ((color3.r - (top - 0.1)) / 0.1);
    }

    result += color3;

	return result;
}

technique Technique1
{
    pass GlowTrail
    {
        PixelShader = compile ps_2_0 GlowingShader();
    }
}