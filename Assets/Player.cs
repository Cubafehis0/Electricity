using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public Rigidbody2D rig;

    public Vector2 speed = new Vector2(0, 0);
    public bool IsFloat
    {
        set;
        get;
    }

    public Vector2 Collider2DExtents
    {
        set;
        get;
    }
    public Vector2 Collider2DCenter
    {
        set;
        get;
    }

    public void Move()
    {

        rig.MovePosition(speed * Time.deltaTime + (Vector2)rig.transform.position);
    }




}
