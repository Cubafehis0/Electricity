using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject dropDustEffect;
    public Rigidbody2D rig;
    public Vector2 speed = new Vector2(0, 0);
    public bool IsFloat
    {
        set;
        get;
    }
    public bool IsOnPlat
    {
        set;
        get;
    } = false;
    
    public bool IsReachDes
    {
        set;
        get;
    } = false;
    public Vector2 Collider2DExtents
    {
        set;
        get;
    }
    public Vector2 ColCen
    {
        set;
        get;
    }
    public bool HasPick
    {
        set;
        get;
    } = false;
    public int Layer
    {
        get;
        set;
    }


    public bool isOnOther//一个在平台上，另一个相互重叠则都在平台上
    {
        set;
        get;

    }


    public Relay PickRelay
    {
        set;
        get;
    } = null;

    public void Move()
    {
        rig.MovePosition(speed * Time.deltaTime + (Vector2)rig.transform.position);
    }

    



}
