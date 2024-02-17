sampler uImage0 : register(s0); 
sampler uImage1 : register(s1); //Main Texture
sampler uImage2 : register(s2); //Alpha Image 1
sampler uImage3 : register(s3); //Alpha Image 2
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

float4 Blade(float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float2 alphaCoords = coords;
    if (uCustomData.z > 0)
    {
        coords.x *= uCustomData.z / uImageSize1.x;
    }
    if (uCustomData.w > 0)
    {
        coords.x += uCustomData.w / uImageSize1.x;
        if (coords.x > 1)
            coords.x -= 1;
    }

    float4 tex = tex2D(uImage1, coords); //Main image

    float4 alpha = tex2D(uImage2, coords); //Alpha image 1, used for general alpha
    color *= alpha.r / 1;//Apply Alpha to Main image

    alphaCoords += float2(uCustomData.x, uCustomData.y);
    if (alphaCoords.x > 1)
        alphaCoords.x -= 1;
    if (alphaCoords.y > 1)
        alphaCoords.y -= 1;

    alpha = tex2D(uImage3, alphaCoords); //Alpha image 2, used for fading
    float top = (1 - uOpacity) * 1.1;
    if (alpha.r < top - 0.1)
    {
        color *= 0; //Complete opacity
    }
    else if (alpha.r < top)
    {
        color *= ((alpha.r - (top - 0.1)) / 0.1); //Ranging Opacity
    }

	return tex * color; //Apply color and opacity to texture
}

technique Technique1
{
    pass BlaidTrail
    {
        PixelShader = compile ps_2_0 Blade();
    }
}