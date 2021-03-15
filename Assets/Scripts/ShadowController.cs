using UnityEngine;

public class ShadowController : MonoBehaviour {

    [Header("Global 2D shadow settings")]
    public Color shadowColor;

    [Header("Complex shadow settings")]
    public bool enableComplexShadow = false;
    [Tooltip("Required to calculate the depth distance from the camera eye")]
    public Camera cam;
    [Tooltip("Required to calculate the depth distance from the fake directional eye")]
    public Transform shadowEye;
    [Tooltip("Show depth texture")]
    public bool debug = false;

    // TODO: create Material wit the ShadowMap Material and RenderTexture at runtime.
    public Material material;
    public RenderTexture renderTexture;

    [Tooltip("Increase shadow range")]
    public float size = 1f;

    private ShadowMesh[] meshes;
    private Matrix4x4 sceneViewMatrix = Matrix4x4.identity;
    private Matrix4x4 lightViewMatrix = Matrix4x4.identity;
    private Matrix4x4 projectionMatrix = Matrix4x4.identity;

    public void Start() {
        meshes = FindObjectsOfType<ShadowMesh>();
    }

    public void Update() {
        if(!enableComplexShadow) {
            return;
        }
        CreateVPMatrices();
        Blit();
    }

    public Matrix4x4 GetLightViewMatrix() {
        return lightViewMatrix;
    }

    public Matrix4x4 GetLightProjectionMatrix() {
        return projectionMatrix;
    }

    public Matrix4x4 GetLightVPMatrix() {
        return projectionMatrix * lightViewMatrix;
    }

    public void OnGUI() {
        if (!debug) {
            return;
        }

        if (Event.current.type.Equals(EventType.Repaint)) {
            Graphics.DrawTexture(new Rect(0, 0, 256, 256), renderTexture);
        }
    }

    private void CreateVPMatrices() {
        projectionMatrix = Matrix4x4.Ortho(
            -1f * size,
            1f * size,
            -1f * size,
            1f * size,
            ShadowConfig.ZNear,
            ShadowConfig.ZFar
        );

        sceneViewMatrix = cam.worldToCameraMatrix;

        lightViewMatrix = shadowEye.worldToLocalMatrix;
        lightViewMatrix.m20 *= -1f;
        lightViewMatrix.m21 *= -1f;
        lightViewMatrix.m22 *= -1f;
        lightViewMatrix.m23 *= -1f;
    }

    private void Blit() {
        RenderTexture prevRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        material.SetPass(0);

        GL.PushMatrix();
        GL.LoadProjectionMatrix(projectionMatrix);

        material.SetMatrix(ShadowConfig.LightViewMatrixUniform, lightViewMatrix);

        GL.Clear(true, true, Color.white);

        foreach (var mesh in meshes) {
            if(mesh.mesh == null) {
                continue;
            }
            Matrix4x4 objectMatrix = Matrix4x4.TRS(mesh.GetPosition(), mesh.GetRotation(), mesh.GetScale());
            Graphics.DrawMeshNow(mesh.mesh, objectMatrix);
        }

        GL.PopMatrix();

        RenderTexture.active = prevRT;
    }
}