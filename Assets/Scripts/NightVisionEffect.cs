using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NightVisionEffect : MonoBehaviour
{
    public Material EffectMaterial;

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, EffectMaterial);
    }
}
;