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
    private Renderer rend;
    private Shader shader;
    public float horizontalSkew = 0.0f;
    public float verticalSkew = 0.0f;
    public float scaleX = 1.0f;
    public float scaleY = 1.0f;

    private float ShaderSkewHorizontal {
        get { return rend.material.GetFloat(OffsetShadowUniforms.HorizontalSkew); }
        set { rend.material.SetFloat(OffsetShadowUniforms.HorizontalSkew, value); }
    }

    private float ShaderSkewVertical {
        get { return rend.material.GetFloat(OffsetShadowUniforms.VerticalSkew); }
        set { rend.material.SetFloat(OffsetShadowUniforms.VerticalSkew, value); }
    }

    private float ShaderScaleX {
        get { return rend.material.GetFloat(OffsetShadowUniforms.ScaleX); }
        set { rend.material.SetFloat(OffsetShadowUniforms.ScaleX, value); }
    }

    private float ShaderScaleY {
        get { return rend.material.GetFloat(OffsetShadowUniforms.ScaleY); }
        set { rend.material.SetFloat(OffsetShadowUniforms.ScaleY, value); }
    }

    public void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void Update()
    {
        ShaderSkewHorizontal = horizontalSkew;
        ShaderSkewVertical = verticalSkew;
        ShaderScaleX = scaleX;
        ShaderScaleY = scaleY;
    }
}
