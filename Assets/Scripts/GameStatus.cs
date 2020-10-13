using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameStatus : MonoBehaviour
{
    public static int Lever
    {
        set;
        get;
    } = 0;

    public static bool IsPass
    {
        set;
        get;

    } = false;

    public static bool IsAlive
    {
        set;
        get;
    } = true;
}
