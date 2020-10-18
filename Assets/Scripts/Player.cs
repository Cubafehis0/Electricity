using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //记录玩家的信息
    public GameObject dropDustEffect;//落地灰尘动画
    public Rigidbody2D rig;
    public Vector2 speed = new Vector2(0, 0);//玩家的速度
    public bool IsFloat//是否浮空
    {
        set;
        get;
    }
    public bool IsOnPlat//是否在平台上
    {
        set;
        get;
    } = false;
    
    public bool IsReachDes//是否抵达终点
    {
        set;
        get;
    } = false;
    public Vector2 Collider2DExtents//碰撞体的长宽
    {
        set;
        get;
    }
    public Vector2 ColCen//碰撞体中心
    {
        set;
        get;
    }
    public bool HasPick//是否拾取继电器
    {
        set;
        get;
    } = false;
    public int Layer//玩家所在的层
    {
        get;
        set;
    }


    public bool isOnOther//一个玩家是否在另一个上面，此标记防止两个玩家在升降台上相互重叠的情况
    {
        set;
        get;
    }


    public Relay PickRelay//记录拾取的继电器
    {
        set;
        get;
    } = null;


    



}
