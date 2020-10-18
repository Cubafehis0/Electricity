using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TropFloor : MonoBehaviour
{
    //控制玻璃的破碎
    public bool canBroken;//是否破碎
    public float liveTime;//破碎所需时间
    public Material material;//碎片的材质
    public Vector3[] vertices;//碎片的顶点坐标

    public AudioClip brokenClip;//破碎声音
    public float baseForce;//碎片破碎时所受力的大小基准
    float time=0;//已经破碎的时间
    // Start is called before the first frame update
   

    // Update is called once per frame
    void FixedUpdate()
    {
        if(canBroken)
        {
            time += Time.deltaTime;
            if (time>liveTime)
            {
                CreatePieces();
                GameObject.Destroy(gameObject);
            }
        }
    }
    void CreatePieces()
    {
        //创建碎片
        GameObject[] pieces = new GameObject[vertices.Length - 1];
        int verticesLength = vertices.Length;
        for (int i = 0; i < verticesLength - 1; i++)
        {
            //根据顶点坐标创建三角形面片
            pieces[i] = new GameObject("pieces");
            pieces[i].transform.position = transform.position+new Vector3(-0.5f,-0.5f);//碎片的位置和玻璃的位置有偏差
            pieces[i].transform.rotation = transform.rotation;
            pieces[i].transform.localScale = transform.localScale;
            pieces[i].transform.parent = transform.parent;
            pieces[i].AddComponent<MeshFilter>();
            pieces[i].AddComponent<MeshRenderer>();//增加组件
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
            if(0==i)
            {
                //为第一个碎片加上破碎音效
                AudioSource source= pieces[i].AddComponent<AudioSource>();
                source.clip = brokenClip;
                source.loop = false;
                source.volume = 0.2f;
                source.Play();
            }
        }
        for (int i = 0; i < pieces.Length; i++)
        {
            //碎片的延迟销毁
            GameObject.Destroy(pieces[i], 1f);
        }


    }
}
