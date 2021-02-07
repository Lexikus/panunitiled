using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OffsetShadowUniforms {
    public static string HorizontalSkew = "_HorizontalSkew";
    public static string VerticalSkew = "_VerticalSkew";
    public static string OffsetX = "_OffsetX";
    public static string OffsetY = "_OffsetY";
    public static string ScaleX = "_ScaleX";
    public static string ScaleY = "_ScaleY";
    public static string RotationRad = "_RotationRad";
    public static string ShadowColor = "_ShadowColor";
}

public class OffsetShadowController : MonoBehaviour {
    private Material material;
    private Shader shader;

    public float offsetX = 0.0f;
    public float offsetY = 0.0f;

    private float ShaderOffsetX {
        get { return material.GetFloat(OffsetShadowUniforms.OffsetX); }
        set { material.SetFloat(OffsetShadowUniforms.OffsetX, value); }
    }

    private float ShaderOffsetY {
        get { return material.GetFloat(OffsetShadowUniforms.OffsetY); }
        set { material.SetFloat(OffsetShadowUniforms.OffsetY, value); }
    }

    public void Awake() {
        material = GetComponent<Renderer>().material;
    }

    public void Update() {
        ShaderOffsetX = offsetX;
        ShaderOffsetY = offsetY;
    }
}
