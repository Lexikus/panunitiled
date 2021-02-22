using UnityEngine;

public class SkewShadowController : MonoBehaviour
{
    private Material material;
    private Shader shader;
    public float horizontalSkew = 0.0f;
    public float verticalSkew = 0.0f;
    public float scaleX = 1.0f;
    public float scaleY = 1.0f;

    private float ShaderSkewHorizontal {
        get { return material.GetFloat(ShadowConfig.HorizontalSkewUniform); }
        set { material.SetFloat(ShadowConfig.HorizontalSkewUniform, value); }
    }

    private float ShaderSkewVertical {
        get { return material.GetFloat(ShadowConfig.VerticalSkewUniform); }
        set { material.SetFloat(ShadowConfig.VerticalSkewUniform, value); }
    }

    private float ShaderScaleX {
        get { return material.GetFloat(ShadowConfig.ScaleXUniform); }
        set { material.SetFloat(ShadowConfig.ScaleXUniform, value); }
    }

    private float ShaderScaleY {
        get { return material.GetFloat(ShadowConfig.ScaleYUniform); }
        set { material.SetFloat(ShadowConfig.ScaleYUniform, value); }
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
