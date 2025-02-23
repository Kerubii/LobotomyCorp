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

float4 Trailing(float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    color = tex2D(uImage2, coords) * color; //Main texture Color

    float4 color2 = tex2D(uImage1, coords); //Alpha Image 1, used for genral alpha
    color.rgba *= color2.r;
    
    if (uOpacity < 0.05) 
    {
        color.rgba *= uOpacity / 0.05;
    }
    else if (uOpacity > 0.5)
    {
        float range = (uOpacity - 0.5) / 0.5;
        if (color2.r < range)
            color.rgba = 0;
        else if (color2.r < range + 0.25)
            color.rgba *= (color2.r - range) / 0.25;
    }

	return color;
}

technique Technique1
{
    pass Trail
    {
        PixelShader = compile ps_2_0 Trailing();
    }
}