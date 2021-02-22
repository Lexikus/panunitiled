using UnityEngine;

public class OffsetShadowController : MonoBehaviour {
    private Material material;
    private Shader shader;

    public float offsetX = 0.0f;
    public float offsetY = 0.0f;

    private float ShaderOffsetX {
        get { return material.GetFloat(ShadowConfig.OffsetXUniform); }
        set { material.SetFloat(ShadowConfig.OffsetXUniform, value); }
    }

    private float ShaderOffsetY {
        get { return material.GetFloat(ShadowConfig.OffsetYUniform); }
        set { material.SetFloat(ShadowConfig.OffsetYUniform, value); }
    }

    public void Awake() {
        material = GetComponent<Renderer>().material;
    }

    public void Update() {
        ShaderOffsetX = offsetX;
        ShaderOffsetY = offsetY;
    }
}
