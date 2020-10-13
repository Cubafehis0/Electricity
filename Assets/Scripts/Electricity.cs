using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    public Transform palyer1;
    public Transform player2;
    public Transform[] conductors = new Transform[5];
    public Transform[] linkConductors = new Transform[6];
    int conductorSize;
    public float liveTime;
    public float notLinkTime;
    bool[] relaysCanLinks = new bool[5];//记录每个继电器可以被连接，如果在其中一条路上不能连接，那在所有路上都不能连接
    public float distance;//电流之间距离
    bool isLink;   //是否串通
    // Start is called before the first frame update
    void Start()
    {
        conductorSize = conductors.Length;
        notLinkTime = 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        JudgeDistance();
        if (!isLink)
        {
            notLinkTime += Time.deltaTime;
        }
        if(notLinkTime>=liveTime)
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
        DFS(0);      
    }
    void DFS(int index)
    {
        if(index==conductorSize)
        {
            return;
        }     
        Vector2 vecDis;
        vecDis = (Vector2)(conductors[index].position - player2.position);

        if (vecDis.magnitude <= distance)
        {
            isLink = true;
            notLinkTime = 0;
            return;
        }
        int[] nearRelaysIndex = new int[5];//存储近的继电器的小标
        int canLinkSizes=FindRelays(index,nearRelaysIndex);//找到可以连接的继电器并排序返回其下标        
        if (canLinkSizes == 0)
            return;
        for(int i=0;i<canLinkSizes;i++)
        {
            relaysCanLinks[nearRelaysIndex[i]] = false;//这个不能连接
            DFS(nearRelaysIndex[i]);
            if (isLink)
                return;
        }
        return;
    }
    int FindRelays(int sourse,int []a)
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
}

