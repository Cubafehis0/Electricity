using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relay : MonoBehaviour
{
    public bool Movable;
    public bool hasPick;
    public Rigidbody2D rig;
    public Vector2 speed;
    public Player picker;
    public Vector2 offSet;//偏离picker的位置
}
