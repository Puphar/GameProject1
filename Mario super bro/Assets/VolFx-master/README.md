# VolFx
[![Twitter](https://img.shields.io/badge/Follow-Twitter?logo=twitter&color=white)](https://twitter.com/NullTale)
[![Boosty](https://img.shields.io/badge/Support-Boosty?logo=boosty&color=white)](https://boosty.to/nulltale)

VolFx is a customizable multi post-processing with buffer system for Unity Urp<br>
that allows building a custom scene processing architecture for visual effects creation ✨

Tested with 2022.2, 2023.2 Web

## Features
* Custom passes - expandable, has minimal set of custom effects and generic blit feature
* Targeting post processing - can be applyed to scene objects by layer mask
* Volume controll - all build in effects controlled via volume profile and linked to a layer mask, so scene processing can be easily made dynamic
* Buffer system - can render object to a buffer texture to provide additional textures(like light maps, pattern animations, height etc) then process and use them later throug a shader
* Configurable pipeline - each effect can be reordered and configurade depending on the application
* Part of Artwork project - all effects from [PostArt](https://github.com/NullTale/PixelationFx) projects can be added as passes to VolFx without cluttering Volume or RenderFeature's list

----------------------------------------- - - - - -  -  - <br>

Visual novel example, post processing applyed to scene objects by LayerMask to blur the background and fade character sprite<br>
Thus you can make the appearance effect or apply graphical changes separately to scene objects <br>

![_Cover](https://github.com/NullTale/VolFx/assets/1497430/a1f99548-8bc8-43fb-93ba-0668f9a46ef9)<br>

There is an example of light texture post processing generated by [GiLight2D](https://github.com/NullTale/GiLight2D)<br>
Volume control allows to tweack texture at runtime to achive desiared light visualization<br>

![_Light](https://github.com/NullTale/VolFx/assets/1497430/2bed6140-1b82-41a6-8b9d-adc991334e3c)<br>

Work with 3D

![_cover](https://github.com/NullTale/VolFx/assets/1497430/58f6af02-83ae-4b1a-9f3c-9d995229c02f)

Can have may instances, order of post effects can be configured as their options<br>

![_cover](https://github.com/NullTale/VolFx/assets/1497430/22b67d1c-80fd-4a24-8980-2b7e0bea6a79)<br>

## Install and usage
Install via Unity [PackageManager](https://docs.unity3d.com/Manual/upm-ui-giturl.html) <br>
```
https://github.com/NullTale/VolFx.git
```

---

Add `VolFx` feature to UrpRenderer add post-process passes to it, control via volume profile <br>

![_Urp](https://github.com/NullTale/VolFx/assets/1497430/21dda2ff-c82e-4d46-8335-d542fc53428c) <br>
<sup>* optionally configure the render event, source with output and volume mask</sup>

`VolBuffers` feature can be used to collect specific object to a texture to provide some vfx texture source<br>
that can be post processed at runtime and used later throug a shader

![image](https://github.com/NullTale/VolFx/assets/1497430/9288c212-c6c2-486b-b699-940eacc11a53)

Example of global texture accessed via shader graph (name match, no exposed checkbox)

![image](https://github.com/NullTale/VolFx/assets/1497430/2b99bebc-cc9c-4d1c-ae5d-2ada10f9be1f)


## Build in Effects

VolFx was built to be highly configurable and most of the effects are powerful in their [combination](https://twitter.com/NullTale/status/1693158627442364490) <br>
There can be a number of custom `Blit` passes controlled via material for easy effect implementation(permanent effects).

#### Extended Color Adjustments
With the curve you can adjust the range on which part of the image to apply Color Adjustement <br>
Other option are classic exept the alpha channel that used to adjust alpha chennel to blend images properly <br>

![Adjustments](https://github.com/NullTale/VolFx/assets/1497430/af84b49d-22c3-47fd-a315-d4e8f7b35ac9)

#### Extended Bloom
Threshold controlled by curve, color made with gradient(support blending) <br>
Also has some advanved options in effect pass like flickering, samples count and scuttering curve. <br>
Basically extended remplementation of moust popular effect to process the images <br>

![Bloom](https://github.com/NullTale/VolFx/assets/1497430/12475cb3-ab40-4e89-a3ac-6730155ed075)

#### Blur
Just simple Blur with radial and distortion options

![Blur](https://github.com/NullTale/VolFx/assets/1497430/28b6a7ab-1eae-4053-8088-99a20cc9a6b3)

#### GradientMap
Colorization effect taken from pupular graphic editors [GradientMap](https://www.bcit.cc/cms/lib04/NJ03000372/Centricity/Domain/299/p6_howto_use_gradient_maps%2018.pdf) <br>
Very powerfull tool to colorize and adjust images, support masking and gradient blending at runtime <br>

![Gradient Map](https://github.com/NullTale/VolFx/assets/1497430/819c808c-9b79-4f6c-b618-fffda4c8cea2)

## Custom Effect

There is an example of simple grayscale effect that can be found in `Project Samples`

![_cover](https://github.com/NullTale/VolFx/assets/1497430/9670cfd8-563e-4283-bb9f-b4c221242bd1)

All effect must be inherated from `VolFx.Pass` and then added to a `VolFxRenderFeature` <br>
Material creates automatically using `ShaderName` attribute, `VolumeSettings` implemented using [Unity API](https://docs.unity3d.com/Packages/com.unity.render-pipelines.core@7.1/api/UnityEngine.Rendering.VolumeComponent.html)

```C#
[ShaderName("Hidden/VolFx/Grayscale")] // shader name for pass material
public class GrayscalePass : VolFx.Pass
{
    // =======================================================================
    public override bool Validate(Material mat)
    {
        // use stack from feature settings, feature use custom VolumeStack with its own LayerMask
        var settings = Stack.GetComponent<GrayscaleVol>();
        
        // return false if we don't want to execute pass, standart check
        if (settings.IsActive() == false)
            return false;
        
        // setup material before drawing
        mat.SetFloat("_Weight", settings.m_Weight.value);
        return true;
    }
}
```
Grayscale shader
```C#
Shader "Hidden/VolFx/Grayscale" // name of the shader for ShaderNameAttribute
...

half luma(half3 rgb)
{
    return dot(rgb, half3(0.299, 0.587, 0.114));
}

frag_in vert(const vert_in v)
{
    frag_in o;
    o.vertex = v.vertex;
    o.uv = v.uv;
    return o;
}

half4 frag(const frag_in i) : SV_Target
{
    half4 main = tex2D(_MainTex, i.uv);
    half4 col  = luma(main.rgb);
    
    return half4(lerp(main.rgb, luma(col.rgb), _Weight), main.a);
}
```
Feature work as a wrapper but can be extended by override low level methods, to gain access to a CommandBuffer and other rendering API stuff<br>
```C#
/// called to perform rendering
public virtual void Invoke(CommandBuffer cmd, RTHandle source, RTHandle dest,
                           ScriptableRenderContext context, ref RenderingData renderingData)
{
    Utils.Blit(cmd, source, dest, _material, 0, Invert);
}
```

## PostArt

More effects can be [downloaded](https://github.com/NullTale/PixelationFx) separately for use in combination<br>
If `VolFx` is installed they will work as part of the framework and will not appear in the RenderFeature list
  
Effects applied sequentially to a 3D object

![_cover](https://github.com/NullTale/VolFx/assets/1497430/38b7fa20-84f6-4717-bc26-cd1333c749bf)<br>

* [Pixelation](https://github.com/NullTale/PixelationFx/)
* [VHS](https://github.com/NullTale/VhsFx)
* [ScreenOutline](https://github.com/NullTale/OutlineFilter)
* [ImageFlow](https://github.com/NullTale/FlowFx)
* [OldMovie](https://github.com/NullTale/OldMovieFx)
