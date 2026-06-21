The URP packages were moved here in order to touch the light fall off range of point lights, which is done to make the light of the camp fire fall off more gradually.
This is adjusted in: Packages\com.unity.render-pipelines.universal\ShaderLibrary\RealtimeLights.hlsl on line 45, function "DistanceAttentuation".
Taken from: https://docs.unity3d.com/6000.1/Documentation/Manual/urp/lighting/custom-lighting-change-light-falloff.html

When upgrading URP remove these packages and re-apply the monkey patch, pretty please.
