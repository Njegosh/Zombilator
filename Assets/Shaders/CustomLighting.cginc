
void MainLight_half(float3 WorldPos, out half3 Direction, out half3 Color, out half DistanceAtten, out half ShadowAtten)
{
#if SHADERGRAPH_PREVIEW
   Direction = half3(0.5, 0.5, 0);
   Color = 1;
   DistanceAtten = 1;
   ShadowAtten = 1;

#else

   half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
   Light mainLight = GetMainLight(shadowCoord, WorldPos, 1);
   Direction = mainLight.direction;
   Color = mainLight.color;
   DistanceAtten = mainLight.distanceAttenuation;
   //ShadowAtten = mainLight.shadowAttenuation;

   #if defined(_RECEIVE_SHADOWS_OFF)
        ShadowAtten = 0;
   #else
        ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
        float shadowStrength = GetMainLightShadowStrength();
        ShadowAtten = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture, sampler_MainLightShadowmapTexture), shadowSamplingData, shadowStrength, false); 
/*SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture,
        sampler_MainLightShadowmapTexture),
        shadowSamplingData, shadowStrength, false);*/
   #endif

   

   
#endif
}

/*
#if SHADOWS_SCREEN
   half4 clipPos = TransformWorldToHClip(WorldPos);
   half4 shadowCoord = ComputeScreenPos(clipPos);
#else
   half4 shadowCoord = TransformWorldToShadowCoord(WorldPos);
#endif


   #if defined(_RECEIVE_SHADOWS_OFF)
        ShadowAtten = 0;
   #else
        ShadowSamplingData shadowSamplingData = GetMainLightShadowSamplingData();
        float shadowStrength = GetMainLightShadowStrength();
        ShadowAtten = SampleShadowmap(shadowCoord, TEXTURE2D_ARGS(_MainLightShadowmapTexture,
        sampler_MainLightShadowmapTexture),
        shadowSamplingData, shadowStrength, false);
   #endif

*/