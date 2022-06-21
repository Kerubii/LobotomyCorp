sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
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
float4 uShaderSpecificData;

float4 Resize(float4 sampleColor : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
    float sine = sin(6.28 * uSaturation);
	float cosine = cos(6.28 * uSaturation);

	float2 position = coords - float2(0.5, 0.5);
	float2 centre = float2(0.5, 0.5);

	position = float2(cosine * position.x - sine * position.y, cosine * position.y + sine * position.x) + centre;
	
	float4 color = tex2D(uImage0, position);
	color *= sampleColor;
	color *= uOpacity;
    return color;
}

technique Technique1
{
    pass ArcanaResize
    {
        PixelShader = compile ps_2_0 Resize();
    }
}