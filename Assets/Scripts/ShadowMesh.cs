using UnityEngine;

public class ShadowMesh : MonoBehaviour {
    public Vector3 positionOffset;
    public Vector3 scale;

    public Mesh mesh;
    public ShadowController shadowController;

    private Material material;

    public void Start() {
        material = GetComponent<Renderer>().material;
    }

    public Vector3 getPosition() {
        return transform.position + positionOffset;
    }

    public Vector3 getScale() {
        return transform.localScale + scale;
    }

    public void Update() {
        Matrix4x4 lightSpaceMatrix = shadowController.getLightVPMatrix();
        material.SetMatrix("_LightSpace", lightSpaceMatrix);
    }
}
