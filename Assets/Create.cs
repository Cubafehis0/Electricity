using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create : MonoBehaviour
{
    public Vector3[] vertices;
    public int[] triangles;
    public Texture texture;

    private void Start()
    {
        CreatePieces();
    }
    [ContextMenu("CreatePieces")]
    void CreatePieces()
    {
        MeshFilter filter = GetComponent<MeshFilter>();
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.material.mainTexture = texture;
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        Vector2[] uvs = new Vector2[vertices.Length];
        for(int i=0;i<vertices.Length;i++)
        {
            uvs[i] = vertices[i];
        }
        mesh.uv = uvs;
        filter.mesh = mesh;

    }
}
