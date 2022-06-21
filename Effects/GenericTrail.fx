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

float4 GenTrail(float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 color2 = tex2D(uImage1, coords);
    color.rgba *= color2.r;
	return color;
}

technique Technique1
{
    pass GenericTrail
    {
        PixelShader = compile ps_2_0 GenTrail();
    }
}