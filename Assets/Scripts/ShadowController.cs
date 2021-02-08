using UnityEngine;

public class ShadowController : MonoBehaviour {

    public bool debug = false;

    public Camera cam;
    public Material material;
    public RenderTexture renderTexture;

    public Transform view;
    public float size = 1f;

    private ShadowMesh[] meshes;
    private Matrix4x4 sceneViewMatrix = Matrix4x4.identity;
    private Matrix4x4 lightViewMatrix = Matrix4x4.identity;
    private Matrix4x4 projectionMatrix = Matrix4x4.identity;


    public void Start() {
        meshes = FindObjectsOfType<ShadowMesh>();
    }

    public void Update() {
        CreateVPMatrices();
        Blit();
    }

    public Matrix4x4 getLightViewMatrix() {
        return lightViewMatrix;
    }

    public Matrix4x4 getLightProjectionMatrix() {
        return projectionMatrix;
    }

    /**
     * Returns the calculated view projection matrix. It is recommended to cache the value though.
     */
    public Matrix4x4 getLightVPMatrix() {
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

    /**
     * Matrices are created every update interavl due to prevent flickering.
     */
    private void CreateVPMatrices() {
        // Create a projection matrix
        projectionMatrix = Matrix4x4.Ortho(
            -1f * size,
            1f * size,
            -1f * size,
            1f * size,
            0.1f,
            100f
        );

        // Create the scene view matric
        sceneViewMatrix = cam.worldToCameraMatrix;

        // Create the light view matrix
        lightViewMatrix = view.worldToLocalMatrix;
        lightViewMatrix.m20 *= -1f;
        lightViewMatrix.m21 *= -1f;
        lightViewMatrix.m22 *= -1f;
        lightViewMatrix.m23 *= -1f;
    }

    private void Blit() {
        // Remember the current texture and set our own as "active".
        RenderTexture prevRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        // Set material as "active". Without this, Unity editor will freeze.
        material.SetPass(0);

        // Push the projection matrix
        GL.PushMatrix();
        GL.LoadProjectionMatrix(projectionMatrix);

        material.SetMatrix("_lightViewMatrix", lightViewMatrix);

        // Clear the texture
        GL.Clear(true, true, Color.white);


        foreach (var mesh in meshes)
        {
            if(mesh.mesh == null) {
                continue;
            }
            // Create the model matrix
            Matrix4x4 objectMatrix = Matrix4x4.TRS(mesh.getPosition(), mesh.transform.rotation, mesh.getScale());

            // Draw the mesh
            Graphics.DrawMeshNow(mesh.mesh, objectMatrix);
        }

        // Pop the projection matrix to set it back to the previous one
        GL.PopMatrix();

        // Re-set the RenderTexture to the last used one
        RenderTexture.active = prevRT;
    }
}