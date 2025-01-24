#ifndef _SHADER_UTILS_
#define _SHADER_UTILS_

#include "UnityCG.cginc"

#define PI 3.14159265359
#define TWO_PI 6.28318530718

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

float textureToPixels(float t, float size){
    return t * size - 0.5;
}

float pixelToTexture(float p, float size){
    return (p + 0.5) / size;
}

float3 random_pcg3d(uint3 v) {
    v = v * 1664525u + 1013904223u;
    v.x += v.y * v.z;
    v.y += v.z * v.x;
    v.z += v.x * v.y;

    v ^= v >> 16u;
    v.x += v.y * v.z;
    v.y += v.z * v.x;
    v.z += v.x * v.y;

    return float3(v) * (1.0 / float(0xffffffffu));
}

//TODO: Differentiate between clamped dot and epsilon dot
float clampedDot(float3 a, float3 b) {
    return max(dot(a, b), 0.0001);
}

float epsilonDot(float3 a, float3 b){
    return max(clampedDot(a,b), 0.0001);
}

int isPositive(float x){
    return x > 0;
}

float3 gammaToLinear(float3 col){
    return pow(col, 2.2);
}

float3 linearToGamma(float3 col){
    return pow(col, 1.0/2.2);
}

// TODO: Blue noise offset?
// TODO: Try these: https://github.com/panthuncia/webgl_test/blob/main/index.html
// https://developer.nvidia.com/gpugems/gpugems2/part-i-geometric-complexity/chapter-8-pixel-displacement-mapping-distance-functions
// view Direction should be passed in as tangent space
float2 parallaxMap(float2 uv, float3 viewDirection, sampler2D displacementTex, float displacementStrength){

    if(length(tex2D(displacementTex, uv)) == 0 || displacementStrength < 0.0001)
    {
        return uv;
    }

    const int minSteps = 64;
    const int maxSteps = 64;
    viewDirection = normalize(viewDirection);
    int numSteps = lerp(maxSteps, minSteps, clampedDot(float3(0,0,1), viewDirection));
    float depthPerStep = 1.0 / (float)numSteps;
    float2 uvDelta = (viewDirection.xy * displacementStrength) / (float)numSteps;

    float2 currUV = uv;
    float currDepth = 1.0 - tex2D(displacementTex, currUV).r; //Inversed for depth rather than height
    float currStep = 0.0;

    [unroll(maxSteps)]
    while(currStep < currDepth){
        currUV -= uvDelta;
        currDepth = 1.0 - tex2D(displacementTex, currUV).r;
        currStep += depthPerStep;
    }

    float2 prevUV = currUV + uvDelta;

    float afterStep = currDepth - currStep;
    float beforeStep = 1.0 - tex2D(displacementTex, prevUV).r - currStep + depthPerStep;

    return lerp(currUV, prevUV, afterStep / (afterStep - beforeStep + 0.0001));
}

float3 normalMap(float3 normal, float3 tangent, float3 bitangent, float2 uv, sampler2D normalTex, float normalStrength){
    float4 sample = tex2D(normalTex, uv);
    if(length(sample) == 0)
    {
        return normal;
    }
    
    float3 tangentSpaceNormal = 0;
    tangentSpaceNormal.xy = sample.wy * 2 - 1;
    tangentSpaceNormal.xy *= normalStrength;
    tangentSpaceNormal.z = sqrt(1 - saturate(dot(tangentSpaceNormal.xy, tangentSpaceNormal.xy)));
    
    return normalize(tangentSpaceNormal.x * tangent + tangentSpaceNormal.y * bitangent + tangentSpaceNormal.z * normal);
}

float3 getBitangent(float3 normal, float3 tangent, float handedness){
    return cross(normal, tangent) * handedness * unity_WorldTransformParams.w;
}

float roundToInt(float n){
    return int(n + 0.5);
}

float sqr(float x){
    return x * x;
}

float3 textureToSphericalDirection(float2 t){
    float theta = PI * (1.0 - t.y);
    float phi = 2.0 * PI * (0.5 - t.x);
    return float3(sin(theta) * cos(phi), sin(theta) * sin(phi), cos(theta));
}

float2 directionToSphericalTexture(float3 s){
    float phi = atan2(s.z, s.x);
    float theta = acos(s.y);
    float x = 0.5 - phi / (2.0 * PI);
    float y = 1.0 - theta / PI;
    return float2(x,y);
}

float3x3 getTBNMatrix(float3 normal){
    float3 someVec = float3(1.0, 0.0, 0.0);
    float dd = dot(someVec, normal);
    float3 tangent = float3(0.0, 1.0, 0.0);
    if(1.0 - abs(dd) > 0.00001){
        tangent = normalize(cross(someVec, normal));
    }
    float3 bitangent = cross(normal, tangent);
    return float3x3(tangent, bitangent, normal);
}

float3 transformToTangentSpace(float3 v, float3 worldNormal, float3 worldTangent, float3 worldBitangent) {
    float3x3 worldToTangent = float3x3(
        worldTangent,
        worldBitangent,
        worldNormal
    );
    
    return mul(worldToTangent, v);
}


#endif