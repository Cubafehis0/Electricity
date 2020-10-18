using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    public Transform palyer1;
    public Transform player2;
    public Transform[] conductors = new Transform[5];
    public Transform[] linkConductors = new Transform[6];//相连的继电器
    int conductorSize;
    public float liveTime;
    public float notLinkTime;

    
    bool[] relaysCanLinks = new bool[5];//记录每个继电器可以被连接，如果在其中一条路上不能连接，那在所有路上都不能连接
    public float distance;//电流之间距离
    bool isLink;   //是否串通
    int linkCnt;//相连的个数

    public int simplePointNum;//取点个数
    public int bezierNum;//贝塞尔曲线取点个数
    public float offSet;//曲线偏差
    public float width;//曲线长度
    public Material material;
    // Start is called before the first frame update
    void Start()
    {
        conductorSize = conductors.Length;
        notLinkTime = 0;
        isLink = true;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        JudgeDistance();
        if (!isLink)
        {
            notLinkTime += Time.deltaTime;
        }
        else
        {
            DrawLine();
        }
        if (notLinkTime >= liveTime)
        {
            GameStatus.IsAlive = false;
        }

    }
    void JudgeDistance()
    {
        isLink = false;
        for (int i = 0; i < conductorSize; i++)
        {
            relaysCanLinks[i] = true;
        }
        //两个导体之间的距离
        linkCnt = 0;
        linkConductors[linkCnt++] = palyer1;
        DFS(0);
    }
    void DFS(int index)
    {
        if (index == conductorSize)
        {
            return;
        }
        Vector2 vecDis;
        vecDis = (Vector2)(conductors[index].position - player2.position);

        if (vecDis.magnitude <= distance)
        {
            isLink = true;
            notLinkTime = 0;
            linkConductors[linkCnt++] = player2;
            return;
        }
        int[] nearRelaysIndex = new int[5];//存储近的继电器的小标
        int canLinkSizes = FindRelays(index, nearRelaysIndex);//找到可以连接的继电器并排序返回其下标        
        if (canLinkSizes == 0)
            return;


        for (int i = 0; i < canLinkSizes; i++)
        {
            linkConductors[linkCnt++] = conductors[nearRelaysIndex[i]];
            relaysCanLinks[nearRelaysIndex[i]] = false;//这个不能连接
            DFS(nearRelaysIndex[i]);
            if (isLink)
                return;
            linkCnt--;
        }
        return;
    }
    int FindRelays(int sourse, int[] a)
    {
        int cnt = 0;
        if (conductorSize > 1)
        {
            for (int i = 1; i < conductorSize; i++)
            {
                if (relaysCanLinks[i])
                {
                    Vector2 vecDis = (Vector2)(conductors[sourse].position - conductors[i].position);
                    if (vecDis.magnitude < distance)
                    {
                        a[cnt++] = i;
                    }
                }
            }
            if (cnt > 1)
            {
                for (int i = 0; i < cnt - 1; i++)
                {
                    int min = i;
                    float minDis = ((Vector2)(conductors[sourse].position - conductors[a[min]].position)).magnitude;
                    for (int j = i + 1; j < cnt; j++)
                    {
                        Vector2 vecDis = (Vector2)(conductors[sourse].position - conductors[a[i]].position);
                        if (vecDis.magnitude < minDis)
                        {
                            minDis = vecDis.magnitude;
                            min = j;
                        }
                    }
                    if (min != i)
                    {
                        int tmp = a[i];
                        a[i] = a[min];
                        a[min] = tmp;
                    }
                }
            }
        }
        return cnt;

    }
    void DrawLine()
    {
        for(int i=0;i<linkCnt-1;i++)
        {
            Create(linkConductors[i].transform.position,linkConductors[i+1].transform.position);
        }
    }
    void Create(Vector2 startPos,Vector2 endPos)
    {

        Vector3[] vertices = new Vector3[simplePointNum * bezierNum+ 1];
        int[] triangles = new int[3 * (vertices.Length - 2)];
        Bezier(startPos,endPos,vertices);
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;       
        for (int j = 0; j < triangles.Length; j++)
        {
            if (j % 6 < 3)
            {
                triangles[j] = j / 3 + j % 6;
            }
            else if (j % 6 == 3)
            {
                triangles[j] = 1 + j / 3;
            }
            else if (j % 6 == 4)
            {
                triangles[j] = j / 3;
            }
            else if (j % 6 == 5)
            {
                triangles[j] = 2 + j / 3;
            }
        }
        mesh.triangles = triangles;
        Graphics.DrawMesh(mesh, new Vector3(0, 0, -5), Quaternion.identity, material,0);
    }
    void Bezier(Vector2 endPos,Vector2 startPos,Vector3[] vertices)
    {
        int cnt = 0;
        Vector2 line = endPos - startPos;
        Vector2 vectorLine = new Vector2(-line.y, line.x).normalized;
        float distance = line.magnitude;
        Vector2[] points = new Vector2[simplePointNum + 1];
        points[0] = startPos;
        points[simplePointNum] = endPos;
        for (int i = 1; i < simplePointNum; i++)
        {
            float ratio = (float)i / simplePointNum;//比例
            float off = offSet * (float)(Random.value - 0.5) * distance;
            points[i] = (1 - ratio) * startPos + ratio * endPos + off * vectorLine;
        }
        for (int i = 0; i <=simplePointNum -1; i += 2)
        {
            Vector2 point;
            float ratio1;
            float ratio2;
            for (int j = 0; j < bezierNum; j++)
            {
                ratio1 = (float)j / bezierNum;
                ratio2 = 1 - ratio1;
                point = ratio1 * ratio1 * points[i + 2] + 2 * ratio1 * ratio2 * points[i + 1] + ratio2 * ratio2 * points[i];
                vertices[cnt++] = point;
                vertices[cnt++] = point + width * vectorLine;
            }
        }
        vertices[cnt++] = endPos;
    }

}

