using UnityEngine;

[System.Serializable]
public class TerrainMeshVisualizer
{
    public Material material;

    private const int tileRes = 128;
    private Mesh _tileMesh;


    /// <summary>
    /// Creates a visualizer with the specified material
    /// </summary>
    public TerrainMeshVisualizer(Material Material)
    {
        if (Material == null)
            throw new System.ArgumentNullException("Material");
        material = Material;
    }

    /// <summary>
    /// Draws a temporary tile mesh representation with the material and the given parameters for this frame only
    /// </summary>
    public void DrawVisualization(Vector3 origin, Vector2 vizSize, Vector2 vizRes, float height)
    {
        // Prepare tile setup
        vizRes.x = Mathf.Max(vizRes.x, tileRes);
        vizRes.y = Mathf.Max(vizRes.y, tileRes);
        int tilesX = Mathf.CeilToInt((float)vizRes.x / tileRes), tilesY = Mathf.CeilToInt((float)vizRes.y / tileRes);

        // Prepare material
        material.SetVector("_WorldSpaceUVTrans", new Vector4(1f / vizSize.x, 1f / vizSize.y, -origin.x / vizSize.x, -origin.z / vizSize.y));

        // Prepare tile transformation
        Vector3 tileScale = new Vector3((float)vizSize.x / vizRes.x, height, (float)vizSize.y / vizRes.y);
        Vector2 tileSize = new Vector2(vizSize.x / tilesX, vizSize.y / tilesY);

        // Get tile mesh
        Mesh tileMesh = GetTileMesh(height);

        // Draw tile grid
        for (int xTile = 0; xTile < tilesX; xTile++)
        {
            for (int yTile = 0; yTile < tilesY; yTile++)
            {
                Matrix4x4 drawMatrix = Matrix4x4.TRS(origin + new Vector3((xTile + 0.5f) * tileSize.x, 0, (yTile + 0.5f) * tileSize.y), Quaternion.identity, tileScale);
                Graphics.DrawMesh(tileMesh, drawMatrix, material, 0);
            }
        }
    }

    private Mesh GetTileMesh(float height)
    {
        if (_tileMesh == null)
            _tileMesh = CreatePlane(tileRes, tileRes, new Rect(0, 0, 1, 1), true);
        _tileMesh.bounds = new Bounds(new Vector3(0, height / 2, 0), new Vector3(tileRes, height, tileRes));
        return _tileMesh;
    }

    #region Utility

    private static Mesh CreatePlane(int res, float size, Rect uvRect, bool calcUVs)
    {
        int segments = res - 1;
        float segWidth = size / segments;

        // Initialize vertex data
        Vector3[] vertices = new Vector3[res * res];
        Vector2[] uvs = calcUVs ? new Vector2[res * res] : null;
        Vector3[] normals = new Vector3[res * res];
        Vector4[] tangents = new Vector4[res * res];
        int[] triangles = new int[segments * segments * 6];

        Vector4 uniformTangent = new Vector4(1, 0, 0, -1);
        Vector4 uniformNormal = Vector3.up;

        // Build vertices and their data
        for (int x = 0; x < res; x++)
        {
            for (int y = 0; y < res; y++)
            {
                int index = y + x * res;
                vertices[index] = new Vector3(x * segWidth - size / 2, 0, y * segWidth - size / 2);
                if (calcUVs) uvs[index] = new Vector2(uvRect.x + uvRect.width * (float)x / segments, uvRect.y + uvRect.height * (float)y / segments);
                normals[index] = uniformNormal;
                tangents[index] = uniformTangent;
            }
        }

        // Build triangles
        int num = 0;
        for (int x = 0; x < segments; x++)
        {
            for (int y = 0; y < segments; y++)
            {
                int index = y + x * res;

                triangles[num++] = index;
                triangles[num++] = index + 1;
                triangles[num++] = index + res;

                triangles[num++] = index + res;
                triangles[num++] = index + 1;
                triangles[num++] = index + res + 1;
            }
        }

        // Build mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        if (calcUVs) mesh.uv = uvs;
        mesh.normals = normals;
        mesh.tangents = tangents;
        mesh.triangles = triangles;

        return mesh;
    }

    #endregion
}