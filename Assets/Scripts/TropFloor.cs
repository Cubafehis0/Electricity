using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TropFloor : MonoBehaviour
{
    public bool canDestory;
    public float liveTime;
    public Material material;
    public Vector3[] vertices;
    public float baseForce;
    float time=0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(canDestory)
        {
            time += Time.deltaTime;
            if(time>liveTime)
            {
                CreatePieces();
                GameObject.Destroy(gameObject);
            }
        }
    }
    void CreatePieces()
    {
        GameObject[] pieces = new GameObject[vertices.Length - 1];
        int verticesLength = vertices.Length;
        for (int i = 0; i < verticesLength - 1; i++)
        {
            pieces[i] = new GameObject("pieces");
            pieces[i].transform.position = transform.position+new Vector3(-0.5f,-0.5f);
            pieces[i].transform.rotation = transform.rotation;
            pieces[i].transform.localScale = transform.localScale;
            pieces[i].transform.parent = transform.parent;
            pieces[i].AddComponent<MeshFilter>();
            pieces[i].AddComponent<MeshRenderer>();
            pieces[i].GetComponent<MeshRenderer>().sortingOrder = -2;
            pieces[i].GetComponent<MeshRenderer>().material = material;
            pieces[i].AddComponent<Rigidbody2D>();
            Vector3[] piecesVertices = new Vector3[3];
            piecesVertices[0] = vertices[i];
            piecesVertices[1] = vertices[(i + 1) % (verticesLength - 1)];
            piecesVertices[2] = vertices[verticesLength - 1];
            int[] piecesTriangles = { 0, 1, 2 };
            Vector2[] piecesuvs = new Vector2[3];
            for (int j = 0; j < 3; j++)
            {
                piecesuvs[j] = piecesVertices[j];
            }
            Mesh mesh = new Mesh();
            mesh.vertices = piecesVertices;
            mesh.triangles = piecesTriangles;
            mesh.uv = piecesuvs;
            pieces[i].GetComponent<MeshFilter>().mesh = mesh;
            Rigidbody2D rig = pieces[i].GetComponent<Rigidbody2D>();
            
            Vector2 force = baseForce * ((piecesVertices[0] + piecesVertices[1]) / 2 - piecesVertices[2]-new Vector3(Random.value,Random.value));
            rig.AddForce(force);
        }
        for (int i = 0; i < pieces.Length; i++)
        {
            GameObject.Destroy(pieces[i], 1f);
        }


    }
}
