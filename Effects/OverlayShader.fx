sampler uImage0 : register(s0);
sampler uImage1 : register(s1);//HexagonAlpha
sampler uImage2 : register(s2);//Hexagon
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

float4 OverlayShader(float2 coords : TEXCOORD0) : COLOR0
{
    float4 color = tex2D(uImage0, coords);
    float2 hexCoords = coords / (uImageSize1 / uScreenResolution);
    hexCoords += uTime * 0.05;
    float4 light = tex2D(uImage1, hexCoords) * 0.5;
    light.rgb *= uColor.rgb;
    float time = uProgress;
    float lightTime = time % 0.33 / 0.33;
    float4 hBright = tex2D(uImage2, hexCoords);

    float c1;
    float c2;
    if (time < 0.33 || time >= 0.99)
    {
        c1 = hBright.g;
        c2 = hBright.r;
    }
    else if (time < 0.66)
    {
        c1 = hBright.b;
        c2 = hBright.g;
    }
    else
    {
        c1 = hBright.r;
        c2 = hBright.b;
    }
    c1 *= lightTime;
    c2 *= (1 - lightTime);

    float percent = (c1 + c2) * 0.5;
    light.rgb += percent * uSecondaryColor;
    float4 vignette = tex2D(uImage3, coords);
    light *= vignette.r;

    color.rgb += (vignette.rgb * uColor * 0.1 + light.rgb * 0.75) * uOpacity * uIntensity;
    return color;
}

technique Technique1
{
    pass OverlayRedMist
    {
        PixelShader = compile ps_2_0 OverlayShader();
    }
}