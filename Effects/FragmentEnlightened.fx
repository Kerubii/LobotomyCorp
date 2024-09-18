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
float2 uWorldPosition;
float2 uImageSize0;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;
float4 uCustomData;

float4 FragmentEnlightened(float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float4 tex = tex2D(uImage0, coords);

    float2 range = uCustomData.x / uImageSize0;

    if (uSaturation == 1 && tex.a > 0 &&
        (tex2D(uImage0, float2(coords.x - range.x, coords.y)).a == 0 ||
         tex2D(uImage0, float2(coords.x + range.x, coords.y)).a == 0 ||
         tex2D(uImage0, float2(coords.x, coords.y - range.y)).a == 0 ||
         tex2D(uImage0, float2(coords.x, coords.y + range.y)).a == 0 ))
    {
        float2 frame = (float2)(uSourceRect.z, uSourceRect.w);
        float2 trueCoords = coords * uImageSize0;
        coords = trueCoords % frame;
        float2 imageCoords = (uWorldPosition + coords) / uImageSize1;
        if (tex.a > 0)
        {
            float4 tex2 = tex2D(uImage1, imageCoords);
            return tex2;
        }
    }
    float alpha = uOpacity;
    tex.rgb *= uColor * alpha;
    tex.a *= alpha;
    return tex;
}

technique Technique1
{
    pass FragmentEn
    {
        PixelShader = compile ps_2_0 FragmentEnlightened();
    }
}