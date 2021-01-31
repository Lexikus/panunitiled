using UnityEngine;

public class ShadowController : MonoBehaviour
{
    [System.Serializable]
    public struct MeshData
    {
        public Mesh mesh;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }

    public Material material;
    public RenderTexture rt;
    public Transform go;

    public MeshData mesh;

    float angle;
    Matrix4x4 objectMatrix;
    public Matrix4x4 viewMatrix;

    void Update()
    {
        Quaternion rotation = Quaternion.Euler(mesh.rotation);

        // Create the object transform matrix
        objectMatrix = Matrix4x4.TRS(mesh.position, rotation, mesh.scale);

        Blit();
    }

    void Blit()
    {
        // Create an projection matrix
        // Matrix4x4 projectionMatrix = Matrix4x4.Ortho(-1, 1, -1, 1, 0.1f, 15);
        Matrix4x4 projectionMatrix = Matrix4x4.Perspective(60, 1f, 0.1f, 15);
        viewMatrix = go.worldToLocalMatrix;


        // Because there's some switching back and forth between cameras
        if (Camera.current != null)
            projectionMatrix *= Camera.current.worldToCameraMatrix.inverse;

        // Remember the current texture and set our own as "active".
        RenderTexture prevRT = RenderTexture.active;
        RenderTexture.active = rt;

        // Set material as "active". Without this, Unity editor will freeze.
        material.SetPass(0);

        // Push the projection matrix
        GL.PushMatrix();
        GL.LoadProjectionMatrix(projectionMatrix);

        // It seems that the faces are in a wrong order, so we need to flip them
        GL.invertCulling = true;

        // Clear the texture
        GL.Clear(true, true, Color.white);

        // Draw the mesh!
        Graphics.DrawMeshNow(mesh.mesh, viewMatrix * objectMatrix);

        // Pop the projection matrix to set it back to the previous one
        GL.PopMatrix();

        // Revert culling
        GL.invertCulling = false;

        // Re-set the RenderTexture to the last used one
        RenderTexture.active = prevRT;
    }

    // Just for live preview
    private void OnGUI()
    {
        if (Event.current.type.Equals(EventType.Repaint))
        {
            Graphics.DrawTexture(new Rect(0, 0, 256, 256), rt);
        }
    }
}