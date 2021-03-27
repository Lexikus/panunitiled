using UnityEngine;

public class ShadowMesh : MonoBehaviour {
    public Vector3 positionOffset;
    public Vector3 scale;
    public Vector3 rotationOffset;

    public Mesh mesh;
    public ShadowController shadowController;

    private Material material;

    public void Start() {
        material = GetComponent<Renderer>().material;
    }

    public Vector3 GetPosition() {
        return transform.position + positionOffset;
    }

    public Vector3 GetScale() {
        return transform.localScale + scale;
    }

    public Quaternion GetRotation() {
        return Quaternion.Euler(rotationOffset);
    }
}
