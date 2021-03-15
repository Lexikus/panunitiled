using UnityEngine;

public class ShadowReceiver : MonoBehaviour {
    public ShadowController shadowController;
    private Material material;

    public void Start() {
        material = GetComponent<Renderer>().material;
    }

    public void Update() {
        Matrix4x4 lightSpaceMatrix = shadowController.GetLightVPMatrix();
        material.SetMatrix(ShadowConfig.LightSpaceUniform, lightSpaceMatrix);
    }
}
