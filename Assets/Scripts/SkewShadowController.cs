using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkewShadowUniforms
{
    public static string HorizontalSkew = "_HorizontalSkew";
    public static string VerticalSkew = "_VerticalSkew";
    public static string OffsetX = "_OffsetX";
    public static string OffsetY = "_OffsetY";
    public static string ScaleX = "_ScaleX";
    public static string ScaleY = "_ScaleY";
    public static string RotationRad = "_RotationRad";
    public static string ShadowColor = "_ShadowColor";
}

public class SkewShadowController : MonoBehaviour
{
    private Material material;
    private Shader shader;
    public float horizontalSkew = 0.0f;
    public float verticalSkew = 0.0f;
    public float scaleX = 1.0f;
    public float scaleY = 1.0f;

    private float ShaderSkewHorizontal {
        get { return material.GetFloat(OffsetShadowUniforms.HorizontalSkew); }
        set { material.SetFloat(OffsetShadowUniforms.HorizontalSkew, value); }
    }

    private float ShaderSkewVertical {
        get { return material.GetFloat(OffsetShadowUniforms.VerticalSkew); }
        set { material.SetFloat(OffsetShadowUniforms.VerticalSkew, value); }
    }

    private float ShaderScaleX {
        get { return material.GetFloat(OffsetShadowUniforms.ScaleX); }
        set { material.SetFloat(OffsetShadowUniforms.ScaleX, value); }
    }

    private float ShaderScaleY {
        get { return material.GetFloat(OffsetShadowUniforms.ScaleY); }
        set { material.SetFloat(OffsetShadowUniforms.ScaleY, value); }
    }

    public void Awake() {
        material = GetComponent<Renderer>().material;
    }

    public void Update()
    {
        ShaderSkewHorizontal = horizontalSkew;
        ShaderSkewVertical = verticalSkew;
        ShaderScaleX = scaleX;
        ShaderScaleY = scaleY;
    }
}
