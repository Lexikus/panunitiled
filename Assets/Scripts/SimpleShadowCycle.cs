using System.Collections.Generic;
using UnityEngine;

/*
 * This script is only for debugging and demonstration purpose.
 * There are better ways to manipulate every material's value in
 * the scene. For instance a shared material could be used.
 */
public class SimpleShadowCycle : MonoBehaviour {
    private SpriteRenderer[] spriteRenderer;
    private float skew = 0.2f;
    private float offset = 0.01f;

    public void Start() {
        spriteRenderer = FindObjectsOfType<SpriteRenderer>();
    }

    public void OnCycleChange(float cycle) {
        foreach (var sprite in spriteRenderer) {
            if(sprite.material.name.Contains("SkewShadow")) {
                sprite.material.SetFloat(ShadowConfig.HorizontalSkewUniform, skew * cycle);
            }
            if(sprite.material.name.Contains("OffsetShadow")) {
                sprite.material.SetFloat(ShadowConfig.OffsetXUniform, offset * cycle);
            }
        }
    }
}
