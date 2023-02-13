sampler uImage0 : register(s0);
sampler uImage1 : register(s1);//Broken Screen
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float3 uColor; //Image info, Scale1, Scale2, Rotation
float3 uSecondaryColor; //Distort Direction
float2 uScreenResolution;
float2 uScreenPosition; 
float2 uTargetPosition; //Center of Image0
float2 uDirection; //Image Direction
float uOpacity;
float uTime;
float uIntensity; //Image Displacement
float uProgress; //ImageOpacity
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;
float2 uImageOffset;
float uSaturation;
float4 uSourceRect;
float2 uZoom;

float4 BrokenShader(float2 coords : TEXCOORD0) : COLOR0
{
    float2 size = float2(200 * uColor.r, 200 * uColor.g) / uScreenResolution;
    float2 min = (uTargetPosition - uScreenPosition) / uScreenResolution - size / 2;
    float2 max = min + size;
    if (min.x < coords.x && coords.x < max.x &&
        min.y < coords.y && coords.y < max.y)
    {
        float2 crackCoord = (coords - min) / size;//float2((coords.x - min.x) / size.x, (coords.y - min.y) / size.y);
        float4 crack = tex2D(uImage1, crackCoord * uDirection) * uProgress;
        crackCoord -= 0.5;
        crackCoord = normalize(crackCoord);
        float range = crack.r * uIntensity;
        coords -= range * crackCoord;
        if (crack.r > 0.18)
        {
            coords.x = -coords.x + 1;
            return tex2D(uImage0, coords) + crack.r;
        }
        else
            return tex2D(uImage0, coords);
    }
    return tex2D(uImage0, coords);
}

technique Technique1
{
    pass BrokenScreenShader
    {
        PixelShader = compile ps_2_0 BrokenShader();
    }
}