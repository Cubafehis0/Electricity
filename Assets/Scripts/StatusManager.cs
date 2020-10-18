using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Animations;
using UnityEngine.SceneManagement;

public class StatusManager : MonoBehaviour
{
    //游戏状态的管理
    public Transform cameraPos;//讲画的mesh的中心移动到相机的位置
    public float time;//暂停的时间
    float curTime;//已经暂停的时间
    float pauseBeginTime;//暂停开始的时间
    bool isPause;//用于控制游戏的暂停和继续
    bool isLoad;//是否播放初始动画
    private void Start()
    {
        curTime = 0;
        GameStatus.IsPass = false;
        GameStatus.IsAlive = true;
        isPause = false;
        if (GameStatus.IsReady)
            isLoad = true;
        else isLoad = false;//在准备界面不播放初始动画
    }

    void Update()
    {
        Ready();//控制准备界面
        JudgeStatus();//根据游戏状态控制游戏进程
        
    }
    void Ready()
    {
        if(!GameStatus.IsReady)
        {
            if (Input.anyKeyDown)
            {
                GameStatus.IsReady = true;
                GameStatus.IsPass = true;
            }
        }
        
    }
    void JudgeStatus()
    {
        if (!GameStatus.IsAlive || GameStatus.IsPass)
        {
            Time.timeScale = 0;//暂停
            if (!isPause)
            {
                isPause = true;
                pauseBeginTime = Time.realtimeSinceStartup;//记录暂停时的时间
            }
            curTime = Time.realtimeSinceStartup - pauseBeginTime;
            CreateEndAni();//加载结束动画
            if (curTime > time)
            {
                Time.timeScale = 1;//继续游戏
                if (GameStatus.IsPass)
                {
                    GameStatus.Lever++;//下一关
                    GameStatus.IsPass = false;
                }
                else if (!GameStatus.IsAlive)
                {
                    GameStatus.IsAlive = true;//重来
                }
                SceneManager.LoadScene(GameStatus.Lever);
            }
        }
        else if (isLoad)//控制开始动画的播放
        {
            Time.timeScale = 0;//暂停
            if (!isPause)
            {
                isPause = true;
                pauseBeginTime = Time.realtimeSinceStartup;
            }
            curTime = Time.realtimeSinceStartup - pauseBeginTime;
            CreateBeginAni();//画mesh
            if (curTime > time)
            {
                Time.timeScale = 1;//继续
                isPause = false;
                isLoad = false;
            }
        }
    }
    void CreateBeginAni()
    {
        Mesh mesh = new Mesh();
        int num = (int)(time / 0.02);//三角形总个数
        int cnt = num-(int)(curTime / 0.02);//去掉部分三角形剩余的个数
        if(cnt<0)//防止帧数过低时cnt小于零的情况
        {
            cnt = 0;
        }
        Vector3[] vertices = new Vector3[cnt+2];//顶点
        this.transform.position = new Vector3(cameraPos.position.x, cameraPos.position.y, -5);
        int[] triangles = new int[3 * cnt];
        float angle = Mathf.Acos(-1) / num * 2;//每次移动的角度
        for (int i = 0; i <= cnt; i++)
        {
            vertices[i] = new Vector3(20 * Mathf.Cos(angle * i), 20 * Mathf.Sin(angle * i));
        }
        vertices[cnt + 1] = new Vector3(0, 0);//最后一个顶点在中心
        for (int i = 0, j = 0; i < cnt; i++)
        {
            triangles[j++] = i;
            triangles[j++] = cnt + 1;
            triangles[j++] = i + 1;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mesh;
    }
    void CreateEndAni()
    {
        Mesh mesh = new Mesh();
        int cnt = (int)(curTime / 0.02) + 1;//三角形个数
        Vector3[] vertices = new Vector3[cnt + 2];
        int num = (int)(time /0.02);//三角形总个数
        this.transform.position = new Vector3(cameraPos.position.x, cameraPos.position.y, -5);
        int[] triangles = new int[3 * cnt];
        float angle = Mathf.Acos(-1) / num * 2;
        for (int i = 0; i <= cnt; i++)
        {
            vertices[i] = new Vector3(20 * Mathf.Cos(angle * i), 20 * Mathf.Sin(angle * i));
        }
        vertices[cnt + 1] = new Vector3(0, 0);
        for (int i = 0, j = 0; i < cnt; i++)
        {
            triangles[j++] = i;
            triangles[j++] = cnt + 1;
            triangles[j++] = i + 1;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        GetComponent<MeshFilter>().mesh = mesh;
    }

}
