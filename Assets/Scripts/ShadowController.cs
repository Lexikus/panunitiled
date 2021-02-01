using UnityEngine;

public class ShadowController : MonoBehaviour {

    [System.Serializable]
    public struct MeshData {
        public Mesh mesh;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }

    public bool debug = false;
    public Material material;
    public RenderTexture renderTexture;
    public Transform view;

    public MeshData[] meshs;

    private Matrix4x4 viewMatrix = Matrix4x4.identity;
    private Matrix4x4 projectionMatrix = Matrix4x4.identity;

    public void Update() {
        CreateVPMatrices();
        Blit();
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
        // Create an projection matrix
        projectionMatrix = Matrix4x4.Ortho(-1, 1, -1, 1, 0.1f, 15);
        // projectionMatrix = Matrix4x4.Perspective(60, 1f, 0.1f, 15);

        // Create the view matrix
        viewMatrix = view.worldToLocalMatrix;
        viewMatrix.m20 *= -1f;
        viewMatrix.m21 *= -1f;
        viewMatrix.m22 *= -1f;
        viewMatrix.m23 *= -1f;
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

        material.SetMatrix("_viewMatrix", viewMatrix);

        // Clear the texture
        GL.Clear(true, true, Color.white);


        foreach (var mesh in meshs)
        {
            Quaternion rotation = Quaternion.Euler(mesh.rotation);
            // Create the model matrix
            Matrix4x4 objectMatrix = Matrix4x4.TRS(mesh.position, rotation, mesh.scale);

            // Draw the mesh!
            Graphics.DrawMeshNow(mesh.mesh, objectMatrix);
        }

        // Pop the projection matrix to set it back to the previous one
        GL.PopMatrix();

        // Re-set the RenderTexture to the last used one
        RenderTexture.active = prevRT;
    }
}