using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameStatus : MonoBehaviour
{
    public static int Lever//关卡
    {
        set;
        get;
    } = 0;
    public static bool IsReady//准备界面的控制
    {
        set;
        get;
    } = false;

    public static bool IsPass//是否抵达终点
    {
        set;
        get;

    } = false;

    public static bool IsAlive//玩家是否存活
    {
        set;
        get;
    } = true;
}
