Shader "Custom/GlowShader"
{
    Properties
    {
        _Color("Main Color", Color) = (.5,.5,.5,1)
        _GlowColor("Glow Color", Color) = (1,0,0,1)
        _MainTex("Base (RGB)", 2D) = "white" { }
    }

        SubShader
    {
        Tags {"Queue" = "Overlay" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma exclude_renderers gles xbox360 ps3
            ENDCG

            SetTexture[_MainTex]
            {
                combine primary
            }
        }
    }

        SubShader
    {
        Tags { "Queue" = "Overlay" }
        LOD 100

        Pass
        {
            Name "OUTLINE"
            Tags {"LightMode" = "Always" }

            Cull Front

            ZWrite On
            ZTest LEqual

            Blend SrcAlpha OneMinusSrcAlpha

            ColorMask RGB

            Stencil
            {
                Ref 1
                Comp always
                Pass replace
            }

            CGPROGRAM
            #pragma exclude_renderers gles xbox360 ps3
            #pragma fragment frag
            ENDCG
        }
    }
}
