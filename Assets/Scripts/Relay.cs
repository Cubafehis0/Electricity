using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relay : MonoBehaviour
{
    //记录继电器的信息
    public bool hasPick;//是否被拾取，初始设为true继电器固定
    public Rigidbody2D rig;//继电器的碰撞体
    public Vector2 speed;//继电器移动的速度
    public Player picker;//继电器的拾取者
    public Vector2 offSet;//偏离picker的位置
}
