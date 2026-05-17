#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

void GetMainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out float DistanceAtten, out float ShadowAtten)
{
#if defined(SHADERGRAPH_PREVIEW)
    // Nilai default agar preview di dalam Shader Graph tidak error
    Direction = float3(0.5, 0.5, 0);
    Color = float3(1, 1, 1);
    DistanceAtten = 1;
    ShadowAtten = 1;
#else
    // Mengubah posisi dunia menjadi kordinat bayangan URP
    float4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
    
    // Mengambil data matahari bawaan URP
    Light mainLight = GetMainLight(shadowCoord);
    
    Direction = mainLight.direction;
    Color = mainLight.color;
    DistanceAtten = mainLight.distanceAttenuation;
    
    // Ini adalah nilai bayangan yang kita cari! (0 = tertutup bayangan, 1 = kena cahaya)
    ShadowAtten = mainLight.shadowAttenuation;
#endif
}

#endif