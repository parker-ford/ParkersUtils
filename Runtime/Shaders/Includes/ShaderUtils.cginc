#ifndef _SHADER_UTILS_
#define _SHADER_UTILS_

#define PI 3.14159265359
#define TWO_PI 6.28318530718


void WritePixelBilinear(RWTexture2D<float4> tex, float2 id, float4 value)
{
    int2 intIndex = int2(id);
    float2 fractional = frac(id);

    float w00 = (1.0 - fractional.x) * (1.0 - fractional.y);
    float w10 = fractional.x * (1.0 - fractional.y);
    float w01 = (1.0 - fractional.x) * fractional.y;
    float w11 = fractional.x * fractional.y;

    tex[intIndex] += value * w00;
    tex[intIndex + int2(1, 0)] += value * w10;
    tex[intIndex + int2(0, 1)] += value * w01;
    tex[intIndex + int2(1, 1)] += value * w11;
}

float4 ReadPixelBilinear(RWTexture2D<float4> tex, float2 id)
{
    int2 intIndex = int2(id);
    float2 fractional = frac(id);

    float w00 = (1.0 - fractional.x) * (1.0 - fractional.y);
    float w10 = fractional.x * (1.0 - fractional.y);
    float w01 = (1.0 - fractional.x) * fractional.y;
    float w11 = fractional.x * fractional.y;

    float4 color = 
        tex[intIndex] * w00 +
        tex[intIndex + int2(1, 0)] * w10 +
        tex[intIndex + int2(0, 1)] * w01 +
        tex[intIndex + int2(1, 1)] * w11;
    
    return color;
}

void WritePixelBilinear(RWTexture2D<float2> tex, float2 id, float2 value)
{
    int2 intIndex = int2(id);
    float2 fractional = frac(id);

    // float w00 = (1.0 - fractional.x) * (1.0 - fractional.y);
    // float w10 = fractional.x * (1.0 - fractional.y);
    // float w01 = (1.0 - fractional.x) * fractional.y;
    // float w11 = fractional.x * fractional.y;

    float w00 = 1.0;
    float w10 = 0;
    float w01 = 0;
    float w11 = 0;

    tex[intIndex] += value * w00;
    // tex[intIndex + int2(1, 0)] += value * w10;
    // tex[intIndex + int2(0, 1)] += value * w01;
    // tex[intIndex + int2(1, 1)] += value * w11;
}

float2 ReadPixelBilinear(RWTexture2D<float2> tex, float2 id)
{
    int2 intIndex = int2(id);
    float2 fractional = frac(id);

    float w00 = (1.0 - fractional.x) * (1.0 - fractional.y);
    float w10 = fractional.x * (1.0 - fractional.y);
    float w01 = (1.0 - fractional.x) * fractional.y;
    float w11 = fractional.x * fractional.y;

    float2 color = 
        tex[intIndex] * w00 +
        tex[intIndex + int2(1, 0)] * w10 +
        tex[intIndex + int2(0, 1)] * w01 +
        tex[intIndex + int2(1, 1)] * w11;
    
    return color;
}

float textureToPixels(float t, float size){
    return t * size - 0.5;
}

float pixelToTexture(float p, float size){
    return (p + 0.5) / size;
}


#endif